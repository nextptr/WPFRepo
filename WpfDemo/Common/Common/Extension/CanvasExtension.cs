using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace  Common.Extension
{
    public static class CanvasExtension
    {
        public static void Fit(this Canvas canvas, Rect bound, Size container)
        {
            Point translation = bound.Center();
            double s = GetFitScale(bound, container);

            Matrix m = new Matrix();
            m.SetIdentity();
            m.Translate(container.Width / 2.0 - translation.X, container.Height / 2.0 - translation.Y);
            m.ScaleAt(s, s, container.Width / 2.0, container.Height / 2.0);

            MatrixTransform mt = canvas.RenderTransform as MatrixTransform;
            if (MatrixTransform.Identity == mt)
            {
                mt = new MatrixTransform(m);
                canvas.RenderTransform = mt;
            }
            else
            {
                mt.Matrix = m;
            }
        }

        public static Matrix GetFitMatrix(Rect bound, Size container)
        {
            if (bound.IsEmpty)
            {
                return Matrix.Identity;
            }

            Point translation = bound.Center();
            double s = GetFitScale(bound, container);

            Matrix m = new Matrix();
            m.SetIdentity();
            m.Translate(container.Width / 2.0 - translation.X, container.Height / 2.0 - translation.Y);
            m.ScaleAt(s, s, container.Width / 2.0, container.Height / 2.0);

            return m;
        }

        private static double GetFitScale(Rect bound, Size container)
        {
            double sx = container.Width / bound.Width;
            double sy = container.Height / bound.Height;
            double s = System.Math.Min(sx, sy);

            return s;
        }
    }
}
