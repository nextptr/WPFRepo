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

namespace WpfApp
{
    /// <summary>
    /// UserDefineCommandPanel.xaml 的交互逻辑
    /// </summary>
    public partial class UserDefineCommandPanel : UserControl,IView
    {
        public UserDefineCommandPanel()
        {
            InitializeComponent();
        }

        public bool IsChanged { get; set; }

        public void Clear()
        {
            this.textBox1.Clear();
            this.textBox2.Clear();
            this.textBox3.Clear();
            this.textBox4.Clear();
        }

        public void Refresh()
        {
        }

        public void Save()
        {
        }

        public void SetBinding()
        {
        }
    }
}
