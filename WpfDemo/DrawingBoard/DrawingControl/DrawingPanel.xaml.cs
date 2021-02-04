using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using DrawingBoard.Primitive;
using DrawingBoard.Tool;

namespace DrawingBoard.DrawingControl
{
    /// <summary>
    /// DrawingPanel.xaml 的交互逻辑
    /// </summary>
    public partial class DrawingPanel : UserControl
    {
        public delegate void PrimitiveStackChangedHandler(CommandActionType cmdType, List<PrimitiveBase> prm);
        public event PrimitiveStackChangedHandler PrimitiveStackChanged;

        public event EventHandler SelectionChanged;
        public event EventHandler PrimitiveMoved;
        public event EventHandler PrimitiveAdded;
        public event EventHandler PrimitiveDeleted;

        private ToolType _tool = ToolType.Pointer;
        private PolyLine1 _polyLine;
        private Point _last;
        private Point _scalePosition;
        private bool _pan = false;
        private const double SCALE_FACTOR = 0.9;

        private List<PrimitiveBase> _selections = new List<PrimitiveBase>();
        private List<PrimitiveBase> _children = new List<PrimitiveBase>();
        private List<PrimitiveBase> _copies = new List<PrimitiveBase>();
        private List<PrimitiveBase> _cutsList = new List<PrimitiveBase>();
        private List<PrimitiveBase> _canvasList = new List<PrimitiveBase>();
        public Stack<PrimitiveBase> _stk = new Stack<PrimitiveBase>();

        public CartesianCanvas Canvas { get { return canvas; } }
        public ScaleTransform Scale { get { return scale; } }
        public TranslateTransform Translate { get { return translate; } }

        public bool HasSelectedItem { get { return Selections.Count > 0; } }
        public List<PrimitiveBase> Selections { get { return GetSelections(); } }
        public List<PrimitiveBase> Children { get { return GetChildren(); } }
        public CommandActionType ActionType { get; set; }
        public Point ScalePosition { get { return _scalePosition; } }


        //用于移动
        Point targetPoint;        //鼠标落下时鼠标在CanvasRoot里的位置
        Point pMoved;             //鼠标移动后鼠标在CanvasRoot里的新位置
        Point ViewBoxMainLocation;//鼠标落下时外层ViewBoxMain在CanvasRoot里的位置
        //用于缩放
        Point mousePoint;         //缩放前鼠标在ViewBoxMain的位置
        Point locationPoint;      //缩放前ViewBoxMain在CanvasRoot里的位置
        Point NewPoint;           //缩放后计算ViewBoxMain的新坐标
        double biliUp = 1.1;      //增大比例
        double biliDown = 0.9;    //缩小比例

        public DrawingPanel()
        {
            InitializeComponent();
            this.Loaded += DrawingBoard_Loaded;

            InitMenu();
            InitCanvas();

            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
            grid.MouseLeftButtonUp += Grid_MouseLeftButtonUp;
            grid.MouseWheel += Grid_MouseWheel;
            grid.MouseMove += Grid_MouseMove;

            btnPointer.Checked += BtnPointer_Checked;
            btnDot.Checked += BtnDot_Checked;
            btnLine.Checked += BtnLine_Checked;
            btnRectangle.Checked += BtnRectangle_Checked;
            btnCircle.Checked += BtnCircle_Checked;
            btnPolyLine.Checked += BtnPolyLine_Checked;

            btnPan.Checked += BtnPan_Checked;
            btnShowField.Click += BtnShowField_Click;
            btnShowAxis.Click += BtnShowAxis_Click;
            btnGenerateGrid.Click += BtnGenerateGrid_Click;
            btnGenerateCross.Click += BtnGenerateCross_Click;
        }

