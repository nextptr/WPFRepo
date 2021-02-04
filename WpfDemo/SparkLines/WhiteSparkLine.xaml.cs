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

namespace SparkLines
{
    /// <summary>
    /// WhiteSparkLine.xaml 的交互逻辑
    /// </summary>
    public partial class WhiteSparkLine : UserControl
    {
        public Queue<double> Points = new Queue<double>();
        protected double MaxVal = 0.0;
        protected double MinVal = 0.0;
        protected double AvgVal = 0.0;
        protected double tmpVal = 0.0;

        protected Dictionary<int, Label> YAxis = new Dictionary<int, Label>();
        protected Dictionary<int, Label> YAxis_unit = new Dictionary<int, Label>();
        protected Dictionary<int, Label> XAxis = new Dictionary<int, Label>();
        protected Dictionary<int, double> XAxisTimeCount = new Dictionary<int, double>();
        protected static int timeCount = 0;

        public WhiteSparkLine()
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
            YAxis_unit[1] = YAxisLab1_unit;
            YAxis_unit[2] = YAxisLab2_unit;
            YAxis_unit[3] = YAxisLab3_unit;
            YAxis_unit[4] = YAxisLab4_unit;
            YAxis_unit[5] = YAxisLab5_unit;
            YAxis_unit[6] = YAxisLab6_unit;
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
            tmpVal = Points.Dequeue();
            Points.Enqueue(val);
            //计算参数
            MaxVal = Points.Peek();
            MinVal = MaxVal;
            double total = 0.0;
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
                total += tmp;
            }
            AvgVal = total / 200;
            //执行功能
            ChangeAxis();
            TimeCalculate();
            PaintLine();
            ShowValue(val);
        }
        //动态修改y轴数据
        protected void ChangeAxis()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                double tmp = (MaxVal - MinVal) / 5;
                foreach (int key in YAxis.Keys)
                {
                    double num = MinVal + (key - 1) * tmp;
                    if ((num < -1000) || (num > 1000))
                    {
                        YAxis_unit[key].Content = "kW";
                        YAxis[key].Content = (num / 1000).ToString("f3");
                    }
                    else if ((num > -1) || (num < 1))
                    {
                        YAxis_unit[key].Content = "mW";
                        YAxis[key].Content = (num * 1000).ToString("f3");
                    }
                    else
                    {
                        YAxis_unit[key].Content = "W";
                        YAxis[key].Content = num.ToString("f3");
                    }
                }
            }));
        }
        //动态修改x轴时间数据
        protected void TimeCalculate()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (timeCount < 60000)
                {
                    timeCount += 300;
                }
                XAxisTimeCount[7] = XAxisTimeCount[7] + 0.3;
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
        //画波动线
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
        //显示即时数据
        protected void ShowValue(double currentval)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                GetUnit(currentval, CurrentValLab, CurrentValLab_unit);
                GetUnit(MaxVal, MaxValLab, MaxValLab_unit);
                GetUnit(MinVal, MinValLab, MinValLab_unit);
                GetUnit(AvgVal, AvgValLab, AvgValLab_unit);
            }));
        }
        //计算数据单位
        protected void GetUnit(double num, Label labval, Label labunit)
        {
            if ((num < -1000) || (num > 1000))
            {
                labunit.Content = "kW";
                labval.Content = (num / 1000).ToString("f3");
            }
            else if ((num > -1) || (num < 1))
            {
                labunit.Content = "mW";
                labval.Content = (num * 1000).ToString("f3");
            }
            else
            {
                labunit.Content = "W";
                labval.Content = num.ToString("f3");
            }
        }
    }
}
