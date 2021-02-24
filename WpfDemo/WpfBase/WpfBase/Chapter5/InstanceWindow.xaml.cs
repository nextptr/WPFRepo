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
using System.Windows.Shapes;

namespace WpfBase.Chapter5
{
    /// <summary>
    /// InstanceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InstanceWindow : Window
    {
        private static InstanceWindow _instance = null;
        public static InstanceWindow Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InstanceWindow();
                }
                return _instance;
            }
        }

        private InstanceWindow()
        {
            InitializeComponent();
            TitleBar.MouseMove += TitleBar_MouseMove;
            btn_Min.Click += Btn_Min_Click;
            btn_Close.Click += Btn_Close_Click;
        }
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Btn_Min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            _instance = null;
            this.Close();
        }
    }
}
