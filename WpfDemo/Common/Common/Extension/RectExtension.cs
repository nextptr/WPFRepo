using System.Windows;

namespace  Common.Extension
{
    public static class RectExtension
    {
        public static Point Center(this Rect me)
        {
            double x = me.X + me.Width / 2.0;
            double y = me.Y + me.Height / 2.0;

            return new Point(x, y);
        }

        public static double Area(this Rect me)
        {
            return me.Width * me.Height;
        }
    }
}
