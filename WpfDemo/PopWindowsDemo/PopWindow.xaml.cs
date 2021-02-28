using System.Windows;
using System.Windows.Input;

namespace PopWindowsDemo
{
    /// <summary>
    /// PopWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PopWindow : Window
    {
        public PopWindow()
        {
            InitializeComponent();
            this.Topmost = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            this.TitleBar.MouseDown += TitleBar_MouseDown;
            this.BtnClose.Click += BtnClose_Click;
            this.BtnMin.Click += BtnMin_Click;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            //this.Close();
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
