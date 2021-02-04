using System;
using System.Reflection;

namespace  Common.Extension
{
    public static class ReflectionExtension
    {
        public static object Clone<T>(this T me)
        {
            object clone = Activator.CreateInstance(typeof(T));

            FieldInfo[] fileds = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fileds.Length; ++i)
            {
                FieldInfo fld = fileds[i];
                fld.SetValue(clone, fld.GetValue(me));
            }
            return clone;
        }

        public static void Copy<T>(this T me, T other)
        {
            FieldInfo[] fileds = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fileds.Length; ++i)
            {
                FieldInfo fld = fileds[i];
                fld.SetValue(me, fld.GetValue(other));
            }
        }

        //public static void Compare<T>(this T me, T other, Action<string, object, object> action)
        //{
        //    FieldInfo[] fileds = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        //    for (int i = 0; i < fileds.Length; ++i)
        //    {
        //        FieldInfo fld = fileds[i];

        //        object v1 = fld.GetValue(me);
        //        object v2 = fld.GetValue(other);

        //        if (!v1.Equals(v2))
        //        {
        //            if (action != null)
        //            {
        //                action(fld.Name, v1, v2);
        //            }
        //        }
        //    }
        //}

        public static void Compare<T>(this T me, T other, Action<string, object, object> action)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < properties.Length; ++i)
            {
                PropertyInfo fld = properties[i];

                object v1 = fld.GetValue(me, null);
                object v2 = fld.GetValue(other, null);

                if (!v1.Equals(v2))
                {
                    if (action != null)
                    {
                        action(fld.Name, v1, v2);
                    }
                }
            }
        }

        public static void Compare<T>(this T me, T other, Action<PropertyInfo, T, T> action)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < properties.Length; ++i)
            {
                PropertyInfo fld = properties[i];

                object v1 = fld.GetValue(me, null);
                object v2 = fld.GetValue(other, null);

                if (!v1.Equals(v2))
                {
                    if (action != null)
                    {
                        action(fld, me, other);
                    }
                }
            }
        }
    }
}
