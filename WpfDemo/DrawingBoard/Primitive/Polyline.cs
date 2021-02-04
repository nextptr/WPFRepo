using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DrawingBoard.Serializables;

namespace DrawingBoard.Primitive
{
    public class Polyline : PrimitiveBase
    {
        public PointCollection Points
        {
            get
            {
                return (PointCollection)base.GetValue(Polyline.PointsProperty);
            }
            set
            {
                base.SetValue(Polyline.PointsProperty, value);
            }
        }

        public bool IsClosed
        {
            get
            {
                return (bool)base.GetValue(Polyline.IsClosedProperty);
            }
            set
            {
                base.SetValue(Polyline.IsClosedProperty, value);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return GetDefiningGeometry();
            }
        }

        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register("Points", typeof(PointCollection), typeof(Polyline), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register("IsClosed", typeof(bool), typeof(Polyline), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public Polyline()
        {
            SetValue(PointsProperty, new PointCollection());

            this.MouseLeftButtonDown += Graphic_MouseLeftButtonDown;
            this.MouseLeftButtonUp += Graphic_MouseLeftButtonUp;
            this.MouseMove += Graphic_MouseMove;
        }

        private void Graphic_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                Point pt = e.GetPosition(this);
                double dx = pt.X - _last.X;
                double dy = pt.Y - _last.Y;

                if (_handle == 0)
                {
                    for (int i = 0; i < Points.Count; ++i)
                    {
                        Point p = Points[i];
                        p.X += dx;
                        p.Y += dy;
                        Points[i] = p;
                    }
                }
                else
                {
                    Point p = Points[_handle - 1];
                    p.X += dx;
                    p.Y += dy;
                    Points[_handle - 1] = p;
                }

                _last = pt;
                e.Handled = true;
                OnMoved(EventArgs.Empty);
            }
        }

        private void Graphic_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
        }

        private void Graphic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = true;
            _last = e.GetPosition(this);
            _handle = 0;

            for (int i = 0; i < Points.Count; ++i)
            {
                Point p = Points[i];
                Rect r = GetHandleRect(p.X, p.Y);
                if (r.Contains(_last))
                {
                    _handle = i + 1;
                }
            }

            this.CaptureMouse();

            e.Handled = true;
        }

        private Geometry GetDefiningGeometry()
        {
            GeometryGroup gg = new GeometryGroup();

            for (int i = 0; i < Points.Count - 1; ++i)
            {
                LineGeometry lineGeometry = new LineGeometry();
                lineGeometry.StartPoint = Points[i];
                lineGeometry.EndPoint = Points[i + 1];
                gg.Children.Add(lineGeometry);
            }

            if (IsClosed)
            {
                LineGeometry lineGeometry = new LineGeometry();
                lineGeometry.StartPoint = Points[Points.Count - 1];
                lineGeometry.EndPoint = Points[0];
                gg.Children.Add(lineGeometry);
            }

            if (IsSelected)
            {
                for (int i = 0; i < Points.Count; ++i)
                {
                    Point p = Points[i];
                    RectangleGeometry rectGeometry = new RectangleGeometry();
                    rectGeometry.Rect = GetHandleRect(p.X, p.Y);
                    gg.Children.Add(rectGeometry);
                }
            }

            return gg;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Stroke, ActualStrokeThickness);

            PathFigureCollection figures = new PathFigureCollection();
            PathGeometry pg = new PathGeometry();
            pg.Figures = figures;

            PathFigure figure = new PathFigure();
            figure.IsClosed = IsClosed;
            figure.StartPoint = Points[0];

            for (int i = 1; i < Points.Count; ++i)
            {
                LineSegment seg = new LineSegment();
                seg.Point = Points[i];
                figure.Segments.Add(seg);
            }
            figures.Add(figure);

            if (IsClosed)
            {
                drawingContext.DrawGeometry(Brushes.Transparent, pen, pg);
            }
            else
            {
                drawingContext.DrawGeometry(null, pen, pg);
            }

            if (IsSelected)
            {
                for (int i = 0; i < Points.Count; ++i)
                {
                    Point p = Points[i];
                    DrawHandle(drawingContext, GetHandleRect(p.X, p.Y));
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

        public override SerializableBase CreateSerializableObject()
        {
            return null;
        }

        public override object Clone()
        {
            Polyline clone = new Polyline();
            return clone;
        }

        public override void Move(double x, double y)
        {
           
        }
    }
}
