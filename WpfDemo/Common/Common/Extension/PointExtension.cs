using System.Windows;
using System.Windows.Media;

namespace  Common.Extension
{
    public static class PointExtension
    {
        public static double Length(this Point p1, ref Point p2)
        {
            return Length(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static double Length2(this Point p1, ref Point p2)
        {
            return Length2(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static Point Rotate(this Point p1, double x, double y, double degree)
        {
            Matrix m = new Matrix();
            m.RotateAt(degree, x, y);
            return m.Transform(p1);
        }

        public static bool Equal(this Point p1, Point p2, double tolerance)
        {
            return (Length2(p1, ref p2) <= tolerance * tolerance);
        }

        public static double Length(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return System.Math.Sqrt(dx * dx + dy * dy);
        }

        public static double Length2(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return (dx * dx + dy * dy);
        }
    }
}
