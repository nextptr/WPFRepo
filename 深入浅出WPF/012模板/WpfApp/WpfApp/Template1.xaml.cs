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
    /// Template1.xaml 的交互逻辑
    /// </summary>
    public partial class Template1 : UserControl
    {
        public Template1()
        {
            InitializeComponent();

            List<ViewUnity> ls = new List<ViewUnity>();
            ls.Add(new ViewUnity(1990,100));
            ls.Add(new ViewUnity(1991,130));
            ls.Add(new ViewUnity(1992,150));
            ls.Add(new ViewUnity(1993,160));
            ls.Add(new ViewUnity(1994,140));
            ls.Add(new ViewUnity(1995,120));
            ls.Add(new ViewUnity(1996,150));
            ls.Add(new ViewUnity(1997,180));
            ls.Add(new ViewUnity(1998,210));

            ls_box.ItemsSource = ls;
            contrl.ItemsSource = ls;
        }
    }



    public class ViewUnity: NotifyPropertyChanged
    {
        public ViewUnity()
        { }
        public ViewUnity(int year, int price)
        {
            Year = year;
            Price = price;
        }


        private int price = 0;
        public int Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }

        }

        private int year = 0;
        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }
    }
}
