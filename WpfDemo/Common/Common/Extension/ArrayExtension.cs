using System;
using System.Linq;

namespace  Common.Extension
{
    public static class ArrayExtension
    {
        public static void SetValue<T>(T[] array, T value)
        {
            T[] from = Enumerable.Repeat<T>(value, array.Length).ToArray();
            Array.Copy(from, array, array.Length);
        }
    }
}
