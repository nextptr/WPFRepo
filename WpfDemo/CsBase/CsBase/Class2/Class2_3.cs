using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class2
{
    #region codeStart
    public class claBase:classBase
    {
        public string val1 = "baseval1";
        public string val2 = "baseval2";
        public void Print()
        {
            ddr("cla.Print() "+ val1 + " " + val2);
            ddr(" ");
        }
        public void SamePrint()
        {
            ddr("cla.Print() " + val1 + " " + val2);
            ddr(" ");
        }
        public virtual void virPrint()
        {
            ddr("cla.virPrint() " + val1 + " " + val2);
        }
    }
    public class claDerivedLv1:claBase
    {
        //使用new 关键字显示屏蔽基类的字段和函数
        new public string val1 = "new val";
        new public void SamePrint()
        {   //val1使用的是派生类，val2使用的是基类
            ddr("claDerivedLv1.SamePrint() " + val1 + " " + val2);
            ddr(" ");
        }
        //使用overrid关键字覆写基类函数
        public override void virPrint()
        {
            ddr("override 覆写基类函数");
            ddr(" ");
        }
    }


    public class MyData
    {
        private double D1;
        private double D2;
        private double D3;

        public MyData(double d1, double d2, double d3)
        {
            D1 = d1;
            D2 = d2;
            D3 = d3;
        }
        public double sum()
        {
            return D1 + D2 + D3;
        }
    }
    internal static class ExtendMyData
    {
        public static double Average(this MyData md)
        {
            return md.sum() / 3;
        }
    }

    #endregion codeEnd

    public class Class2_3:classBase
    {
        #region codeStart
        public claBase claBase1 = new claBase();
        public claBase claBase2 = new claBase();
        public claDerivedLv1 claDerived1 = new claDerivedLv1();
        public MyData md = new MyData(4, 5, 6);

        public override void RunTest()
        {
            ddr("基类函数");
            claBase1.Print();
            ddr("派生类函数");
            claDerived1.Print();
            ddr("派生类屏蔽基类函数和字段");
            claDerived1.SamePrint();
            claBase2 = (claBase)claDerived1;
            ddr("派生类override基类虚函数");
            claBase2.virPrint();

            ddr("类的扩展函数");
            ddr($"本类函数sun(a,b,c)={md.sum()}");
            ExtendMyData.Average(md);
            ddr($"本类扩展函数average(a,b,c)={md.Average()}");
        }
        #endregion codeEnd

        public Class2_3()
        {
            claBase1.ddMsgEvent += ShowMsg;
            claBase2.ddMsgEvent += ShowMsg;
            claDerived1.ddMsgEvent += ShowMsg;
        }
        public void ShowMsg(string str)
        {
            this.ddr(str);
        }

    }
}
