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

namespace Chart
{
    /// <summary>
    /// ChartPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ChartPanel : UserControl
    {
        double margin = 30;
        double act_width = 0.0;
        double act_height = 0.0;

        Point topLeft = new Point();
        Point topRight = new Point();
        Point bottomLeft = new Point();
        Point bottomRight = new Point();
        Polyline curveLine = new Polyline();
        List<double> _data = new List<double>();

        public ChartPanel()
        {
            InitializeComponent();
            this.Loaded += ChartPanel_Loaded;
            curveLine.Stroke = Brushes.Red;
            chartCanvas.Children.Add(curveLine);
        }

        private void ChartPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible)
            {
                ReSizePanel();
            }
        }
        private void drawingCurveLine()
        {
            if (_data.Count == 0)
            {
                return;
            }

            double maxVal = 0.0;
            double minVal = 0.0;
            if (_data.Count == 1)
            {
                maxVal = _data[0];
            }
            else
            {
                maxVal = _data[0];
                minVal = _data[0];
            }

            foreach (double val in _data)
            {
                if (val > maxVal)
                {
                    maxVal = val;
                }
                if (val < minVal)
                {
                    minVal = val;
                }
            }

            double y_gap = maxVal - minVal;
            double x_step = Math.Abs(topLeft.X - topRight.X) / _data.Count;
            curveLine.Points.Clear();

            curveLine.Points.Add(new Point(bottomLeft.X, bottomLeft.Y));
            for (int i = 0; i < _data.Count; i++)
            {
                curveLine.Points.Add(new Point((i + 1) * x_step + bottomLeft.X, bottomLeft.Y - ((double)_data[i] - minVal) / y_gap * (Math.Abs(topLeft.Y - bottomLeft.Y))));
            }
        }

        public void ReSizePanel()
        {
            act_width = this.ActualWidth;
            act_height = this.ActualHeight;

            topLeft = new Point(margin, margin);
            topRight = new Point(act_width - margin, margin);
            bottomLeft = new Point(margin, act_height - margin);
            bottomRight = new Point(act_width - margin, act_height - margin);

            //X轴，Y轴
            x_axis.X1 = bottomLeft.X;
            x_axis.Y1 = bottomLeft.Y;
            x_axis.X2 = bottomRight.X;
            x_axis.Y2 = bottomRight.Y;

            y_axis.X1 = bottomLeft.X;
            y_axis.Y1 = bottomLeft.Y;
            y_axis.X2 = topLeft.X;
            y_axis.Y2 = topLeft.Y;

            //箭头
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(bottomRight.X, bottomRight.Y - 4);
            LineSegment seg1 = new LineSegment(new Point(bottomRight.X, bottomRight.Y + 4), true);
            LineSegment seg2 = new LineSegment(new Point(bottomRight.X + 10, bottomRight.Y), true);
            pathFigure.Segments.Add(seg1);
            pathFigure.Segments.Add(seg2);
            pathGeometry.Figures.Add(pathFigure);
            x_axisArrow.Data = pathGeometry;

            pathGeometry = new PathGeometry();
            pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(topLeft.X - 4, topLeft.Y);
            seg1 = new LineSegment(new Point(topLeft.X + 4, topLeft.Y), true);
            seg2 = new LineSegment(new Point(topLeft.X, topLeft.Y - 10), true);
            pathFigure.Segments.Add(seg1);
            pathFigure.Segments.Add(seg2);
            pathGeometry.Figures.Add(pathFigure);
            y_axisArrow.Data = pathGeometry;

            //标签
            Ellipse dataEllipse = new Ellipse();
            dataEllipse.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0xff));
            dataEllipse.Width = 8;
            dataEllipse.Height = 8;

            Canvas.SetLeft(x_label, bottomRight.X + 5);
            Canvas.SetTop(x_label, bottomRight.Y + 5);

            Canvas.SetLeft(y_label, topLeft.X - 20);
            Canvas.SetTop(y_label, topLeft.Y - 5);

            Canvas.SetLeft(o_label, bottomLeft.X - 20);
            Canvas.SetTop(o_label, bottomLeft.Y + 5);
            drawingCurveLine();
        }
        public void AddData(object val)
        {
            double d;
            double.TryParse(val.ToString(), out d);

            _data.Add(d);
            drawingCurveLine();
        }
        public void CleanData()
        {
            _data.Clear();
            curveLine.Points.Clear();
        }
    }
}
