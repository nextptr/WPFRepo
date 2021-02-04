using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using DrawingBoard.Serializables;

namespace DrawingBoard.Primitive
{
    public abstract class PrimitiveBase : Shape
    {
       
        public event EventHandler Moved;
        public event EventHandler IsSelectedChanged;
     

        protected static double HANDLE_SIZE = 10;
       
        public CommandType cmdType;

        public string  cmdActionType=string.Empty;
        //public Dictionary<PrimitiveBase, LaserParameterItem>

        protected Point _last;
        protected int _handle = -1;

        protected int _isChange = -1;
        public int PrmIndex { get; set; }
        //private static PrimitiveBase GetInstance()
        //{
        //    if (null == _instance)
        //    {
        //        _instance = new PrimitiveBase();
        //    }
        //    return _instance;
        //}

        public bool IsSelected
        {
            get
            {
                return (bool)base.GetValue(PrimitiveBase.IsSelectedProperty);
            }
            set
            {
                base.SetValue(PrimitiveBase.IsSelectedProperty, value);
            }
        }

        public double Scale
        {
            get
            {
                return (double)base.GetValue(PrimitiveBase.ScaleProperty);
            }
            set
            {
                base.SetValue(PrimitiveBase.ScaleProperty, value);
            }
        }

        public int IsChange
        {
            get
            {
                return _isChange;
            }
            set
            {
                _isChange = value;
            }
           
        }

        protected double ActualStrokeThickness
        {
            get
            {
                double thickness = (Scale <= 1) ? StrokeThickness : (StrokeThickness / Scale);
                return thickness;
            }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(PrimitiveBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, OnIsSelectedChanged));
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(PrimitiveBase), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public PrimitiveBase()
        {
            Stroke = Brushes.Black;
            StrokeThickness = 1;

            this.MouseLeftButtonDown += PrimitiveBase_MouseLeftButtonDown;
            this.MouseLeftButtonUp += PrimitiveBase_MouseLeftButtonUp;
            this.MouseMove += PrimitiveBase_MouseMove;
            this.MouseRightButtonDown += PrimitiveBase_MouseRightButtonDown;
           
        }

        private void PrimitiveBase_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                OnPrimitiveMouseMove(e);
                e.Handled = true;
                OnMoved(EventArgs.Empty);
            }
        }

        private void PrimitiveBase_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnPrimitiveMouseLeftButtonUp(e);
            this.ReleaseMouseCapture();
        }

        private void PrimitiveBase_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = true;
            OnPrimitiveMouseLeftButtonDown(e);
            this.CaptureMouse();
            e.Handled = true;
        }

        private void PrimitiveBase_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsSelected)
            {
                PrimitiveContexMenu menu = new PrimitiveContexMenu();
                MenuItem mi = menu.GetItem(0);
                mi.Click += MiMoveToOrigin_Click;
                menu.IsOpen = true;
            }
        }

        private void MiMoveToOrigin_Click(object sender, RoutedEventArgs e)
        {
            OnMoveToOrigin();
            OnMoved(EventArgs.Empty);
        }

        protected virtual void OnPrimitiveMouseLeftButtonDown(MouseButtonEventArgs e)
        {

        }

        protected virtual void OnPrimitiveMouseLeftButtonUp(MouseButtonEventArgs e)
        {

        }

        protected virtual void OnPrimitiveMouseMove(MouseEventArgs e)
        {

        }

        protected virtual void OnMoveToOrigin()
        {

        }

        protected virtual void OnMoved(EventArgs e)
        {
            if (Moved != null)
            {
                Moved(this, e);
               
            }
        }

        protected virtual void OnIsSelectedChanged(EventArgs e)
        {
            if (IsSelectedChanged != null)
            {
                IsSelectedChanged(this, e);
            }
        }

        public abstract SerializableBase CreateSerializableObject();
        public abstract object Clone();
        public abstract void Move(double x, double y);
      

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PrimitiveBase prim = (PrimitiveBase)d;
            prim.OnIsSelectedChanged(EventArgs.Empty);
        }

        internal Rect GetHandleRect(double x, double y)
        {
            double size = Math.Max(HANDLE_SIZE / Scale, ActualStrokeThickness);

            Rect rect = new Rect(new Point(x - size / 2, y - size / 2),
                                 new Point(x + size / 2, y + size / 2));
            return rect;
        }

        internal void DrawHandle(DrawingContext drawingContext, Rect rect)
        {
            // External
            drawingContext.DrawRectangle(Brushes.Black, null, rect);

            // Middle
            drawingContext.DrawRectangle(Brushes.White, null,
                new Rect(rect.Left + rect.Width / 8,
                         rect.Top + rect.Height / 8,
                         rect.Width * 6 / 8,
                         rect.Height * 6 / 8));

            // Internal
            drawingContext.DrawRectangle(Brushes.Blue, null,
                new Rect(rect.Left + rect.Width / 4,
                 rect.Top + rect.Height / 4,
                 rect.Width / 2,
                 rect.Height / 2));
        }

    }

    public enum CommandType
    {
        Move,       // 平移或者修改
        Add,        // 新增
        Del,        // 删除
        Cut,        // 剪切
        Copy,       // 右转
    }
}
