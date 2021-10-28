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

namespace WpfControlsDemo
{
    /// <summary>
    /// StylePanel.xaml 的交互逻辑
    /// </summary>
    public partial class StylePanel : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StylePanel()
        {
            InitializeComponent();
            attBtn.DataContext = this;
            attBtn.Click += AttBtn_Click;
            testBtn.Click += TestBtn_Click;
        }

        private bool propResult;
        public bool PropResult
        {
            get { return propResult; }
            set
            {
                propResult = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PropResult)));
            }
        }

        private void AttBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (testBtn.IsChecked == true)
            {
                PropResult = true;
            }
            else
            {
                PropResult = false;
            }
        }
      
    }
}
