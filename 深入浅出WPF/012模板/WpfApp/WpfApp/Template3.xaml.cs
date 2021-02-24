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
    /// Template3.xaml 的交互逻辑
    /// </summary>
    public partial class Template3 : UserControl
    {
        public Template3()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            List<Car> carList = new List<Car>()
            {
                new Car(){Automaker="lamborghini",Name="Diablo"    ,Year="1990",TopSpeed="340" },
                new Car(){Automaker="lamborghini",Name="Murcielago",Year="2001",TopSpeed="353" },
                new Car(){Automaker="lamborghini",Name="Gallardo"  ,Year="2003",TopSpeed="325" },
                new Car(){Automaker="lamborghini",Name="RevenTon"  ,Year="2008",TopSpeed="356" }
            };
            listBoxCars.ItemsSource = carList;
        }
    }
}
