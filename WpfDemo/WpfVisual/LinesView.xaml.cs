using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfVisual
{
    /// <summary>
    /// LinesView.xaml 的交互逻辑
    /// </summary>
    public enum LineStatus
    {
        Wait,
        Flash,
        Finish
    }
    public enum LineAction
    {
        Selected,
        Unselected,
    }
    public enum LineLength
    {
        LongLine,
        ShortLine,
    }
    public class LineItem
    {
        private Point _start;
        private Point _end;
        private DrawingVisual _visual;

        public DrawingVisual _Visual
        {
            get
            {
                return _visual;
            }
        }
        public Point StartPoint
        {
            get
            {
                return _start;
            }
        }
        public Point EndPoint
        {
            get
            {
                return _end;
            }
        }
        public Brush brush;

        public LineLength LineSide;
        public LineAction LineAction;
        public LineStatus Status;
        public LineItem(DrawingVisual vis, LineStatus stat, LineAction act, LineLength sid, Brush brh, Point start, Point end)
        {
            _visual = vis;
            Status = stat;
            LineAction = act;
            LineSide = sid;
            brush = brh;
            _start = start;
            _end = end;
        }
    }
    public partial class LinesView : UserControl
    {
        //实物属性
        private double longsied = 0.0;
        private double shortsied = 0.0;
        private double rate = 0;
        //控件属性
        private double Length = 0.0;         //控件内切正方形的长度
        private double offset_w = 0.0;       //偏移值
        private double offset_h = 0.0;
        private double canvas_Width = 0.0;   //减去偏移值后映射实物在控件中的宽度
        private double canvas_Height = 0.0;
        private double center_w;             //旋转中心
        private double center_h;

        private static int wheelCount = 0;     //滚轮放大倍数
        private static int wheelMaxCount = 20; //滚轮放大最大倍数
        private object M_Lock = new object();

        public List<LineItem> ShortCuttingLines = new List<LineItem>();
        public List<LineItem> LongCuttingLines = new List<LineItem>();

        public LinesView()
        {
            InitializeComponent();
            this.Loaded += LinesView_Loaded;
            this.Unloaded += LinesView_Unloaded;
            LinesPanel.MouseWheel += OledPanel_MouseWheel;
        }
        private void LinesView_Loaded(object sender, RoutedEventArgs e)
        {
            if (true == IsVisible)
            {
                if (Length == 0.0)
                {
                    double background_x = this.ActualWidth;
                    double background_y = this.ActualHeight;
                    //缩略显示切割控件
                    LinesPanel.Width = background_x / 2;
                    LinesPanel.Height = background_y / 2;
                    Length = background_x > background_y ? background_y : background_x;
                }
            }
        }
        private void LinesView_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        public void ResetViewerRate(double longline, double shortline)//重置oled大小
        {
            ClearLines();
            longsied = longline;
            shortsied = shortline;
            rate = shortline / longline;

            double width = Length;
            double height = width * rate;
            LinesPanel.Width = width;
            LinesPanel.Height = height;

            center_w = width / 2;
            center_h = height / 2;
            offset_w = 5;
            offset_h = offset_w * rate;
            canvas_Height = height - offset_h * 2;
            canvas_Width = width - offset_w * 2;
        }
        public bool AddShortLine(double offset, Brush brsh)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            Point start_point = new Point();
            Point end_point = new Point();
            //参数设置和验证
            double width = offset / longsied * canvas_Width;
            if (width < 0 || width > (offset_w + canvas_Width))
            {
                return false;
            }
            start_point.X = offset_w + width;
            start_point.Y = offset_h;
            end_point.X = offset_w + width;
            end_point.Y = offset_h + canvas_Height;
            Pen pen = new Pen(brsh, 1);
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                dc.DrawLine(pen, start_point, end_point);
            }
            //添加
            if (LinesPanel.AddShortVisual(drawingVisual))
            {
                LineItem item = new LineItem(drawingVisual, LineStatus.Wait, LineAction.Unselected, LineLength.ShortLine, brsh, start_point, end_point);
                if (ShortCuttingLines.Count == 0)
                {
                    ShortCuttingLines.Add(item);
                }
                else
                {
                    int index = 0;
                    bool flag = false;
                    for (; index < ShortCuttingLines.Count;)
                    {
                        if (ShortCuttingLines[index].StartPoint.X < start_point.X)
                        {
                            index++;
                        }
                        else
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag || (index == ShortCuttingLines.Count))
                    {
                        ShortCuttingLines.Insert(index, item);
                    }
                }
                return true;
            }
            return false;
        }
        public bool AddLongLine(double offset, Brush brsh)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            Point start_point = new Point();
            Point end_point = new Point();
            //参数设置和验证
            double height = offset / shortsied * canvas_Height;
            if (height < 0 || height > (offset_h + canvas_Height))
            {
                return false;
            }
            start_point.X = offset_w;
            start_point.Y = offset_h + height;
            end_point.X = offset_w + canvas_Width;
            end_point.Y = offset_h + height;
            Pen pen = new Pen(brsh, 1);
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                dc.DrawLine(pen, start_point, end_point);
            }
            //添加
            if (LinesPanel.AddLongVisual(drawingVisual))
            {
                LineItem item = new LineItem(drawingVisual, LineStatus.Wait, LineAction.Unselected, LineLength.LongLine, brsh, start_point, end_point);
                if (LongCuttingLines.Count == 0)
                {
                    LongCuttingLines.Add(item);
                }
                else
                {
                    int index = 0;
                    bool flag = false;
                    for (; index < LongCuttingLines.Count;)
                    {
                        if (LongCuttingLines[index].StartPoint.X < start_point.X)
                        {
                            index++;
                        }
                        else
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag || (index == LongCuttingLines.Count))
                    {
                        LongCuttingLines.Insert(index, item);
                    }
                }
                return true;
            }
            return false;
        }

        public void DeleteLine(LineLength length, int index)
        {
            if (length == LineLength.ShortLine)
            {
                if (index < 0 || ShortCuttingLines.Count <= 0 || ShortCuttingLines.Count <= index)
                {
                    return;
                }
                ShortCuttingLines.RemoveAt(index);
                LinesPanel.DeleteVisual(index);
            }
            else
            {
                if (index < 0 || LongCuttingLines.Count <= 0 || LongCuttingLines.Count <= index)
                {
                    return;
                }
                LongCuttingLines.RemoveAt(index);
                LinesPanel.DeleteVisual(ShortCuttingLines.Count + index);
            }
        }
        public void SelectLine(LineLength length, int index)
        {
            if (length == LineLength.ShortLine)
            {
                if (index < 0 || ShortCuttingLines.Count <= 0 || ShortCuttingLines.Count <= index)
                {
                    return;
                }
                LineItem line = ShortCuttingLines[index];
                if (line.LineAction == LineAction.Selected)
                {
                    return;
                }
                Pen pen = new Pen(Brushes.Blue, 3);
                line.LineAction = LineAction.Selected;
                SetLineColor(line._Visual, line, pen);
            }
            else
            {
                if (index < 0 || LongCuttingLines.Count <= 0 || LongCuttingLines.Count <= index)
                {
                    return;
                }
                LineItem line = LongCuttingLines[index];
                if (line.LineAction == LineAction.Selected)
                {
                    return;
                }
                Pen pen = new Pen(Brushes.Blue, 3);
                line.LineAction = LineAction.Selected;
                SetLineColor(line._Visual, line, pen);
            }
        }
        public void UnselectLine(LineLength length, int index)
        {
            if (length == LineLength.ShortLine)
            {
                if (index < 0 || ShortCuttingLines.Count <= 0 || ShortCuttingLines.Count <= index)
                {
                    return;
                }
                LineItem line = ShortCuttingLines[index];
                if (line.LineAction == LineAction.Unselected)
                {
                    return;
                }
                Pen pen = new Pen(line.brush, 1);
                line.LineAction = LineAction.Unselected;
                SetLineColor(line._Visual, line, pen);
            }
            else
            {
                if (index < 0 || LongCuttingLines.Count <= 0 || LongCuttingLines.Count <= index)
                {
                    return;
                }
                LineItem line = LongCuttingLines[index];
                if (line.LineAction == LineAction.Unselected)
                {
                    return;
                }
                Pen pen = new Pen(line.brush, 1);
                line.LineAction = LineAction.Unselected;
                SetLineColor(line._Visual, line, pen);
            }
        }


        private void SetLineColor(Visual visual, LineItem line, Pen pen)
        {
            DrawingVisual drawingVisual = visual as DrawingVisual;
            Point strPoint = line.StartPoint;
            Point endPoint = line.EndPoint;
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                dc.DrawLine(pen, strPoint, endPoint);
            }
        }
        public void ClearLines()
        {
            wheelCount = 0;
            ShortCuttingLines.Clear();
            LongCuttingLines.Clear();
            LinesPanel.ClearVisuals(); //清理面板
        }
        public void RotateViewer(double angle = 0.0)//旋转
        {
            RotateTransform rt = new RotateTransform(angle, center_w, center_h);
            LinesPanel.RenderTransform = rt;
        }
        private void OledPanel_MouseWheel(object sender, MouseWheelEventArgs e)//缩放
        {
            Point center = e.GetPosition(LinesPanel);
            ScaleTransform st;
            if (e.Delta > 0)
            {
                wheelCount++;
                if (wheelCount > wheelMaxCount)
                {
                    wheelCount = wheelMaxCount;
                }
                st = new ScaleTransform(1 + wheelCount * 0.1, 1 + wheelCount * 0.1, center.X, center.Y);
                LinesPanel.RenderTransform = st;
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
                    st = new ScaleTransform(1 + wheelCount * 0.1, 1 + wheelCount * 0.1, center.X, center.Y);
                }
                LinesPanel.RenderTransform = st;
            }
        }
    }
}
