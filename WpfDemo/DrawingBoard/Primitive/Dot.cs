using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using DrawingBoard.Serializables;

namespace DrawingBoard.Primitive
{
    public class Dot : PrimitiveBase
    {
        public double X
        {
            get
            {
                return (double)base.GetValue(Dot.XProperty);
            }
            set
            {
                base.SetValue(Dot.XProperty, value);
            }
        }

        public double Y
        {
            get
            {
                return (double)base.GetValue(Dot.YProperty);
            }
            set
            {
                base.SetValue(Dot.YProperty, value);
            }
        }

        public int Duration
        {
            get
            {
                return (int)base.GetValue(Dot.DurationProperty);
            }
            set
            {
                base.SetValue(Dot.DurationProperty, value);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return GetDefiningGeometry();
            }
        }

        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(Dot), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(Dot), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(int), typeof(Dot), new FrameworkPropertyMetadata(1000));

        private static readonly double SIZE = 0.5;

        public Dot()
        {
            Fill = Brushes.Black;
        }

        protected override void OnMoveToOrigin()
        {
            X = 0;
            Y = 0;
        }

        protected override void OnPrimitiveMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _handle = -1;

            _last = e.GetPosition(this);
            Rect r = GetHandleRect(X, Y);
            if (r.Contains(_last))
            {
                _handle = 0;
            }
        }

        protected override void OnPrimitiveMouseMove(MouseEventArgs e)
        {
            Point pt = e.GetPosition(this);
            double dx = pt.X - _last.X;
            double dy = pt.Y - _last.Y;
            _isChange =-1;
            if (_handle == 0)
            {
                X += dx;
                Y += dy;
                _isChange = 1;
            }

            _last = pt;
        }
    
        private Geometry GetDefiningGeometry()
        {
            EllipseGeometry ellipseGeometry = new EllipseGeometry();
            RectangleGeometry rectGeomety = new RectangleGeometry();
            GeometryGroup gg = new GeometryGroup();

            ellipseGeometry.Center = new Point(X, Y);
            ellipseGeometry.RadiusX = SIZE;
            ellipseGeometry.RadiusY = SIZE;

            gg.Children.Add(ellipseGeometry);

            if (IsSelected)
            {
                rectGeomety.Rect = GetHandleRect(X, Y); 
                gg.Children.Add(rectGeomety);
            }

            return gg;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Stroke, ActualStrokeThickness);

            drawingContext.DrawEllipse(Fill, pen, new Point(X, Y), SIZE, SIZE);

            if (IsSelected)
            {
                DrawHandle(drawingContext, GetHandleRect(X, Y));
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

        public override SerializableBase CreateSerializableObject()
        {
            SerializableDot dot = new SerializableDot(this);
            return dot;
        }

        public override object Clone()
        {
            Dot clone = new Dot();
            clone.X = this.X;
            clone.Y = this.Y;
            clone.Duration = this.Duration;
            return clone;
        }

        public override void Move(double x, double y)
        {
            this.X += x;
            this.Y += y;
        }
    }
}
