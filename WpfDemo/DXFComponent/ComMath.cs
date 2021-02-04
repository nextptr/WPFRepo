using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DXFComponent
{
    public class ComMath
    {
        public static byte[] Hex16StringToByteArry(string str)
        {
            string tmpstr = "";
            List<int> tmpint = new List<int>();
            for (int i = 0; i < str.Length; i += 2)
            {
                tmpstr = "";
                tmpstr = tmpstr + str[i] + str[i + 1];
                tmpint.Add((int)Convert.ToInt32(tmpstr, 16));
            }
            byte[] arry = new byte[tmpint.Count];
            for (int i = 0; i < tmpint.Count; i++)
            {
                arry[i] = (byte)tmpint[i];
            }
            return arry;
        }

        public static string ByteArryToHex16String(byte[] arr)
        {
            StringBuilder info = new StringBuilder();
            int num = 0;
            int mod = 0;
            foreach (byte bt in arr)
            {
                num = (int)bt;
                //高4位
                mod = (num >> 4) & 15;
                info.AppendFormat("{0:x}", mod);
                //低4位
                mod = num & 15;
                info.AppendFormat("{0:x}", mod);
            }
            return info.ToString();
        }

        public static byte[] AsiicStringToByteArry(string str)
        {
            List<int> tmpint = new List<int>();
            foreach (char ch in str)
            {
                tmpint.Add((int)ch);
            }
            byte[] arry = new byte[tmpint.Count];
            for (int i = 0; i < tmpint.Count; i++)
            {
                arry[i] = (byte)tmpint[i];
            }
            return arry;
        }


        public static double PointDistance(Point p1, Point p2)
        {
            double dx = Math.Abs(p1.X - p2.X);
            double dy = Math.Abs(p1.Y - p2.Y);
            double dis = Math.Sqrt(dx * dx + dy * dy);
            return dis;
        }

        public static Point PointRotate(double x, double y, double ang)
        {
            double angle = ang / 180 * Math.PI;
            Point pos = new Point();
            pos.X = x * Math.Cos(angle) - y * Math.Sin(angle);
            pos.Y = y * Math.Cos(angle) + x * Math.Sin(angle);
            return pos;
        }
        public static Point PointRotate(Point point, double ang)
        {
            double angle = ang / 180 * Math.PI;
            Point pos = new Point();
            pos.X = point.X * Math.Cos(angle) - point.Y * Math.Sin(angle);
            pos.Y = point.Y * Math.Cos(angle) + point.X * Math.Sin(angle);
            return pos;
        }

        public static Double PointAngle(Point pos)
        {
            double ang = 0.0;
            double x = pos.X;
            double y = pos.Y;
            double r = Math.Sqrt(x * x + y * y);
            if (r == 0)
            {
                return 0;
            }
            double cosVal = x / r;
            double angCos = Math.Acos(cosVal);
            double angle = angCos / Math.PI * 180;

            if (y > 0)
            {
                ang = angle;
            }
            else if (y == 0)
            {
                ang = angle;
            }
            else if (y < 0)
            {
                ang = 360 - angle;
            }
            return ang;
        }

        public static Point PointScale(Point cenPos, double x, double y, double rate)
        {
            double rat = rate - 1;
            Point pos = new Point();
            pos.X = x + (x - cenPos.X) * rat;
            pos.Y = y + (y - cenPos.Y) * rat;
            return pos;
        }
        public static Point PointScale(Point cenPos, Point srcPos, double rate)
        {
            double rat = rate - 1;
            Point pos = new Point();
            pos.X = srcPos.X + (srcPos.X - cenPos.X) * rat;
            pos.Y = srcPos.Y + (srcPos.Y - cenPos.Y) * rat;
            return pos;
        }
    }
}
