using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("Dll1.dll", EntryPoint = "ADD", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ADD(double a, double b);

        [DllImport("Dll1.dll", EntryPoint = "MIN", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MIN(double a, double b);

        [DllImport("Dll1.dll", EntryPoint = "MUL", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MUL(double a, double b);

        [DllImport("Dll1.dll", EntryPoint = "SUB", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SUB(double a, double b);

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            double lef = double.Parse(box_a.Text);
            double rig = double.Parse(box_b.Text);
            double ret = 0;
            int tag = Convert.ToInt16(btn.Tag);
            switch (tag)
            {
                case 1:
                    ret = ADD(lef, rig);
                    break;
                case 2:
                    ret = MIN(lef, rig);
                    break;
                case 3:
                    ret = MUL(lef, rig);
                    break;
                case 4:
                    ret = SUB(lef, rig);
                    break;
                default:
                    break;
            }
            lab_val.Content = ret;
        }

        public MainWindow()
        {
            InitializeComponent();
            btn_add.Click += Btn_Click;
            btn_min.Click += Btn_Click;
            btn_mul.Click += Btn_Click;
            btn_sub.Click += Btn_Click;
        }
    }
}
