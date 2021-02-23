using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class2
{
    public class Class2_1:classBase
    {
        #region codeStart
        public override void RunTest()
        {
            //函数参数
            {
                //参数数组
                func(1, 3, 5, 7, 9);
                int[] arr = { 2, 4, 6, 8, 10 };
                func(arr);
                //命名参数
                func(last: 5, first: 2, second: 9);
                func(last: 3, second: 2, first: 8);
                //默认参数
                func(1);
                func(1,2);
                func(1,b:3);//命名参数

            }
        }

        private void func(params int[] arr)
        {
            ddr("数组参数");
            foreach (int tmp in arr)
            {
                ddr(" " + tmp);
            }
        }
        private void func(int first, int second, int last)
        {
            ddr($"命名参数: {first}+{second}-{last}={first+second-last}");
        }
        private void func(int a, int b = 1)
        {
            ddr($"可选参数:a+b={a + b}");
        }

        #endregion codeEnd
    }
}
