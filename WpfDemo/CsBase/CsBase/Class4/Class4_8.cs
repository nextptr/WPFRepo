using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class4
{
    public class Class4_8 : classBase
    {
        public override void RunTest()
        {
            Person pe = new Person("jhon",25,"10086");
            ddh("封闭类的扩展");
            ddr(pe.GetPersonInfor());

            ddh("接口扩展");
            CalculateObject obj = new CalculateObject();
            ddr($"a+b:{ obj.Add(6, 3)}");
            ddr($"a-b:{ obj.Sub(6, 3)}");
            ddr($"a*b:{ obj.Maltiply(6, 3)}");
            ddr($"a/b:{ obj.Division(6, 3)}");
        }
    }

    //封闭类的扩展,封闭类无法被继承
    public sealed class Person //密封类
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public Person()
        {

        }
        public Person(string name,int age,string phone)
        {
            Name = name;
            Age = age;
            Phone = phone;
        }
    }
    //在扩展方法中可以使用访问静态类中的字段
    public static class PersonExtend
    {
        public static string GetPersonInfor(this Person person)
        {
            return person.Name + " " + person.Age + " " + person.Phone;
        }
    }

    
    public interface ICalculate
    {
        int Add(int a, int b);
    }
    public static class CalculateExtend
    {
        public static int Sub(this ICalculate calculate, int a, int b)
        {
            return a - b;
        }
        public static int Maltiply(this ICalculate calculate, int a, int b)
        {
            return a * b;
        }
        public static int Division(this ICalculate calculate, int a, int b)
        {
            return a / b;
        }
    }

    public class CalculateObject : ICalculate
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }


}
