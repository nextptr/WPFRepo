using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawingBoard.Primitive
{
    public class Axis : Shape
    {
        public double Left
        {
            get
            {
                return (double)base.GetValue(Axis.LeftProperty);
            }
            set
            {
                base.SetValue(Axis.LeftProperty, value);
            }
        }

        public double Top
        {
            get
            {
                return (double)base.GetValue(Axis.TopProperty);
            }
            set
            {
                base.SetValue(Axis.TopProperty, value);
            }
        }

        public double Right
        {
            get
            {
                return (double)base.GetValue(Axis.RightProperty);
            }
            set
            {
                base.SetValue(Axis.RightProperty, value);
            }
        }

        public double Bottom
        {
            get
            {
                return (double)base.GetValue(Axis.BottomProperty);
            }
            set
            {
                base.SetValue(Axis.BottomProperty, value);
            }
        }

        public double Step
        {
            get
            {
                return (double)base.GetValue(Axis.StepProperty);
            }
            set
            {
                base.SetValue(Axis.StepProperty, value);
            }
        }

        public double Scale
        {
            get
            {
                return (double)base.GetValue(Axis.ScaleProperty);
            }
            set
            {
                base.SetValue(Axis.ScaleProperty, value);
            }
        }

        protected double ActualStrokeThickness
        {
            get
            {
                double thickness = Scale <= 1 ? StrokeThickness : (StrokeThickness / Scale);
                return thickness;
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return GetDefiningGeometry();
            }
        }

        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(Axis), new FrameworkPropertyMetadata(-100.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(double), typeof(Axis), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(Axis), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(double), typeof(Axis), new FrameworkPropertyMetadata(-100.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(Axis), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnStepChanged), new CoerceValueCallback(CoerceStep)));
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(Axis), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public Axis()
        {
        }

        private Geometry GetDefiningGeometry()
        {
            GeometryGroup gg = new GeometryGroup();

            for (double i = Bottom; i <= Top; i += Step)
            {
                if (i == 0)
                {
                    continue;
                }

                LineGeometry lineGeometry = new LineGeometry();
                lineGeometry.StartPoint = new Point(Left, i);
                lineGeometry.EndPoint = new Point(Right, i);
                gg.Children.Add(lineGeometry);
            }

            for (double i = Left; i <= Right; i += Step)
            {
                if (i == 0)
                {
                    continue;
                }

                LineGeometry lineGeometry = new LineGeometry();
                lineGeometry.StartPoint = new Point(i, Top);
                lineGeometry.EndPoint = new Point(i, Bottom);
                gg.Children.Add(lineGeometry);
            }

            return gg;
        }

        private static void OnStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(StepProperty);
        }

        private static object CoerceStep(DependencyObject d, object value)
        {
            Axis g = (Axis)d;
            double current = (double)value;
            if (current < 10)
                current = 10;
            return current;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            RenderGrid(drawingContext);
            RenderXY(drawingContext);

            // call base.OnRender will invoke DefiningGeometry
            //base.OnRender(drawingContext);
        }

        private void RenderGrid(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Stroke, ActualStrokeThickness);

            for (double i = Bottom; i <= Top; i += Step)
            {
                if (i == 0)
                {
                    continue;
                }

                Point p1 = new Point(Left, i);
                Point p2 = new Point(Right, i);
                drawingContext.DrawLine(pen, p1, p2);
            }

            for (double i = Left; i <= Right; i += Step)
            {
                if (i == 0)
                {
                    continue;
                }

                Point p1 = new Point(i, Top);
                Point p2 = new Point(i, Bottom);
                drawingContext.DrawLine(pen, p1, p2);
            }
        }

        private void RenderXY(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Brushes.Red, ActualStrokeThickness + 0.2);

            {
                Point p1 = new Point(Left - 1000, 0);
                Point p2 = new Point(Right + 1000, 0);
                drawingContext.DrawLine(pen, p1, p2);
            }

            {
                Point p1 = new Point(0, Top + 1000);
                Point p2 = new Point(0, Bottom - 1000);
                drawingContext.DrawLine(pen, p1, p2);
            }
        }
    }
}
