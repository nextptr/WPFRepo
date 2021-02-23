using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class4
{
    public class Class4_5:classBase
    {
        public class Student
        {
            public int StId;
            public string LastName;
        }
        public class CourseStudent
        {
            public int StId;
            public string CourseName;
        }

        static Student[] students = new Student[]
            {
                new Student{ StId=1,LastName="Carson"},
                new Student{ StId=2,LastName="Klassen"},
                new Student{ StId=3,LastName="Fleming"},
            };

        static CourseStudent[] studentsInCourse = new CourseStudent[]
            {
                new CourseStudent{ CourseName="Art",StId=1},
                new CourseStudent{ CourseName="Art",StId=2},
                new CourseStudent{ CourseName="History",StId=1},
                new CourseStudent{ CourseName="History",StId=3},
                new CourseStudent{ CourseName="Physics",StId=3},
            };

        int[] numbers = { 2, 5, 28, 31, 17, 16, 42 };

        #region codeStart
        public override void RunTest()
        {
            //匿名对象
            ddh("完全的匿名对象");
            var Student=  new{ name="jack",Age=19,Addrss="china"};
            ddr("var Student=  new{ name=\"jack\",Age=19,Addrss=\"china\"};");
            ddr($"Student.name {Student.name}\nStudent.Age {Student.Age}\nStudent.Add {Student.Addrss}");
            //linq查询
            var numsQuery = from n in numbers            //查询语法,type n,type是可选的，形式和foreach类似
                            where n < 20
                            select n;

            var numsMethod = numbers.Where(x => x < 20); //方法语法

            int numsCount = (from n in numbers           //混合语法
                             where n < 20
                             select n).Count();
            ddh("原始数据");
            string str = "";
            foreach (var x in numbers)
            {
                str += x+" ";
            }
            ddr(str);
            ddr("sql查询语法 x<20");
            str = "";
            foreach (var x in numsQuery)
            {
                str += x + " ";
            }
            ddr(str);
            ddr("命令式语法 x<20");
            str = "";
            foreach (var x in numsMethod)
            {
                str += x + " ";
            }
            ddr(str);
            ddr($"混合式  x<20 {numsCount}");
            //join子句
            ddh("join子句");
            var query = from stu in students
                        join cou in studentsInCourse on stu.StId equals cou.StId
                        where cou.CourseName == "History"
                        select stu.LastName;
            foreach (var q in query)
            {
                ddr($"choise History: {q}");
            }
            //from let where 子句
            ddh("from子句");
            var groupA = new[] { 3, 4, 5, 6 };
            var groupB = new[] { 6, 7, 8, 9 };
            var someInts = from a in groupA
                           from b in groupB
                           where a > 4 && b <= 8
                           select new { a, b, sum = a + b };
            foreach (var q in someInts)
            {
                ddr($"{q}");
            }
            //let子句
            ddh("let子句");
            var someInts2 = from a in groupA
                            from b in groupB
                            let sum = a + b
                            where sum == 12
                            select new { a, b, sum };
            foreach (var q in someInts2)
            {
                ddr($"{q}");
            }
            //where子句
            ddh("where子句");
            var someInts3 = from int a in groupA
                            from int b in groupB
                            let sum = a + b
                            where sum >= 10 && sum <= 100
                            where a == 4
                            select new { a, b, sum };
            foreach (var q in someInts3)
            {
                ddr($"{q}");
            }
            //orderby子句
            ddh("orderby子句");
            var colleges = new[]
            {
                new { Lname="Jones",  FName="Mary",  Age=19,Major="History"},
                new { Lname="Smith",  FName="Bob",   Age=20,Major="CompSci"},
                new { Lname="Fleming",FName="Carol", Age=21,Major="History"},
            };
            var ls1 = from tmp in colleges
                     orderby tmp.Age
                     select tmp;
            var ls2 = from tmp in colleges
                     orderby tmp.Age descending
                     select tmp;
            ddh("orderby 默认升序");
            foreach (var q in ls1)
            {
                ddr($"{q}");
            }
            ddh("orderby 逆序");
            foreach (var q in ls2)
            {
                ddr($"{q}");
            }
            //select子句
            ddh("select子句");
            var ls3 = from s in colleges
                      select s.Lname;
            foreach (var q in ls3)
            {
                ddr($"{q}");
            }
            //group子句
            ddh("group子句");
            var ls4 = from tmp in colleges
                      group tmp by tmp.Major;
            foreach (var stu in ls4)
            {
                ddr($"{stu.Key}");
                foreach (var tmp in stu)
                {
                    ddr($"   {tmp.Lname}-{tmp.FName}");
                }
            }
            //into子句 延续查询
            ddh("into子句");
            var ls5 = from a in groupA
                      join b in groupB on a equals b
                      into groupAandB
                      from c in groupAandB
                      select c;
            str = "";
            foreach (var tmp in ls5)
            {
                str += " " + tmp;
            }
            ddr($"{str}");
        }
        #endregion codeEnd
    }
}
