using System.Runtime.InteropServices;

namespace  Common.Extension
{
    public static class ByteArrayExtension
    {
        public static T ByteArrayToStructure<T>(this byte[] bytes) where T : struct
        {
            T stuff;
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return stuff;
        }
    }
}
