using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DrawingBoard.Serializables;

namespace DrawingBoard.Primitive
{
    public class Rectangle : PrimitiveBase
    {
        public double Top
        {
            get
            {
                return (double)base.GetValue(Rectangle.TopProperty);
            }
            set
            {
                base.SetValue(Rectangle.TopProperty, value);
            }
        }
        public double Left
        {
            get
            {
                return (double)base.GetValue(Rectangle.LeftProperty);
            }
            set
            {
                base.SetValue(Rectangle.LeftProperty, value);
            }
        }

        new public double Width
        {
            get
            {
                return (double)base.GetValue(Rectangle.WidthProperty);
            }
            set
            {
                base.SetValue(Rectangle.WidthProperty, value);
            }
        }
        new public double Height
        {
            get
            {
                return (double)base.GetValue(Rectangle.HeightProperty);
            }
            set
            {
                base.SetValue(Rectangle.HeightProperty, value);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return GetDefiningGeometry();
            }
        }

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(Rectangle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(Rectangle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(double), typeof(Rectangle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(double), typeof(Rectangle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        new public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(Rectangle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        new public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(double), typeof(Rectangle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        public Rectangle()
        {
            Fill = Brushes.Transparent;
        }

        protected override void OnMoveToOrigin()
        {
            double x = Left + Width / 2;
            double y = Top - Height / 2;

            Left += -x;
            Top += -y;
        }

        protected override void OnPrimitiveMouseMove(MouseEventArgs e)
        {
            Point pt = e.GetPosition(this);
            double dx = pt.X - _last.X;
            double dy = pt.Y - _last.Y;
            _isChange = -1;
            if (_handle == 0)
            {
                Left += dx;
                Top += dy;
                _isChange = 1;
            }
            else if (_handle == 1)
            {
                Left += dx;
                Top += dy;
                Width -= dx;
                Height += dy;
                _isChange = 1;
            }
            else if (_handle == 2)
            {
                Top += dy;
                Height += dy;
                _isChange = 1;
            }
            else if (_handle == 3)
            {
                Width += dx;
                Top += dy;
                Height += dy;
                _isChange = 1;
            }
            else if (_handle == 4)
            {
                Width += dx;
                _isChange = 1;
            }
            else if (_handle == 5)
            {
                Width += dx;
                Height -= dy;
                _isChange = 1;
            }
            else if (_handle == 6)
            {
                Height -= dy;
                _isChange = 1;
            }
            else if (_handle == 7)
            {
                Left += dx;
                Width -= dx;
                Height -= dy;
                _isChange = 1;
            }
            else if (_handle == 8)
            {
                Left += dx;
                Width -= dx;
                _isChange = 1;
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
                Rect body = new Rect(new Point(Left, Top), new Point(Left + Width, Top - Height));
                if (body.Contains(_last))
                {
                    _handle = 0;
                }
            }
        }

        private Geometry GetDefiningGeometry()
        {
            RectangleGeometry bodyGeomety = new RectangleGeometry();
            RectangleGeometry[] handleGeomety = new RectangleGeometry[8];

            bodyGeomety.Rect = new Rect(new Point(Left, Top), new Point(Left + Width, Top - Height));

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

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Stroke, ActualStrokeThickness);
            Rect rect = new Rect(new Point(Left, Top), new Point(Left + Width, Top - Height));
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

        private Rect[] GetHandles()
        {
            double x = Left + Width / 2;
            double y = Top - Height / 2;
            double right = Left + Width;
            double bottom = Top - Height;

            Rect[] rect = new Rect[8];

            rect[0] = GetHandleRect(Left, Top);
            rect[1] = GetHandleRect(x, Top);
            rect[2] = GetHandleRect(right, Top);

            rect[3] = GetHandleRect(right, y);

            rect[4] = GetHandleRect(right, bottom);
            rect[5] = GetHandleRect(x, bottom);
            rect[6] = GetHandleRect(Left, bottom);

            rect[7] = GetHandleRect(Left, y);

            return rect;
        }

        protected override void OnMoved(EventArgs e)
        {
            base.OnMoved(e);
        }

        public override SerializableBase CreateSerializableObject()
        {
            SerializableRectangle rect = new SerializableRectangle(this);
            return rect;
        }

        public override object Clone()
        {
            Rectangle clone = new Rectangle();
            clone.Top = this.Top;
            clone.Left = this.Left;
            clone.Width = this.Width;
            clone.Height = this.Height;

            return clone;
        }

        public override void Move(double x, double y)
        {
            this.Left += x;
            this.Top += y;
        }
    }
}
