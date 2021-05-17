using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class2
{
    #region codeStart
    public class cla1
    {
        public string first;
        public string second;
        public string last;
        public string this[int index]
        {
            set
            {
                switch (index)
                {
                    case 0:
                        first = value;
                        break;
                    case 1:
                        second = value;
                        break;
                    case 2:
                        last = value;
                        break;
                }
            }
            get
            {
                switch (index)
                {
                    case 0:
                        return first;
                    case 1:
                        return second;
                    case 2:
                        return last;
                    default:
                        return "";
                }
            }
        }
        public string this[string id]  //索引器重载
        {
            get
            {
                switch (id)
                {
                    case "f":
                        return first;
                    case "s":
                        return second;
                    case "l":
                        return last;
                    default:
                        return "";
                }
            }
        }
        public string this[int id, bool flag]  //多索引重载
        {
            get
            {
                string tmp = "";
                switch (id)
                {
                    case 0:
                        tmp= first;
                        break;
                    case 1:
                        tmp= second;
                        break;
                    case 2:
                        tmp= last;
                        break;
                    default:
                        return "";
                }
                if (flag)
                {
                    return tmp;
                }
                return "";
            }
        }
    }

    public class BaseClas
    {
        public virtual int VirtualField { get; set; } = 1;
        
        public virtual int Calculate(int a,int b)
        {
            return VirtualField * (a + b);
        }
    }

    public class ChildClas:BaseClas
    {
        public override int VirtualField
        {
            get => base.VirtualField;
            set => base.VirtualField = value + 1;
        }
        public override int Calculate(int a, int b)
        {
            return VirtualField * (a * b);
        }
    }

    public sealed class person
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string GetPhone()
        {
            return Phone;
        }
    }
    public static class personExtend
    {
        public static string ShowPhone(this person p)
        {
            return p.Phone;
        }
    }

    public interface Icalc
    {
        string calc();
    }

    public class CalcObj : Icalc
    {
        public string calc()
        {
            return "接口继承";
        }
    }
    public static class CalcExten
    {
        public static string calc2(this Icalc cal)
        {
            return "接口扩展";
        }
    }

    #endregion codeEnd
    public class Class2_2:classBase
    {
        #region codeStart
        public override void RunTest()
        {
            cla1 cla = new cla1();
            cla[0] = "first";
            cla[1] = "2222";
            cla[2] = "第三";
            ddr("int index下标 cal[0]  " + cla[0]);
            ddr("int index下标 cal[1]  " + cla[1]);
            ddr("int index下标 cal[2]  " + cla[2]);
            ddr("\n");
            ddr("string index 下标重载索引器");
            ddr("string index下标 cal[f]  " + cla[@"f"]);
            ddr("string index下标 cal[s]  " + cla[@"s"]);
            ddr("string index下标 cal[l]  " + cla[@"l"]);
            ddr("\n");
            ddr("多下标索引器");
            ddr("ind id,bool falg cla[0,true]  " + cla[0,true]);
            ddr("ind id,bool falg cla[1,false] " + cla[1,false]);
            ddr("ind id,bool falg cla[2,true]  " + cla[2,true]);


            ddr("\n");
            ddr("virtual 虚函数、虚方法");
            BaseClas baseClas = new BaseClas();
            baseClas.VirtualField = 2;
            ddr("baseClas.Calculate(2,3)  " + baseClas.Calculate(2,3));
            ChildClas childClas = new ChildClas();
            childClas.VirtualField = 2;
            ddr("childClas.Calculate(2,3)  " + childClas.Calculate(2, 3));


            ddr("\n");
            ddr("扩展方法，静态类的静态方法的this参数");
            person p = new person();
            ddr(p.ShowPhone());


            ddr("\n");
            ddr("扩展方法和继承方法的优先级");
            CalcObj calcObj = new CalcObj();
            ddr(calcObj.calc());
        }
        #endregion codeEnd
    }
}