        public void InitMenu()
        {
            ContextMenu cm = new ContextMenu();
            MenuItem mmi = new MenuItem();
            mmi.Header = "复制";
            mmi.Click += MmiCopy_Click;
            cm.Items.Add(mmi);

            mmi = new MenuItem();
            mmi.Header = "剪切";
            mmi.Click += MmiCut_Click;
            cm.Items.Add(mmi);

            mmi = new MenuItem();
            mmi.Header = "粘贴";
            mmi.Click += MmiPaste_Click;
            cm.Items.Add(mmi);

            cm.Opened += Cm_Opened;
            grid.ContextMenu = cm;
        }
        public void InitCanvas()
        {
            CanvasMain.MouseDown += CanvasMain_MouseDown;
            CanvasMain.MouseUp += CanvasMain_MouseUp;
            CanvasMain.MouseMove += CanvasMain_MouseMove;
            CanvasMain.MouseWheel += CanvasMain_MouseWheel;

            canvas.Focusable = true;
            canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseRightButtonDown += Canvas_MouseRightButtonDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.PreviewMouseLeftButtonDown += Canvas_PreviewMouseLeftButtonDown;
            canvas.PreviewKeyDown += Canvas_PreviewKeyDown;
        }

        #region 业务
        private void CanvasMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var targetElement = e.Source as IInputElement;

