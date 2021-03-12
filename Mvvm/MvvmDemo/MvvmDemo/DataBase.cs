using MvvmDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmDemo
{
    public class DataBase
    {
        private static List<Employee> _Employee = new List<Employee>
        {
            new Employee{ Name="肥猫"},
            new Employee{ Name="大牛"},
            new Employee{ Name="猪头"},
        };

        public static List<Employee> AllEmployees
        {
            get
            {
                return _Employee;
            }
            set
            {
                _Employee = value;
            }
        }
    }
}