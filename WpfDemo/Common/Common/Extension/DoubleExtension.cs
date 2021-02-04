
namespace  Common.Extension
{
    public static class DoubleExtension
    {
        public static bool Equals(this double me, double other, double tolerance = 1E-3)
        {
            return System.Math.Abs(me - other) < tolerance;
        }
    }
}