            if (targetElement != null)
            {
                ViewBoxMainLocation.X = System.Windows.Controls.Canvas.GetLeft(ViewBoxMain);
                ViewBoxMainLocation.Y = System.Windows.Controls.Canvas.GetTop(ViewBoxMain);
                targetPoint = e.GetPosition(CanvasRoot);
                //开始捕获鼠标
                targetElement.CaptureMouse();
            }
        }
        private void CanvasMain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //取消捕获鼠标   
            Mouse.Capture(null);
            this.Cursor = System.Windows.Input.Cursors.Arrow;
        }
        private void CanvasMain_MouseMove(object sender, MouseEventArgs e)
        {
            //确定鼠标左键处于按下状态并且有元素被选中

            var targetElement = Mouse.Captured as UIElement;

            if (e.LeftButton == MouseButtonState.Pressed && targetElement != null)
            {
                pMoved = e.GetPosition(CanvasRoot);
                //设置最终位置
                System.Windows.Controls.Canvas.SetLeft(ViewBoxMain, pMoved.X - targetPoint.X + ViewBoxMainLocation.X);
                System.Windows.Controls.Canvas.SetTop(ViewBoxMain, pMoved.Y - targetPoint.Y + ViewBoxMainLocation.Y);

                this.Cursor = System.Windows.Input.Cursors.ScrollAll;
            }
        }
        private void CanvasMain_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mousePoint = e.GetPosition(ViewBoxMain);//获取当前的点
            locationPoint.X = System.Windows.Controls.Canvas.GetLeft(ViewBoxMain);
            locationPoint.Y = System.Windows.Controls.Canvas.GetTop(ViewBoxMain);

            double width = ViewBoxMain.Width;
            double height = ViewBoxMain.Height;

            if (e.Delta > 0)//放大
            {
                ViewBoxMain.Width = width * biliUp;
                ViewBoxMain.Height = height * biliUp;

                NewPoint.X = locationPoint.X - mousePoint.X * (biliUp - 1);
                NewPoint.Y = locationPoint.Y - mousePoint.Y * (biliUp - 1);
            }
            else
            {
                ViewBoxMain.Width = width * biliDown;
                ViewBoxMain.Height = height * biliDown;

                NewPoint.X = locationPoint.X + mousePoint.X * (1 - biliDown);
                NewPoint.Y = locationPoint.Y + mousePoint.Y * (1 - biliDown);
            }

            System.Windows.Controls.Canvas.SetLeft(ViewBoxMain, NewPoint.X);
            System.Windows.Controls.Canvas.SetTop(ViewBoxMain, NewPoint.Y);

            e.Handled = true;//一定要有这行代码，否则会有偏差
        }
        #endregion

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_tool == ToolType.Polyline)
            {
                //PrimitiveContexMenu menu = new PrimitiveContexMenu();
                //MenuItem mi = menu.GetItem(0);
                //mi.Click += MiMoveToOrigin_Click;
                //menu.IsOpen = true;

                //this.ReleaseMouseCapture();
                ToolManager.Instance.Get(_tool).MouseRightDown(canvas, e, canvas.ScreenToWorld, this._polyLine);
            }
            return;
            Polyline _polyline1 = new Polyline();
            //_rectangle.Left = p.X;
            //_rectangle.Top = p.Y;
            //_rectangle.Width = 0;
            //_rectangle.Height = 0;
            int x = 113;//x值递增*10
            int y = 117;

            var point = new Point(x, y);


            //_polyline1.Points.Add(point);//添加新的数据点
            //point = new Point(13, 17);


            //_polyline1.Points.Add(point);//添加新的数据点

            //point = new Point(113, 117);


            _polyline1.Points.Add(point);//添加新的数据点


            canvas.Children.Add(_polyline1);

            return;
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            PathGeometry pathGeometry = new PathGeometry();
            ArcSegment arc = new ArcSegment(new Point(100, 200), new Size(50, 100), 0, false, SweepDirection.Counterclockwise, true);
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(100, 0);
            figure.Segments.Add(arc);
            pathGeometry.Figures.Add(figure);
            path.Data = pathGeometry;
            path.Stroke = Brushes.Orange;

            RotateTransform angle1 = new RotateTransform(); //旋转 
            ScaleTransform scale = new ScaleTransform(); //缩放 
            TransformGroup group = new TransformGroup();
            RotateTransform angle = new RotateTransform(30, 0, 0);
            group.Children.Add(scale);
            group.Children.Add(angle);

            path.RenderTransform = group;


            canvas.Children.Add(path);



            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = new Point(0, 0);
            myLineGeometry.EndPoint = new Point(50, 50);

            System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = myLineGeometry;


            angle1 = new RotateTransform(); //旋转 
            scale = new ScaleTransform(); //缩放 
            group = new TransformGroup();
            angle = new RotateTransform(30, 0, 0);
            group.Children.Add(scale);
            group.Children.Add(angle);

            myPath.RenderTransform = group;
            canvas.Children.Add(myPath);


            LineGeometry myLineGeometry1 = new LineGeometry();
            myLineGeometry1.StartPoint = new Point(0, 0);
            myLineGeometry1.EndPoint = new Point(50, 50);

            System.Windows.Shapes.Path myPath1 = new System.Windows.Shapes.Path();
            myPath1.Stroke = Brushes.Black;
            myPath1.StrokeThickness = 1;
            myPath1.Data = myLineGeometry1;


            angle1 = new RotateTransform(); //旋转 
            scale = new ScaleTransform(); //缩放 
            group = new TransformGroup();
            angle = new RotateTransform(50);
            group.Children.Add(scale);
            group.Children.Add(angle);

            myPath1.RenderTransform = group;
            canvas.Children.Add(myPath1);




        }
        private void MmiPaste_Click(object sender, RoutedEventArgs e)
        {
            List<PrimitiveBase> bsList = new List<PrimitiveBase>();
            foreach (PrimitiveBase pb in _copies)
            {
                PrimitiveBase clone = pb.Clone() as PrimitiveBase;
                clone.Move(5, -5);
                canvas.Children.Add(clone);
                clone.cmdActionType = "add";
                bsList.Add(clone);
                //ToolManager.Instance._undoStk.Push(clone);
                if (PrimitiveAdded != null)
                {
                    PrimitiveAdded(this, EventArgs.Empty);

                }
            }

            if (bsList.Count > 0)
            {
                PrimitiveStackChanged(CommandActionType.Add, bsList);
            }


            btnPointer.IsChecked = true;
        }
        private void MmiCut_Click(object sender, RoutedEventArgs e)
        {
            _copies.Clear();
            foreach (var child in canvas.Children)
            {
                PrimitiveBase pb = child as PrimitiveBase;
                PrimitiveBase pbNew = null;
                if (pb != null && pb.IsSelected)
                {
                    pbNew = pb.Clone() as PrimitiveBase;
                    _copies.Add(pbNew);
                }
            }

            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                PrimitiveBase pb = canvas.Children[i] as PrimitiveBase;
                if (pb != null && pb.IsSelected)
                {
                    canvas.Children.RemoveAt(i);
                    if (PrimitiveDeleted != null)
                    {
                        PrimitiveDeleted(this, EventArgs.Empty);
                    }
                }
            }
        }
        private void MmiCopy_Click(object sender, RoutedEventArgs e)
        {
            _copies.Clear();

            foreach (var child in canvas.Children)
            {
                PrimitiveBase pb = child as PrimitiveBase;
                if (pb != null && pb.IsSelected)
                {
                    _copies.Add(pb);
                }
            }
        }
        private void Cm_Opened(object sender, RoutedEventArgs e)
        {
            bool found = false;
            foreach (var child in canvas.Children)
            {
                PrimitiveBase pb = child as PrimitiveBase;
                if (pb != null && pb.IsSelected)
                {
                    found = true;
                }
            }

            ContextMenu cm = sender as ContextMenu;
            ((MenuItem)(cm.Items[0])).IsEnabled = found;
            ((MenuItem)(cm.Items[1])).IsEnabled = found;
            ((MenuItem)(cm.Items[2])).IsEnabled = _copies.Count > 0 || _cutsList.Count > 0;
            ((MenuItem)(cm.Items[2])).IsEnabled = _polyLine != null;

        }
        private void BtnGenerateCross_Click(object sender, RoutedEventArgs e)
        {
            int size = 20;

            Line l1 = new Line();
            l1.Y1 = 0;
            l1.Y2 = 0;
            l1.X1 = -size;
            l1.X2 = size;
            Add(l1);

            Line l2 = new Line();
            l2.Y1 = -size;
            l2.Y2 = size;
            l2.X1 = 0;
            l2.X2 = 0;
            Add(l2);
        }
        private void BtnGenerateGrid_Click(object sender, RoutedEventArgs e)
        {
            //GenerateGridWindow w = new GenerateGridWindow();
            //if (w.ShowDialog() == true)
            //{
            //    GenerateGridParameterItem p = w.GenerateGridParameterItem;
            //    GenerateGrid(p.X, p.Y, p.Spacing);
            //}
        }
        private void BtnShowAxis_Click(object sender, RoutedEventArgs e)
        {
            UIElement axis = canvas.Children[0];
            axis.Visibility = (axis.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
        }
        private void GenerateGrid(int x, int y, int spacing)
        {
            x = x / 2;
            y = y / 2;
            int ext = 5;

            for (int i = -y; i <= y; i += spacing)
            {
                Line l = new Line();
                l.Y1 = i;
                l.Y2 = i;
                l.X1 = -x - ext;
                l.X2 = x + ext;

                Add(l);
            }

            for (int i = -x; i <= x; i += spacing)
            {
                Line l = new Line();
                l.Y1 = -y - ext;
                l.Y2 = y + ext;
                l.X1 = i;
                l.X2 = i;

                Add(l);
            }

            // indication of x-axis direction
            {
                Line l = new Line();
                l.Y1 = -1;
                l.Y2 = 1;
                l.X1 = x + ext + 3;
                l.X2 = x + ext + 3;
                Add(l);
            }

            // indication of y-axis direction
            for (int i = 0; i < 2; ++i)
            {
                Line l = new Line();
                l.Y1 = y + ext + 3 + i * 1;
                l.Y2 = y + ext + 3 + i * 1;
                l.X1 = -2;
                l.X2 = 2;
                Add(l);
            }
        }
        private void ShowField()
        {
            Axis axis = canvas.Children[0] as Axis;
            Rect bounds = axis.RenderedGeometry.Bounds;
            bounds.Inflate(5, 5);

            double sw = canvas.ActualWidth / bounds.Width;
            double sh = canvas.ActualHeight / bounds.Height;
            double s = Math.Min(sw, sh);
            scale.ScaleX = s;
            scale.ScaleY = s;

            double x = canvas.ActualWidth / 2 * s;
            double y = canvas.ActualHeight / 2 * s;

            double dx = x - canvas.ActualWidth / 2;
            double dy = y - canvas.ActualHeight / 2;

            translate.X = -dx;
            translate.Y = -dy;

            SetScale(scale.ScaleX);
        }
        private void BtnShowField_Click(object sender, RoutedEventArgs e)
        {
            ShowField();
        }
        private void DrawingBoard_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                ShowField();

                this.Loaded -= DrawingBoard_Loaded;
            }
        }
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                scale.ScaleX /= SCALE_FACTOR;
                scale.ScaleY /= SCALE_FACTOR;
            }
            else if (e.Delta < 0)
            {
                scale.ScaleX *= SCALE_FACTOR;
                scale.ScaleY *= SCALE_FACTOR;
            }

            Point p00 = canvas.RenderTransform.Transform(new Point(0, 0));
            double dx = _scalePosition.X - p00.X;
            double dy = _scalePosition.Y - p00.Y;

            TranslateTransform t1 = new TranslateTransform();
            TranslateTransform t2 = new TranslateTransform();

            t1.X = _scalePosition.X;
            t1.Y = _scalePosition.Y;

            if (e.Delta > 0)
            {
                t2.X = -dx / SCALE_FACTOR;
                t2.Y = -dy / SCALE_FACTOR;
            }
            else if (e.Delta < 0)
            {
                t2.X = -dx * SCALE_FACTOR;
                t2.Y = -dy * SCALE_FACTOR;
            }

            Matrix m = t1.Value * t2.Value;
            translate.X = m.OffsetX;
            translate.Y = m.OffsetY;

            SetScale(scale.ScaleX);
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _last = e.GetPosition(grid);
        }
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            _scalePosition = e.GetPosition(grid);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_pan)
                {
                    Point point = e.GetPosition(grid);
                    double dx = point.X - _last.X;
                    double dy = point.Y - _last.Y;

                    Pan(dx, dy);

                    _last = point;
                }
                else
                {
                    if (_tool == ToolType.Pointer)
                    {
                        Point point = e.GetPosition(grid);
                        Rect r = new Rect(_last, point);

                        rectSelection.Visibility = Visibility.Visible;
                        rectSelection.Width = r.Width;
                        rectSelection.Height = r.Height;
                        System.Windows.Controls.Canvas.SetLeft(rectSelection, r.Left);
                        System.Windows.Controls.Canvas.SetTop(rectSelection, r.Top);
                    }
                }
            }
        }
        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_tool == ToolType.Pointer)
            {
                rectSelection.Visibility = Visibility.Hidden;

                Point point = e.GetPosition(grid);

                Point p1 = canvas.RenderTransform.Inverse.Transform(_last);
                Point p2 = canvas.RenderTransform.Inverse.Transform(point);

                p1 = canvas.ScreenToWorld.Transform(p1);
                p2 = canvas.ScreenToWorld.Transform(p2);

                Rect r = new Rect(p1, p2);

                canvas.RenderTransform.TransformBounds(r);

                Select(r);

                if (SelectionChanged != null)
                    SelectionChanged(this, e);
            }
        }
        private void BtnPan_Checked(object sender, RoutedEventArgs e)
        {
            _tool = ToolType.Pointer;
            _pan = true;
            CanHit(false);
        }
        private void BtnCircle_Checked(object sender, RoutedEventArgs e)
        {
            _tool = ToolType.Circle;
            grid.Cursor = Cursors.Cross;
            CanHit(false);
            _pan = false;
        }
        private void BtnPolyLine_Checked(object sender, RoutedEventArgs e)
        {
            _tool = ToolType.Polyline;
            _polyLine = new PolyLine1();
            _polyLine.Points = new PointCollection();
            grid.Cursor = Cursors.Cross;
            CanHit(false);
            _pan = false;
        }
        private void BtnRectangle_Checked(object sender, RoutedEventArgs e)
        {
            _tool = ToolType.Rectangle;
            grid.Cursor = Cursors.Cross;
            CanHit(false);
            _pan = false;
        }
        private void BtnDot_Checked(object sender, RoutedEventArgs e)
        {
            _tool = ToolType.Dot;
            grid.Cursor = Cursors.Cross;
            CanHit(false);
            _pan = false;
        }
        private void BtnLine_Checked(object sender, RoutedEventArgs e)
        {
            _tool = ToolType.Line;
            grid.Cursor = Cursors.Cross;
            CanHit(false);
            _pan = false;
        }
        private void BtnPointer_Checked(object sender, RoutedEventArgs e)
        {
            _tool = ToolType.Pointer;
            grid.Cursor = null;
            CanHit(true);
            _pan = false;
        }
        private void Delete()
        {
            bool deleted = false;

            List<PrimitiveBase> bsList = new List<PrimitiveBase>();
            for (int i = canvas.Children.Count - 1; i >= 1; --i)
            {
                PrimitiveBase g = canvas.Children[i] as PrimitiveBase;
                if (g != null && g.IsSelected)
                {
                    g.cmdActionType = "del";
                    g.PrmIndex = i;
                    bsList.Add(g);
                    canvas.Children.Remove(g);
                    deleted = true;
                }
            }

            if (deleted)
            {
                if (bsList.Count > 0)
                {
                    var tlist = bsList.OrderBy(d => d.PrmIndex).ToList();
                    PrimitiveStackChanged(CommandActionType.Del, tlist);
                }
                if (PrimitiveDeleted != null)
                {
                    PrimitiveDeleted(this, EventArgs.Empty);
                }

                if (SelectionChanged != null)
                {
                    SelectionChanged(this, EventArgs.Empty);
                }
            }
        }
        private void Canvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Delete();
            }



        }
        private void Canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectAll(false);
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            canvas.Focus();
            if (_tool == ToolType.Polyline)
            {
                ToolManager.Instance.Get(_tool).MouseDownPolyLine(canvas, e, canvas.ScreenToWorld, this._polyLine);
            }
            else
            {
                ToolManager.Instance.Get(_tool).MouseDown(canvas, e, canvas.ScreenToWorld);
            }

            SetScale(scale.ScaleX);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            ToolManager.Instance.Get(_tool).MouseMove(canvas, e, canvas.ScreenToWorld);
        }
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ToolManager.Instance.Get(_tool).MouseUp(canvas, e, canvas.ScreenToWorld);

            if (_tool == ToolType.Polyline)
            {
                return;
            }
            List<PrimitiveBase> pbsList = new List<PrimitiveBase>();
            if (_tool != ToolType.Pointer)
            {
                PrimitiveBase prim = canvas.Children[canvas.Children.Count - 1] as PrimitiveBase;
                pbsList = new List<PrimitiveBase>();
                _canvasList.Add(prim);
                prim.cmdActionType = "add";

                prim.PrmIndex = canvas.Children.Count - 1;

                prim.Moved += Primitive_Moved;
                if (PrimitiveAdded != null)
                {
                    PrimitiveAdded(this, EventArgs.Empty);
                    pbsList.Add(prim);
                    PrimitiveStackChanged(CommandActionType.Add, pbsList);
                }
            }
            else
            {
                if (GetSelections().Count > 0)
                {
                    PrimitiveBase prim = GetSelections()[0];
                    pbsList = new List<PrimitiveBase>();
                    if (prim.cmdActionType == "move" || (prim.cmdActionType == string.Empty && prim.IsChange > 0))
                    {
                        //bool d=canvas.Children.Contains(prim);
                        if (prim.GetType().Name.ToLower() == "line" || prim.GetType().Name.ToLower() == "axis"
                            || prim.GetType().Name.ToLower() == "dot" || prim.GetType().Name.ToLower() == "polyline" || prim.GetType().Name.ToLower() == "rectangle")
                        {
                            if (PrimitiveStackChanged != null)
                            {
                                pbsList.Add(prim);
                                prim.PrmIndex = canvas.Children.IndexOf(prim);
                                PrimitiveStackChanged(CommandActionType.Move, pbsList);
                                prim.cmdActionType = string.Empty;
                                prim.IsChange = -1;
                            }
                        }
                    }
                }
                else
                {

                    foreach (var childPrim in canvas.Children)
                    {
                        PrimitiveBase prm = childPrim as PrimitiveBase;
                        if (prm != null && !prm.IsSelected)
                        {
                            prm.cmdActionType = string.Empty;
                            prm.IsChange = -1;
                        }
                    }

                }
            }
            btnPointer.IsChecked = true;
        }

        private void Primitive_Moved(object sender, EventArgs e)
        {
            if (PrimitiveMoved != null)
            {
                PrimitiveMoved(this, EventArgs.Empty);
            }
        }
        private void Pan(double deltaX, double deltaY)
        {
            translate.X += deltaX;
            translate.Y += deltaY;
        }
        private void CanHit(bool hit)
        {
            for (int i = 1; i < canvas.Children.Count; ++i)
            {
                PrimitiveBase g = canvas.Children[i] as PrimitiveBase;
                if (g != null)
                {
                    g.IsHitTestVisible = hit;
                }
            }
        }
        private void SelectAll(bool select)
        {
            for (int i = 1; i < canvas.Children.Count; ++i)
            {
                PrimitiveBase g = canvas.Children[i] as PrimitiveBase;
                if (g != null)
                {
                    g.IsSelected = select;
                }
            }

        }

        private void SetScale(double scale)
        {
            for (int i = 1; i < canvas.Children.Count; ++i)
            {
                PrimitiveBase g = canvas.Children[i] as PrimitiveBase;
                if (g != null)
                {
                    g.Scale = scale;
                }
            }
        }
        private void Select(Rect rect)
        {
            for (int i = 1; i < canvas.Children.Count; ++i)
            {
                PrimitiveBase g = canvas.Children[i] as PrimitiveBase;
                if (g != null)
                {
                    if (rect.Contains(g.RenderedGeometry.Bounds))
                    {
                        g.IsSelected = true;

                        //RectangleGeometry rects = new RectangleGeometry(rect);



                        //System.Windows.Shapes. Path myPath = new System.Windows.Shapes.Path();

                        //myPath.Stroke = Brushes.Black;
                        //myPath.StrokeThickness = 1;
                        //myPath.Data = rects;
                        //canvas.Children.Add(myPath);
                    }
                }
            }
        }
        private List<PrimitiveBase> GetSelections()
        {
            _selections.Clear();

            for (int i = 1; i < canvas.Children.Count; ++i)
            {
                PrimitiveBase prim = canvas.Children[i] as PrimitiveBase;
                if (prim != null && prim.IsSelected)
                {
                    _selections.Add(prim);
                }
            }

            return _selections;
        }
        public void Invalidate()
        {
            for (int i = 1; i < canvas.Children.Count; ++i)
            {
                PrimitiveBase prim = canvas.Children[i] as PrimitiveBase;
                if (prim != null)
                {
                    prim.InvalidateVisual();
                }
            }
        }

        private List<PrimitiveBase> GetChildren()
        {
            _children.Clear();

            for (int i = 1; i < canvas.Children.Count; ++i)
            {
                _children.Add(canvas.Children[i] as PrimitiveBase);
            }

            return _children;
        }
        public void Select(int index)
        {
            PrimitiveBase prim = null;
            if (index < 0)
            {


            }
            else
            {
                prim = canvas.Children[index + 1] as PrimitiveBase;
                prim.IsSelected = true;

                if (SelectionChanged != null)
                    SelectionChanged(this, EventArgs.Empty);
            }
        }
        public void Unselect(int index)
        {
            PrimitiveBase prim = canvas.Children[index + 1] as PrimitiveBase;
            prim.IsSelected = false;

            if (SelectionChanged != null)
                SelectionChanged(this, EventArgs.Empty);
        }
        public void RevokeCanvas()
        {
            if (_canvasList.Count > 0)
            {


            }

        }
        public void Add(PrimitiveBase prim)
        {
            prim.Scale = scale.ScaleX;
            prim.Moved += Primitive_Moved;
            canvas.Children.Add(prim);
            if (PrimitiveAdded != null)
            {
                PrimitiveAdded(this, EventArgs.Empty);
            }
        }
        public void Clear()
        {
            for (int i = canvas.Children.Count - 1; i >= 1; --i)
            {
                PrimitiveBase g = canvas.Children[i] as PrimitiveBase;
                if (g != null)
                {
                    canvas.Children.Remove(g);
                }
            }

            if (PrimitiveDeleted != null)
            {
                PrimitiveDeleted(this, EventArgs.Empty);
            }

            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }
    }
}
