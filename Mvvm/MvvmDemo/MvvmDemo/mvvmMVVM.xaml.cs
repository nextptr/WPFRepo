using MvvmDemo.ViewModel;
using System;
using System.Collections.Generic;
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
    /// mvvmMVVM.xaml 的交互逻辑
    /// </summary>
    public partial class mvvmMVVM : UserControl
    {
        EmployeeViewModel emp = new EmployeeViewModel();
        public mvvmMVVM()
        {
            InitializeComponent();
            txt_ema.DataContext = emp;
            txt_nam.DataContext = emp;
            txt_pho.DataContext = emp;
            btn_add.DataContext = emp;
            cmb_ls.DataContext = emp;
        }
    }
}
