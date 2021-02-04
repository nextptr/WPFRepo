using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Chart
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.SizeChanged += MainWindow_SizeChanged;
            btn_input.Click += Btn_input_Click;
            btn_reset.Click += Btn_reset_Click;
            btn_roundomBeg.Click += Btn_roundomBeg_Click;
            btn_roundomEnd.Click += Btn_roundomEnd_Click;
        }

        bool flg = false;
        private void Btn_roundomBeg_Click(object sender, RoutedEventArgs e)
        {
            flg = true;
            Task.Factory.StartNew(randomTest);
        }
        private void Btn_roundomEnd_Click(object sender, RoutedEventArgs e)
        {
            flg = false;
        }
        private void randomTest()
        {
            Random rd = new Random(456);
            while (flg)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    chartView.AddData(rd.Next() % 500);
                }));
                Thread.Sleep(250);
            }
        }


        private void Btn_reset_Click(object sender, RoutedEventArgs e)
        {
            chartView.CleanData();
        }

        private void Btn_input_Click(object sender, RoutedEventArgs e)
        {
            chartView.AddData(txt_input.Text);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            chartView.ReSizePanel();
        }
    }
}
