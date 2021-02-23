using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsBase.Common;

namespace CsBase.Class1
{
    public class Class1_2:classBase
    {
        #region codeStart
        public override void RunTest()
        {
            //格式化字符串
            //标记必须递增1排序
            string v1 = "aaa";
            int v2 = 111;
            double v3 = 22.22;
            ddr(string.Format("格式化字符串,参数1{0}，参数2{2},参数3{1}", v1, v2, v3));
            ddr(string.Format("格式化数字字符串|{0}|", 500));
            ddr(string.Format("格式化数字字符串|{0:C}|", 500));      //C格式化为货币
            ddr(string.Format("格式化数字字符串|{0,10}|", 500));     //0索引，10对齐长度
            ddr(string.Format("格式化数字字符串|{0,-10}|", 500));    //左对齐

            double v4 = 12.345678;
            ddr(string.Format("|-10:G | " + "{0,-10:G}", v4));
            ddr(string.Format("|  -10 | " + "{0,  -10}", v4));
            ddr(string.Format("|-10:F4| " + "{0,-10:F4}", v4));
            ddr(string.Format("|-10:C | " + "{0,-10:C}", v4));
            ddr(string.Format("|-10:E3| " + "{0,-10:E3}", v4));
            ddr(string.Format("|-10:x | " + "{0,-10:x}", 31));

            ddr(string.Format("0:C 12.5       | 货币            |" + "{0:C}", 12.5));
            ddr(string.Format("0:D4 12        | 十进制，填充0   |" + "{0:D4}", 12));
            ddr(string.Format("0:F4 12.34567  | 小数点后位数    |" + "{0:F4}", 12.34567));
            ddr(string.Format("0:G4 12.34567  | 保留位数        |" + "{0:G4}", 12.34567));
            ddr(string.Format("0:x 31         | 16进制          |" + "{0:x}", 31));
            ddr(string.Format("0:P3 0.12345   | 百分比          |" + "{0:P3}", 0.12345));
            ddr(string.Format("0:e3 12.34567  | 科学计数法      |" + "{0:e3}", 12.34567));
            ddr(string.Format("0:N3 12345678.54321 | 三个数字一组加逗号|" + "{0:N3}", 12345678.54321));
        }
        #endregion codeEnd
    }
}
