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
    /// FullDataSparkLine.xaml 的交互逻辑
    /// </summary>
    public partial class FullDataSparkLine : UserControl
    {
        public List<double> Points = new List<double>();
        protected double MaxVal = 0.0;
        protected double MinVal = 0.0;
        protected double AvgVal = 0.0;
        protected double tmpVal = 0.0;

        protected Dictionary<int, Label> YAxis = new Dictionary<int, Label>();
        protected static int timeCount = 0;

        public FullDataSparkLine()
        {
            InitializeComponent();
            YAxis[1] = YAxisLab1;
            YAxis[2] = YAxisLab2;
            YAxis[3] = YAxisLab3;
            YAxis[4] = YAxisLab4;
            YAxis[5] = YAxisLab5;
            YAxis[6] = YAxisLab6;
        }
        public void Add(double val)
        {
            if (Points.Count == 0)
            {
                MaxVal = val;
                MinVal = val;
                AvgVal = val;
                tmpVal = val;
            }
            Points.Add(val);
            //计算参数
            double total = 0.0;
            if (MaxVal < val)
            {
                MaxVal = val;
            }
            if (MinVal > val)
            {
                MinVal = val;
            }
            total += val;
            AvgVal = total / Points.Count;
            //执行功能
            ChangeAxis();
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
                    YAxis[key].Content = num.ToString("f2");
                }
            }));
        }
        //画波动线
        protected void PaintLine()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                theWaveline.Points.Clear();
                double hei = MaxVal - MinVal;
                double xgap = 600 / Points.Count;
                double xpos = 0.0;
                double ypos = 0.0;
                int i = 0;
                if (hei != 0)
                {
                    theWaveline.Points.Add(new Point(101, 600));
                    foreach (double unm in Points)
                    {
                        ypos = 596 - 492 / hei * (unm - MinVal);
                        xpos = 101 + i * xgap;
                        theWaveline.Points.Add(new Point(xpos, ypos));
                        i++;
                    }
                }
            }));
        }
        //显示即时数据
        protected void ShowValue(double currentval)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                CurrentValLab.Content = currentval.ToString("f2");
                MaxValLab.Content = MaxVal.ToString("f2");
                MinValLab.Content = MinVal.ToString("f2");
                AvgValLab.Content = AvgVal.ToString("f2");
            }));
        }
    }

}
