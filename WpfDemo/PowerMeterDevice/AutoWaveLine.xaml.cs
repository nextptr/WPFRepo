using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PowerMeterDevice
{
    /// <summary>
    /// AutoWaveLine.xaml 的交互逻辑
    /// </summary>
    public partial class AutoWaveLine : UserControl
    {
        public Queue<double> Points = new Queue<double>();
        protected double MaxVal = 0.0;
        protected double MinVal = 0.0;
        protected double AvgVal = 0.0;

        protected Dictionary<int, Label> YAxis = new Dictionary<int, Label>();
        protected Dictionary<int, Label> XAxis = new Dictionary<int, Label>();
        protected Dictionary<int, double> XAxisTimeCount = new Dictionary<int, double>();

        //统计
        public void SetZeroState(bool sta)
        {
            labZero.Text = sta.ToString();
        }
        public void SetWaveLength(int length)
        {
            labLength.Text = length.ToString();
        }
        public AutoWaveLine()
        {
            InitializeComponent();
            for (int i = 0; i < 200; i++)
            {
                Points.Enqueue(0);
            }
            YAxis[1] = YAxisLab1;
            YAxis[2] = YAxisLab2;
            YAxis[3] = YAxisLab3;
            YAxis[4] = YAxisLab4;
            YAxis[5] = YAxisLab5;
            YAxis[6] = YAxisLab6;//_unit
            XAxis[1] = XAxisLab1;
            XAxis[2] = XAxisLab2;
            XAxis[3] = XAxisLab3;
            XAxis[4] = XAxisLab4;
            XAxis[5] = XAxisLab5;
            XAxis[6] = XAxisLab6;
            XAxis[7] = XAxisLab7;
            XAxisTimeCount[1] = 0.0;
            XAxisTimeCount[2] = 0.0;
            XAxisTimeCount[3] = 0.0;
            XAxisTimeCount[4] = 0.0;
            XAxisTimeCount[5] = 0.0;
            XAxisTimeCount[6] = 0.0;
            XAxisTimeCount[7] = 0.0;
        }

        public void Add(double val)
        {
            double tmpVal = Points.Dequeue();
            Points.Enqueue(val);
            //计算参数
            MaxVal = Points.Peek();
            MinVal = MaxVal;
            foreach (double tmp in Points)
            {
                if (MaxVal < tmp)
                {
                    MaxVal = tmp;
                }
                if (MinVal > tmp)
                {
                    MinVal = tmp;
                }
            }
            //执行功能
            ChangeAxis();
            TimeCalculate();
            PaintLine();
            OntimeValue(val);
        }
        public void Reset()
        {
            Points.Clear();
            for (int i = 0; i < 200; i++)
            {
                Points.Enqueue(0);
            }
            MaxVal = 0.0;
            MinVal = 0.0;
            AvgVal = 0.0;

            foreach (int key in XAxis.Keys)
            {
                XAxis[key].Content = "0";
                XAxisTimeCount[key] = 0.0;
            }
        }
        protected void ChangeAxis()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                double tmp = (MaxVal - MinVal) / 5;             //Y轴一格高度
                foreach (int key in YAxis.Keys)
                {
                    double num = MinVal + (key - 1) * tmp;      //Y轴刻度
                    YAxis[key].Content = num.ToString("f3");
                }
            }));
        }
        protected void TimeCalculate()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                XAxisTimeCount[7] = XAxisTimeCount[7] + 0.3;         //时间刻度
                if (XAxisTimeCount[7] > 10)
                {
                    XAxisTimeCount[6] = XAxisTimeCount[6] + 0.3;
                }
                if (XAxisTimeCount[6] > 10)
                {
                    XAxisTimeCount[5] = XAxisTimeCount[5] + 0.3;
                }
                if (XAxisTimeCount[5] > 10)
                {
                    XAxisTimeCount[4] = XAxisTimeCount[4] + 0.3;
                }
                if (XAxisTimeCount[4] > 10)
                {
                    XAxisTimeCount[3] = XAxisTimeCount[3] + 0.3;
                }
                if (XAxisTimeCount[3] > 10)
                {
                    XAxisTimeCount[2] = XAxisTimeCount[2] + 0.3;
                }
                if (XAxisTimeCount[2] > 10)
                {
                    XAxisTimeCount[1] = XAxisTimeCount[1] + 0.3;
                }
                foreach (int key in XAxis.Keys)
                {
                    XAxis[key].Content = XAxisTimeCount[key].ToString("f3");
                }
            }));
        }
        protected void PaintLine()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                theWaveline.Points.Clear();
                double count = MaxVal - MinVal;
                double pos = 0.0;
                int i = 0;
                if (count != 0)
                {
                    theWaveline.Points.Add(new Point(101, 600));
                    foreach (double unm in Points)
                    {
                        pos = 596 - 492 / count * (unm - MinVal);
                        theWaveline.Points.Add(new Point(i * 3 + 100, pos));
                        i++;
                    }
                    theWaveline.Points.Add(new Point(699, 600));
                }
            }));
        }
        protected void OntimeValue(double currentval)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                CurrentValLab.Content = currentval.ToString("f3");
            }));
        }
    }
}