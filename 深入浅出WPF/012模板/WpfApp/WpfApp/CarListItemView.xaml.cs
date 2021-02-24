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
    /// CarListItemView.xaml 的交互逻辑
    /// </summary>
    public partial class CarListItemView : UserControl
    {
        private Car car;

        public Car Car
        {
            get
            {
                return car;
            }
            set
            {
                car = value;
                this.txt_name.Text = car.Name;
                this.txt_year.Text = car.Year;
                string uriStr = string.Format(@"Resources/Logos/{0}.png", car.Automaker);
                this.image_logo.Source = new BitmapImage(new Uri(uriStr, UriKind.Relative));
            }
        }

        public CarListItemView()
        {
            InitializeComponent();
        }
    }
}
