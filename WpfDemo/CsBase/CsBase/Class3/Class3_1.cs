using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class3
{
    public class Class3_1:classBase
    {
        #region codeStart
        public override void RunTest()
        {
            //转义字符
            char c1 = 'd';
            char c2 = '\u2764';
            char c3 = '\x0061';
            char c4 = '\u2605';
            ddr($"{c1}");
            ddr($"{c2}");
            ddr($"{c3}");
            ddr($"{c4}");
            ddr(@"@不解释\r\n\t制表符");

            //Type运算符
            Type t = typeof(Class3_1);
            FieldInfo[] fi = t.GetFields();
            MethodInfo[] mi = t.GetMethods();
            foreach (FieldInfo f in fi)
            {
                ddr($"field:{f.Name}");
            }
            foreach (MethodInfo m in mi)
            {
                ddr($"method:{m.Name}");
            }

            //语句
            int x = 10;        //声明语句
            int y = 7;         //声明语句
            int z;             //声明语句
            {                  //块
                z = x + y;     //嵌入语句
                tag: y = 23;   //标签语句
                {              //嵌套块
                    //...
                }
            }

            switch (x)
            {
                case 0:
                    y += 1;
                    break;
                case 1:
                    y += 2;
                    break;
                case 2:
                    goto default;
                case 3:
                    y += 4;
                    break;
                case 4:
                    goto case 0; //内部跳转
                default:
                    goto flag; //跳转到标签
            }
            //标签
            flag: ddr("标签flag: ddr(\"标签\");");

            //using 语句
            /*
            using (type source=new type())
            {
                do();
            }
            等价于
            func()
            {
                type source =new type(); //分配资源
                try
                {
                     do();               //使用资源
                }
                finally
                {    //type必须实现了IDisposable接口
                     source.Dispose;     //清理资源
                }
            }
            */

        }
        #endregion codeEnd
    }
}
