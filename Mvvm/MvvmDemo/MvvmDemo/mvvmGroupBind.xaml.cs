using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// mvvmGroupBind.xaml 的交互逻辑
    /// </summary>
    /// 

    public class Employe
    {
        public ObservableCollection<string> Employees { get; set; }

        public Employe()
        {
            Employees = new ObservableCollection<string>()
        {
            "肥猫", "大牛", "猪头"
        };
        }
    }
    public partial class mvvmGroupBind : UserControl
    {
        Employe emp = new Employe();
        public mvvmGroupBind()
        {
            InitializeComponent();
            btn_add.Click += Btn_add_Click;
            lsv_cont.DataContext = emp;
            com_cont.DataContext = emp;
        }

        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {
            emp.Employees.Add(txt_arg.Text);
        }
    }
}
