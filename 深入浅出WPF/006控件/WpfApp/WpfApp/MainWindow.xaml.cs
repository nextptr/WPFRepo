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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btn_1.Click += Btn_Click;
            btn_2.Click += Btn_Click;
            btn_3.Click += Btn_Click;

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //List<string> ls = new List<string>();
            //ls.Add("st1");
            //ls.Add("st2");
            //ls.Add("st3");
            //ls.Add("st4");
            //ls.Add("st5");
            //ls.Add("st6");
            //ls.Add("st7");
            //ls_box.ItemsSource = ls;

            //grid使用代码添加行列布局
            grid_main.RowDefinitions.Add(new RowDefinition());  //*

            RowDefinition row = new RowDefinition();            //Auto
            row.Height = GridLength.Auto;
            grid_main.RowDefinitions.Add(row);

            RowDefinition row2 = new RowDefinition();           //60
            row2.Height = new GridLength(60);
            grid_main.RowDefinitions.Add(row2);

            TextBlock tx1 = new TextBlock();
            TextBlock tx2 = new TextBlock();
            TextBlock tx3 = new TextBlock();
            tx1.Text = "123";
            tx2.Text = "456";
            tx3.Text = "789";
            grid_main.Children.Add(tx1);
            grid_main.Children.Add(tx2);
            grid_main.Children.Add(tx3);
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            DependencyObject level = VisualTreeHelper.GetParent(btn);
            for (int i = 1; level != null; i++)
            {
                showMsg($"level{i}"+ level.GetType().ToString());
                level = VisualTreeHelper.GetParent(level);
            }
        }

        private void showMsg(object obj)
        {
            ls_box.Items.Add(obj.ToString());
        }
    }
}
