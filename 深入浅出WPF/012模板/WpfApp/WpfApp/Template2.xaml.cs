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
    /// Template2.xaml 的交互逻辑
    /// </summary>
    public partial class Template2 : UserControl
    {
        public Template2()
        {
            InitializeComponent();
            initCarList();
        }

        private void initCarList()
        {
            List<Car> carList = new List<Car>()
            {
                new Car(){Automaker="lamborghini",Name="Diablo"    ,Year="1990",TopSpeed="340" },
                new Car(){Automaker="lamborghini",Name="Murcielago",Year="2001",TopSpeed="353" },
                new Car(){Automaker="lamborghini",Name="Gallardo"  ,Year="2003",TopSpeed="325" },
                new Car(){Automaker="lamborghini",Name="RevenTon"  ,Year="2008",TopSpeed="356" }
            };

            foreach (Car car in carList)
            {
                CarListItemView view = new CarListItemView();
                view.Car = car;
                ls_box.Items.Add(view);
            }
        }

        private void Ls_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CarListItemView view = e.AddedItems[0] as CarListItemView;

            if (view != null)
            {
                this.detailView.Car = view.Car;
            }
        }
    }



}
