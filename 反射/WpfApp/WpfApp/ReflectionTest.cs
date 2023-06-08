using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{

    public class Student
    {
        public string Name;
        public int Age;
    }

    public class ReflectionHelper
    {
        public static void SetValue(object obj, string fieldName, object value)
        {
            FieldInfo info = obj.GetType().GetField(fieldName);
            info.SetValue(obj, value);
        }
        public static void SetValue<T>(object obj, string fieldName, T value)
        {
            FieldInfo info = obj.GetType().GetField(fieldName);
            info.SetValue(obj, value);
        }
        public static object GetValue(object obj, string fieldName)
        {
            FieldInfo info = obj.GetType().GetField(fieldName);
            return info.GetValue(obj);
        }
        public static T GetValue<T>(object obj, string fieldName)
        {
            FieldInfo info = obj.GetType().GetField(fieldName);
            return (T)info.GetValue(obj);
        }
    }

    public class ReflectionTest
    {
        public Action<string> msg;

        public void RefTest()
        {
            Student stu = new Student();
            stu.Name = "张三";
            stu.Age = 30;
            msg?.Invoke($"Name:{stu.Name} Arg:{stu.Age}");

            ReflectionHelper.SetValue(stu, "Name", "李四");
            msg?.Invoke($"Name:{stu.Name} Arg:{stu.Age}");

            ReflectionHelper.SetValue<int>(stu, "Age", 40);
            msg?.Invoke($"Name:{stu.Name} Arg:{stu.Age}");

        }

    }
}
