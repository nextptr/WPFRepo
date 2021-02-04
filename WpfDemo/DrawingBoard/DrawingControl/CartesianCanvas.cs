using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DrawingBoard.DrawingControl
{
    public class CartesianCanvas : Panel
    {
        public Matrix ScreenToWorld
        {
            get
            {
                Matrix m = new Matrix();
                m.M11 = 1;
                m.M21 = 0;
                m.M21 = 0;
                m.M22 = -1;
                m.OffsetX = -Origin.X;
                m.OffsetY = Origin.Y;

                return m;
            }
        }

        public Point Origin
        {
            get
            {
                return new Point(ActualWidth / 2, ActualHeight / 2);
            }
        }

        public CartesianCanvas()
        {
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Point middle = new Point(arrangeSize.Width / 2, arrangeSize.Height / 2);

            double x = 0.0, y = 0.0;
            foreach (UIElement element in base.InternalChildren)
            {
                if (element == null)
                    continue;

                element.RenderTransform = new MatrixTransform(1, 0, 0, -1, 0, 0);
                element.Arrange(new Rect(new Point(middle.X + x, middle.Y + y), element.DesiredSize));
            }
            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            foreach (UIElement element in base.InternalChildren)
            {
                if (element != null)
                {
                    element.Measure(availableSize);
                }
            }
            return new Size();
        }
    }
}
