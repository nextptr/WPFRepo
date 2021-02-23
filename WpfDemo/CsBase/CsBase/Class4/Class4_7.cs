using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class4
{

    #region codeStart

    //基类
    public class human
    {
        private string address;
        private int age;
        public bool sexuality;  

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string sleep()
        {
            return "human sleep";
        }
        public string eat()
        {
            return "human eat";
        }
    }

    //派生类
    public class student : human
    {
        public int score;

        private string major;
        public string Major
        {
            get { return major; }
            set { major = value; }
        }

        private int grade;
        public int Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        private int password;

        public string learn()
        {
            return "student learn";
        }
        private string truancy()
        {
            return "student truancy";
        }
    }
    
    //派生类
    public class employee : human
    {
        public int experience;
        private int salary;


        private string company;
        public string Company
        {
            get { return company; }
            set { company = value; }
        }


        public string work()
        {
            return "employee work";
        }
    }

    class Class4_7 : classBase
    {
        public override void RunTest()
        {
            human hum = new human();
            student stu = new student();
            employee emp = new employee();
            Type tp_hum = hum.GetType();
            Type tp_stu = stu.GetType();
            Type tp_emp = emp.GetType();

            Show(tp_hum);
            Show(tp_stu);
            Show(tp_emp);

            Type tp = tp_hum.GetType();
            Show(tp);
        }

        public void Show(Type t)
        {
            ddh("Name 名称");
            ddr(t.Name);

            ddh("NameSpace 命名空间");
            ddr(t.Namespace);

            ddh("Assembly 程序集");
            ddr(t.Assembly.ToString());

            ddh("GetField 字段");
            FieldInfo[] fi = t.GetFields();
            foreach (var f in fi)
            {
                ddr(f.Name);
            }

            ddh("GetProperties 属性");
            PropertyInfo[] pi = t.GetProperties();
            foreach (var f in pi)
            {
                ddr(f.Name);
            }

            ddh("GetMethods 成员函数");
            MethodInfo[] mi = t.GetMethods();
            foreach (var f in mi)
            {
                ddr(f.Name);
            }
        }
    }
    #endregion codeEnd

}
