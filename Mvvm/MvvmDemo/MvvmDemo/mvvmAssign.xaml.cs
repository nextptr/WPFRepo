using System;
using System.Collections.Generic;
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
    /// mvvmAssign.xaml 的交互逻辑
    /// </summary>
    /// 

    public class MyClass : INotifyPropertyChanged
    {
        public MyClass()
        {
            this._Time = DateTime.Now.ToString();
        }

        private string _Time;
        public string Time
        {
            get
            {
                return this._Time;
            }
            set
            {
                if (this._Time != value)
                {
                    this._Time = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Time"));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public partial class mvvmAssign : UserControl
    {
        MyClass cla = new MyClass();
        public mvvmAssign()
        {
            InitializeComponent();
            btn_flash_assign.Click += Btn_flash_assign_Click;
            btn_flash_bind.Click += Btn_flash_bind_Click;
        }

        private void Btn_flash_assign_Click(object sender, RoutedEventArgs e)
        {
            cla.Time = DateTime.Now.ToString();
            lab_assign.Content = cla.Time;
        }

        private void Btn_flash_bind_Click(object sender, RoutedEventArgs e)
        {
            cla.Time = DateTime.Now.ToString();
            lab_bind.DataContext = null;
            lab_bind.DataContext = cla;
        }
    }
}
