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

namespace DXFComponent
{
    /// <summary>
    /// DxfCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class DxfCanvas : UserControl
    {
        private double act_wid = 0.0;
        private double act_hei = 0.0;
        private static int wheelCount = 0;     //滚轮放大倍数
        private static int wheelMaxCount = 20; //滚轮放大最大倍数
        private double center_w;               //旋转中心
        private double center_h;
        public DxfCanvas()
        {
            InitializeComponent();
            this.Loaded += DxfCanvas_Loaded;
        }

        private void DxfCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible)
            {
                this.Loaded -= DxfCanvas_Loaded;
                cav_board.MouseWheel += Cav_board_MouseWheel;
                act_wid = cav_board.ActualWidth;
                act_hei = cav_board.ActualHeight;
                double len = act_hei < act_wid ? act_hei : act_wid;
                center_w = len / 2;
                center_h = len / 2;
            }
        }
        private void Cav_board_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point center = e.GetPosition(cav_board);
            ScaleTransform st;
            if (e.Delta > 0)
            {
                wheelCount++;
                if (wheelCount > wheelMaxCount)
                {
                    wheelCount = wheelMaxCount;
                }
                st = new ScaleTransform(1 + wheelCount * 1, 1 + wheelCount * 1, center.X, center.Y);
                cav_board.RenderTransform = st;
            }
            else
            {
                wheelCount--;
                if (wheelCount < 0)
                {
                    wheelCount = 0;
                    st = new ScaleTransform(1, 1, center_w, center_h);
                }
                else
                {
                    st = new ScaleTransform(1 + wheelCount * 1, 1 + wheelCount * 1, center.X, center.Y);
                }
                cav_board.RenderTransform = st;
            }
        }

        public void PaintingPath(List<PATH> inls)
        {
            cav_board.Children.Clear();
            double max_x = 0.0;
            double max_y = 0.0;
            List<PATH> roatLs = new List<PATH>();
            List<PATH> mirrLs = new List<PATH>();
            List<PATH> pintLs = new List<PATH>();
            DxfReader.Instance.PathRotateAng(inls, roatLs, 180);//旋转180
            DxfReader.Instance.PathMirrorY(roatLs, mirrLs);     //Y轴镜像路径
            DxfReader.Instance.TransformToOneQuadrant(mirrLs, pintLs, ref max_x, ref max_y); //第一象限路径

            List<PATH> paintPth = new List<PATH>();
            foreach (PATH th in pintLs)
            {
                paintPth.Add(th.Clon());
            }

            if (act_hei < act_wid)
            {
                ScalaPath(ref paintPth, max_x, max_y, act_hei - 10);
            }
            else
            {
                ScalaPath(ref paintPth, max_x, max_y, act_wid - 10);
            }

            OffsetPath(ref paintPth, 5, 5);
            foreach (var tmp in paintPth)
            {
                PATH pth;
                pth = tmp as PATH;
                if (pth.ePathType == EPathType.Line)
                {
                    Polyline lin = new Polyline();
                    lin.Stroke = Brushes.White;
                    lin.StrokeThickness = 1;
                    lin.Points.Add(new Point(pth.StartX, pth.StartY));
                    lin.Points.Add(new Point(pth.EndX, pth.EndY));
                    cav_board.Children.Add(lin);
                }
                if (pth.ePathType == EPathType.Lwpolyline)
                {
                    Polyline lin = new Polyline();
                    lin.Stroke = Brushes.White;
                    lin.StrokeThickness = 1;
                    foreach (Point pos in pth.throughPoints)
                    {
                        lin.Points.Add(pos);
                    }
                    cav_board.Children.Add(lin);
                }
                if (pth.ePathType == EPathType.Arc)
                {
                    Path path = new Path();
                    PathGeometry pathGeometry = new PathGeometry();
                    ArcSegment arc = null;
                    if (pth.StartAngle > pth.EndAngle)
                    {
                        if (360 - pth.StartAngle + pth.EndAngle < 180)
                        {
                            arc = new ArcSegment(new Point(pth.EndX, pth.EndY), new Size(pth.Radio, pth.Radio), 0, false, SweepDirection.Clockwise, true);
                        }
                        else
                        {
                            arc = new ArcSegment(new Point(pth.EndX, pth.EndY), new Size(pth.Radio, pth.Radio), 0, true, SweepDirection.Clockwise, true);
                        }
                    }
                    else
                    {
                        if (pth.EndAngle - pth.StartAngle < 180)
                        {
                            arc = new ArcSegment(new Point(pth.EndX, pth.EndY), new Size(pth.Radio, pth.Radio), 0, false, SweepDirection.Clockwise, true);
                        }
                        else
                        {
                            arc = new ArcSegment(new Point(pth.EndX, pth.EndY), new Size(pth.Radio, pth.Radio), 0, true, SweepDirection.Clockwise, true);
                        }
                    }
                    PathFigure figure = new PathFigure();
                    figure.StartPoint = new Point(pth.StartX, pth.StartY);
                    figure.Segments.Add(arc);
                    pathGeometry.Figures.Add(figure);
                    path.Data = pathGeometry;
                    path.Stroke = Brushes.White;
                    path.StrokeThickness = 1;
                    cav_board.Children.Add(path);
                }
                if (pth.ePathType == EPathType.Circle)
                {
                    Ellipse eps = new Ellipse();
                    eps.Stroke = Brushes.White;
                    eps.StrokeThickness = 1;
                    eps.Width = pth.Radio;
                    eps.Height = pth.Radio;
                    Thickness thick = new Thickness(pth.CenterX - pth.Radio, pth.CenterY - pth.Radio, 0, 0);
                    eps.Margin = thick;
                    cav_board.Children.Add(eps);
                }
            }
        }

        private void ScalaPath(ref List<PATH> ls, double max_x, double max_y, double sca_len)
        {
            double max_len = max_x;
            if (max_x < max_y)
            {
                max_len = max_y;
            }
            foreach (PATH pth in ls)
            {
                if (pth.ePathType == EPathType.Line)
                {
                    pth.StartX = (pth.StartX / max_len) * sca_len;
                    pth.StartY = (pth.StartY / max_len) * sca_len;
                    pth.EndX = (pth.EndX / max_len) * sca_len;
                    pth.EndY = (pth.EndY / max_len) * sca_len;
                }
                if (pth.ePathType == EPathType.Lwpolyline)
                {
                    for (int i = 0; i < pth.throughPoints.Count; i++)
                    {
                        Point pos = new Point();
                        pos.X = (pth.throughPoints[i].X / max_len) * sca_len;
                        pos.Y = (pth.throughPoints[i].Y / max_len) * sca_len;
                        pth.throughPoints[i] = pos;
                    }
                }
                if (pth.ePathType == EPathType.Arc)
                {
                    pth.CenterX = (pth.CenterX / max_len) * sca_len;
                    pth.CenterY = (pth.CenterY / max_len) * sca_len;
                    pth.StartX = (pth.StartX / max_len) * sca_len;
                    pth.StartY = (pth.StartY / max_len) * sca_len;
                    pth.EndX = (pth.EndX / max_len) * sca_len;
                    pth.EndY = (pth.EndY / max_len) * sca_len;
                    pth.Radio = (pth.Radio / max_len) * sca_len;
                }
                if (pth.ePathType == EPathType.Circle)
                {
                    pth.CenterX = (pth.CenterX / max_len) * sca_len;
                    pth.CenterY = (pth.CenterY / max_len) * sca_len;
                    pth.Radio = (pth.Radio / max_len) * sca_len;
                }
            }
        }
        private void OffsetPath(ref List<PATH> ls, double offset_x, double offset_y)
        {
            foreach (var pth in ls)
            {
                if (pth.ePathType == EPathType.Line)
                {
                    pth.StartX = pth.StartX + offset_x;
                    pth.StartY = pth.StartY + offset_y;
                    pth.EndX = pth.EndX + offset_x;
                    pth.EndY = pth.EndY + offset_y;
                }
                if (pth.ePathType == EPathType.Lwpolyline)
                {
                    for (int i = 0; i < pth.throughPoints.Count; i++)
                    {
                        Point pos = new Point();
                        pos.X = pth.throughPoints[i].X + offset_x;
                        pos.Y = pth.throughPoints[i].Y + offset_y;
                        pth.throughPoints[i] = pos;
                    }
                }
                if (pth.ePathType == EPathType.Arc)
                {
                    pth.CenterX = pth.CenterX + offset_x;
                    pth.CenterY = pth.CenterY + offset_y;
                    pth.StartX = pth.StartX + offset_x;
                    pth.StartY = pth.StartY + offset_y;
                    pth.EndX = pth.EndX + offset_x;
                    pth.EndY = pth.EndY + offset_y;
                }
                if (pth.ePathType == EPathType.Circle)
                {
                    pth.CenterX = pth.CenterX + offset_x;
                    pth.CenterY = pth.CenterY + offset_y;
                }
            }
        }
    }
}
