using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvvmDemo
{
    /// <summary>
    /// mvvmCommandBind.xaml 的交互逻辑
    /// </summary>
    /// 
    public class AddEmployeeCommand : ICommand
    {
        Action<object> _Execute;
        public AddEmployeeCommand(Action<object> execute)
        {
            _Execute = execute;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _Execute(parameter);
        }
    }

    public class ComEmploye : INotifyPropertyChanged
    {
        //界面显示，用于获取界面数据
        private string _newEmployee;
        public event PropertyChangedEventHandler PropertyChanged;
        public string NewEmployee
        {
            get
            {
                return this._newEmployee;
            }
            set
            {
                if (this._newEmployee != value)
                {
                    this._newEmployee = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("NewEmployee"));
                    }
                }
            }
        }

        //命令，调用命令执行动作
        public ObservableCollection<string> Employees { get; set; }
        private ICommand _AddEmployee;
        public ICommand AddEmployee
        {
            get
            {
                if (_AddEmployee == null)
                {
                    _AddEmployee = new AddEmployeeCommand((p) =>
                    {
                        Employees.Add(NewEmployee);
                    });
                }
                return _AddEmployee;
            }
        }

        public ComEmploye()
        {
            Employees = new ObservableCollection<string>()
        {
            "肥猫", "大牛", "猪头"
        };
        }
    }

    public partial class mvvmCommandBind : UserControl
    {
        ComEmploye emp = new ComEmploye();
        public mvvmCommandBind()
        {
            InitializeComponent();
            lsv_cont.DataContext = emp;
            com_cont.DataContext = emp;
            txt_arg.DataContext = emp;
            btn_add.DataContext = emp;
        }
    }
}
