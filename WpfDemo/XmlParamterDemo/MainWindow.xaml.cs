using Common.Parameter;
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

namespace XmlParamterDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestParam param = new TestParam();
        public MainWindow()
        {
            InitializeComponent();
            btn_add.Click += Btn_add_Click;
            btn_read.Click += Btn_read_Click;
            btn_write.Click += Btn_write_Click;
            ls_bx.ItemsSource = param.Datas;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            param.Read();
        }

        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {
            param.Datas.Add(txt_box.Text);
        }
        private void Btn_read_Click(object sender, RoutedEventArgs e)
        {
            param.Read();
        }
        private void Btn_write_Click(object sender, RoutedEventArgs e)
        {
            param.Write();
        }
    }

    public class TestParam : ParameterBase
    {
        private ObservableCollection<string> datas;
        public ObservableCollection<string> Datas
        {
            get
            {
                return datas;
            }
            set
            {
                datas = value;
                OnPropertyChanged(nameof(Datas));
            }
        }

        public TestParam()
        {
            Datas = new ObservableCollection<string>();
        }

        public override void Copy(IParameter source)
        {
            TestParam sp = source as TestParam;
            if (sp != null) 
            {
                this.Datas.Clear();
                foreach (var item in sp.Datas)
                {
                    this.Datas.Add(item);
                }
            }
        }
    }

}
