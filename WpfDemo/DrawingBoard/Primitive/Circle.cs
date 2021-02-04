using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DrawingBoard.Serializables;

namespace DrawingBoard.Primitive
{
    public class Circle : PrimitiveBase
    {
        public double X
        {
            get
            {
                return (double)base.GetValue(Circle.XProperty);
            }
            set
            {
                base.SetValue(Circle.XProperty, value);
            }
        }

        public double Y
        {
            get
            {
                return (double)base.GetValue(Circle.YProperty);
            }
            set
            {
                base.SetValue(Circle.YProperty, value);
            }
        }

        public double Radius
        {
            get
            {
                return (double)base.GetValue(Circle.RadiusProperty);
            }
            set
            {
                base.SetValue(Circle.RadiusProperty, value);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return GetDefiningGeometry();
            }
        }

        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(Circle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(Circle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(Circle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        public Circle()
        {
            Fill = Brushes.Transparent;
        }

        protected override void OnMoveToOrigin()
        {
            X = 0;
            Y = 0;
        }

        protected override void OnPrimitiveMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _last = e.GetPosition(this);
            Rect r = GetHandleRect(X + Radius, Y);
            if (r.Contains(_last))
            {
                _handle = 1;
            }
            else
            {
                _handle = 0;
            }
        }

        protected override void OnPrimitiveMouseMove(MouseEventArgs e)
        {
            int int_size = sizeof(int);

            Point pt = e.GetPosition(this);
            double dx = pt.X - _last.X;
            double dy = pt.Y - _last.Y;
            _isChange = -1;
            if (_handle == 0)
            {
                X += dx;
                Y += dy;
                _isChange = 1;
            }
            else
            {
                Radius += dx;
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
            ellipseGeometry.RadiusX = Radius;
            ellipseGeometry.RadiusY = Radius;

            gg.Children.Add(ellipseGeometry);

            if (IsSelected)
            {
                rectGeomety.Rect = GetHandleRect(X + Radius, Y);
                gg.Children.Add(rectGeomety);
            }

            return gg;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Stroke, ActualStrokeThickness);

            drawingContext.DrawEllipse(Fill, pen, new Point(X, Y), Radius, Radius);

            if (IsSelected)
            {
                DrawHandle(drawingContext, GetHandleRect(X + Radius, Y));
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
            SerializableCircle circle = new SerializableCircle(this);
            return circle;
        }

        public override object Clone()
        {
            Circle clone = new Circle();
            clone.X = this.X;
            clone.Y = this.Y;
            clone.Radius = this.Radius;
            return clone;
        }

        public override void Move(double x, double y)
        {
            this.X += x;
            this.Y += y;
        }
    }
}
