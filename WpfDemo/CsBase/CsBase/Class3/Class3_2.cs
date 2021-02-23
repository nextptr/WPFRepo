using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class3
{
    class Class3_2:classBase
    {
        #region codeStart
        [Flags]//使用同一个uint变量中的不同位
        enum Direction:uint
        {
            Up = 0x01,
            Down = 0x02,
            Left = 0x03,
            Right = 0x04
        }
        public override void RunTest()
        {
            ///枚举
            Direction dir = Direction.Down | Direction.Left | Direction.Right;
            if (dir.HasFlag(Direction.Left))
            {
                ddr("位标志枚举类型dir包含Left位");
            }
            if (dir.HasFlag(Direction.Right))
            {
                ddr("位标志枚举类型dir包含Right位");
            }
            foreach (var nam in Enum.GetNames(typeof(Direction)))
            {
                ddr($"枚举成员名称：{nam}");
            }

            ///数组
            int[] arr1 = new int[3];
            int[] arr2 = new int[] { 1, 2, 3 };
            ddr("int[] arr1 = new int[3];");
            foreach (int d in arr1)
            {
                ddr($"{d}");
            }
            ddr("int[] arr2 = new int[] { 1, 2, 3 };");
            foreach (int d in arr2)
            {
                ddr($"{d}");
            }
            //多维数组arr3==arr4
            int[,] arr3 = { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,] arr4 = new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } };
        }
        #endregion codeEnd
    }
}
