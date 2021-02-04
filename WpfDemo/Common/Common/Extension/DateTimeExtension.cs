using System;

namespace  Common.Extension
{
    public static class DateTimeExtension
    {
        public static string ToYMDHMS(this DateTime me)
        {
            return me.Year.ToString("0000-") +
                   me.Month.ToString("00-") +
                   me.Day.ToString("00-") +
                   me.Hour.ToString("00-") +
                   me.Minute.ToString("00-") +
                   me.Second.ToString("00-") +
                   me.Millisecond.ToString("00");
        }

        public static string ToYMD(this DateTime me)
        {
            return me.Year.ToString("0000-") +
                   me.Month.ToString("00-") +
                   me.Day.ToString("00");

        }

        public static string ToHMS(this DateTime me)
        {
            string h = string.Format("{0:00}:", me.Hour);
            string m = string.Format("{0:00}:", me.Minute);
            string s = string.Format("{0:00}", me.Second);
            return h + m + s;
        }
    }
}
