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
            Operation ope = new Operation();

            ope= OperationFactory.CreateInstance("+");
            ope.Number1 = 100;
            ope.Number2 = 25;
            txt_box1.Text = ope.GetResult().ToString();

            ope = OperationFactory.CreateInstance("-");
            ope.Number1 = 100;
            ope.Number2 = 25;
            txt_box2.Text = ope.GetResult().ToString();

            ope = OperationFactory.CreateInstance("*");
            ope.Number1 = 100;
            ope.Number2 = 25;
            txt_box3.Text = ope.GetResult().ToString();

            ope = OperationFactory.CreateInstance("/");
            ope.Number1 = 100;
            ope.Number2 = 25;
            txt_box4.Text = ope.GetResult().ToString();

            txt_box5.Text += motionEnum.aoi + "|";
            txt_box5.Text += Enum.Parse(typeof(motionEnum), "load")+"|";
            txt_box5.Text += (motionEnum)Enum.Parse(typeof(motionEnum), "load");
        }
    }


    public enum motionEnum
    {
        none=0,
        load,
        light,
        aoi,
        mid
    }
}
