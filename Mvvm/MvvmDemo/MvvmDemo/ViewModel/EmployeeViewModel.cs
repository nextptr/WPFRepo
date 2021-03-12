using MvvmDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MvvmDemo.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Employee> Employees { get; set; }//ObservableCollection可以直接用于界面显示

        ///可绑定数据，只是用于从界面获取数据
        public event PropertyChangedEventHandler PropertyChanged;
        private string _newEmployeeName;
        public string NewEmployeeName
        {
            get
            {
                return this._newEmployeeName;
            }
            set
            {
                if (this._newEmployeeName != value)
                {
                    this._newEmployeeName = value;
                    if (this.PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("NewEmployeeName"));
                    }
                }
            }
        }

        private string _newEmployeeEmail;
        public string NewEmployeeEmail
        {
            get
            {
                return this._newEmployeeEmail;
            }
            set
            {
                if (this._newEmployeeEmail != value)
                {
                    this._newEmployeeEmail = value;
                    if (this.PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("NewEmployeeEmail"));
                    }
                }
            }
        }

        private string _newEmployeePhone;
        public string NewEmployeePhone
        {
            get
            {
                return this._newEmployeePhone;
            }
            set
            {
                if (this._newEmployeePhone != value)
                {
                    this._newEmployeePhone = value;
                    if (this.PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("NewEmployeePhone"));
                    }
                }
            }
        }


        ///可绑定命令
        public ICommand AddEmployee
        {
            get
            {
                return new RelayCommand(new Action(() =>
                {
                    if (string.IsNullOrEmpty(NewEmployeeName))
                    {
                        MessageBox.Show("姓名不能为空!");
                        return;
                    }
                    var newEmployee = new Employee
                    {
                        Name = _newEmployeeName,
                        Email = NewEmployeeEmail,
                        Phone = NewEmployeePhone
                    };
                    newEmployee.Add(); //添加到database
                    Employees.Add(newEmployee);//添加到可现实队列
                }));
            }
        }


        public EmployeeViewModel()
        {
            Employees = new ObservableCollection<Employee>(DataBase.AllEmployees);
        }
    }
}
