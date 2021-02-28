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

namespace BindingConvertDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Item item = new Item();
        public multItem multItem1 = new multItem();
        public multItem multItem2 = new multItem();
        public MainWindow()
        {
            InitializeComponent();
            item.Color = 1;
            TxtBox.DataContext = item;
            labTest.DataContext = item;
            multItem1.AxisXPos = 123;
            multItem1.AxisYPos = 456;
            multItem1.AxisZPos = 789;
            multItem2.AxisXPos = 1.23;
            multItem2.AxisYPos = 4.56;
            multItem2.AxisZPos = 7.89;
            txtBlock1.DataContext = multItem1;
            txtBlock2.DataContext = multItem2;
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            item.Color++;
            item.Color = item.Color % 5;
        }

        private void BtnSub_Click(object sender, RoutedEventArgs e)
        {
            item.Color--;
            if (item.Color == -1)
            {
                item.Color = 4;
            }
        }
    }
    public class multItem : NotifyPropetryChanged
    {
        private double axisXPos;
        public double AxisXPos
        {
            get
            {
                return axisXPos;
            }
            set
            {
                axisXPos = value;
                OnPropertyChanged(nameof(AxisXPos));
            }
        }

        private double axisYPos;
        public double AxisYPos
        {
            get
            {
                return axisYPos;
            }
            set
            {
                axisYPos = value;
                OnPropertyChanged(nameof(AxisYPos));
            }
        }

        private double axisZPos;
        public double AxisZPos
        {
            get
            {
                return axisZPos;
            }
            set
            {
                axisZPos = value;
                OnPropertyChanged(nameof(AxisZPos));
            }
        }
    }
}
