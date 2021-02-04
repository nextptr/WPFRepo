using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DrawingBoard.Serializables;

namespace DrawingBoard.Primitive
{
    public class Rectangle1 : PrimitiveBase
    {
        public double Top
        {
            get
            {
                return (double)base.GetValue(Rectangle1.TopProperty);
            }
            set
            {
                base.SetValue(Rectangle1.TopProperty, value);
            }
        }
        public double Bottom
        {
            get
            {
                return (double)base.GetValue(Rectangle1.BottomProperty);
            }
            set
            {
                base.SetValue(Rectangle1.BottomProperty, value);
            }
        }

        public double Left
        {
            get
            {
                return (double)base.GetValue(Rectangle1.LeftProperty);
            }
            set
            {
                base.SetValue(Rectangle1.LeftProperty, value);
            }
        }
        public double Right
        {
            get
            {
                return (double)base.GetValue(Rectangle1.RightProperty);
            }
            set
            {
                base.SetValue(Rectangle1.RightProperty, value);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return GetDefiningGeometry();
            }
        }
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(Rectangle1), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(Rectangle1), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(double), typeof(Rectangle1), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(double), typeof(Rectangle1), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        public Rectangle1()
        {
            Fill = Brushes.Transparent;
        }

        protected override void OnMoveToOrigin()
        {
            double x = (Left + Right) / 2;
            double y = (Top + Bottom) / 2;

            Left += -x;
            Top += -y;
            Right += -x;
            Bottom += -y;
        }
        protected override void OnPrimitiveMouseMove(MouseEventArgs e)
        {
            Point pt = e.GetPosition(this);
            double dx = pt.X - _last.X;
            double dy = pt.Y - _last.Y;

            if (_handle == 0)
            {
                Left += dx;
                Top += dy;
                Right += dx;
                Bottom += dy;
            }
            else if (_handle == 1)
            {
                Left += dx;
                Top += dy;
            }
            else if (_handle == 2)
            {
                Top += dy;
            }
            else if (_handle == 3)
            {
                Right += dx;
                Top += dy;
            }
            else if (_handle == 4)
            {
                Right += dx;
            }
            else if (_handle == 5)
            {
                Right += dx;
                Bottom += dy;
            }
            else if (_handle == 6)
            {
                Bottom += dy;
            }
            else if (_handle == 7)
            {
                Left += dx;
                Bottom += dy;
            }
            else if (_handle == 8)
            {
                Left += dx;
            }

            _last = pt;
        }
        protected override void OnPrimitiveMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _last = e.GetPosition(this);
            Rect[] rect = GetHandles();

            _handle = -1;

            for (int i = 0; i < rect.Length; ++i)
            {
                if (rect[i].Contains(_last))
                {
                    _handle = i + 1;
                    break;
                }
            }

            if (_handle == -1)
            {
                Rect body = new Rect(new Point(Left, Top), new Point(Right, Bottom));
                if (body.Contains(_last))
                {
                    _handle = 0;
                }
            }
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Stroke, ActualStrokeThickness);
            Rect rect = new Rect(new Point(Left, Top), new Point(Right, Bottom));
            drawingContext.DrawRectangle(Fill, pen, rect);

            if (IsSelected)
            {
                Rect[] rectangles = GetHandles();
                foreach (Rect r in rectangles)
                {
                    DrawHandle(drawingContext, r);
                }
            }
            else
            {

            }
            // call base.OnRender will invoke DefiningGeometry
            //base.OnRender(drawingContext);
        }
        protected override void OnMoved(EventArgs e)
        {
            base.OnMoved(e);
        }

        private Geometry GetDefiningGeometry()
        {
            RectangleGeometry bodyGeomety = new RectangleGeometry();
            RectangleGeometry[] handleGeomety = new RectangleGeometry[8];

            bodyGeomety.Rect = new Rect(new Point(Left, Top), new Point(Right, Bottom));

            GeometryGroup gg = new GeometryGroup();
            gg.Children.Add(bodyGeomety);

            if (IsSelected)
            {
                Rect[] rectangles = GetHandles();

                for (int i = 0; i < rectangles.Length; ++i)
                {
                    handleGeomety[i] = new RectangleGeometry();
                    handleGeomety[i].Rect = rectangles[i];
                    gg.Children.Add(handleGeomety[i]);
                }
            }

            return gg;
        }
        public override SerializableBase CreateSerializableObject()
        {
            return null;
        }

        public override void Move(double x, double y)
        {
            this.Left += x;
            this.Right += x;
            this.Top += y;
            this.Bottom += y;
        }
        public override object Clone()
        {
            Rectangle1 clone = new Rectangle1();
            clone.Top = this.Top;
            clone.Left = this.Left;
            clone.Bottom = this.Bottom;
            clone.Right = this.Right;

            return clone;
        }
        private Rect[] GetHandles()
        {
            Rect[] rect = new Rect[8];

            rect[0] = GetHandleRect(Left, Top);
            rect[1] = GetHandleRect((Right + Left) / 2, Top);
            rect[2] = GetHandleRect(Right, Top);

            rect[3] = GetHandleRect(Right, (Top + Bottom) / 2);

            rect[4] = GetHandleRect(Right, Bottom);
            rect[5] = GetHandleRect((Right + Left) / 2, Bottom);
            rect[6] = GetHandleRect(Left, Bottom);

            rect[7] = GetHandleRect(Left, (Top + Bottom) / 2);

            return rect;
        }
    }
}
