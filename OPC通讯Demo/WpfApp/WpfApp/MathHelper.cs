using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class MathHelper
    {
        public static UInt16 SetBit(UInt16 src, int index)
        {
            if (index <= 0)
                return src;
            UInt16 ret = (UInt16)(1 << index-1);
            return (UInt16)(ret | src);
        }
    }
}
