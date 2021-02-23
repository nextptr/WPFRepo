using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class4
{
    #region codeStart
    public interface iInfo
    {
        string GetName();
        string GetAge();
    }
    public class Ca:iInfo
    {

        public string Name;
        public int Age;
        public string GetAge()
        {
            return Age.ToString();
        }
        public string GetName()
        {
            return Name;
        }
    }

    public class Cb:iInfo
    {
        public string First;
        public string Last;
        public double PersonsAge;
        public string GetAge()
        {
            return PersonsAge.ToString();
        }
        public string GetName()
        {
            return First + " " + Last;
        }
        //自定义类型转换
        public static implicit operator int(Cb x)//implicit 隐式转换
        {
            return (int)(x.PersonsAge);
        }
        public static explicit operator bool(Cb e)//explicit 显式转换
        {
            return e.PersonsAge > 18 ? true : false;
        }
    }
    #endregion codeEnd
    public class Class4_2:classBase
    {
        #region codeStart
        public override void RunTest()
        {
            Ca a = new Ca() { Name = "xql", Age = 25 };
            Cb b = new Cb() { First = "Jane", Last = "Doe", PersonsAge = 23.5 };
            print(a);
            print(b);

            Cb cb = new Cb() { First = "Jim", Last = "morty", PersonsAge = 22.6 };
            print(cb);
            int intAge = cb;
            bool isAdult = (bool)cb;
            ddr($"Jim int intAge = Cb::cb = {intAge}");
            ddr($"Jim bool isAdule = (bool)cb= {isAdult}");

        }

        public void print(iInfo ifo)  //以接口作为函数参数
        {
            ddr($"Name:{ifo.GetName()},Age {ifo.GetAge()}");
        }

        #endregion codeEnd
    }
}
