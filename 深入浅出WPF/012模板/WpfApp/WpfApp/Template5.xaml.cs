using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Template5.xaml 的交互逻辑
    /// </summary>
    public partial class Template5 : UserControl
    {

        ObservableCollection<visUnity> lst1 = new ObservableCollection<visUnity>();
        ObservableCollection<visUnity> lst2 = new ObservableCollection<visUnity>();
        ObservableCollection<BadPointsCheck> badLs = new ObservableCollection<BadPointsCheck>();

        public Template5()
        {
            InitializeComponent();
            lst1.Add(new visUnity(1, "1"));
            lst1.Add(new visUnity(2, "2"));
            lst1.Add(new visUnity(3, "3"));
            lst1.Add(new visUnity(4, "4"));
            lst1.Add(new visUnity(5, "5"));
            lst1.Add(new visUnity(6, "6"));
            lst1.Add(new visUnity(7, "7"));

            lst2.Add(new visUnity(1, "q"));
            lst2.Add(new visUnity(2, "w"));
            lst2.Add(new visUnity(3, "e"));
            lst2.Add(new visUnity(4, "r"));
            lst2.Add(new visUnity(5, "t"));
            lst2.Add(new visUnity(6, "y"));
            lst2.Add(new visUnity(7, "u"));

            ls1.ItemsSource = lst1;

            badLs.Add(new BadPointsCheck(1));
            badLs.Add(new BadPointsCheck(2));
            badLs.Add(new BadPointsCheck(3));
            badLs.Add(new BadPointsCheck(4));


            List<testUnity> ls = new List<testUnity>();
            ls.Add(new testUnity());
            ls.Add(new testUnity());
            ls.Add(new testUnity());
            ls.Add(new testUnity());


            dg_tb1.ItemsSource = badLs;
            dg_tb2.ItemsSource = badLs;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int a = 10;
            visUnity vas= ls1.SelectedItem as visUnity;
            if (vas != null)
            {
                lst1.Remove(vas);
                ls1.ItemsSource = lst1;
            }
        }



        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            badLs[0].Check1 = PointCheck.Bad;
            badLs[1].Check1 = PointCheck.Bad;
            badLs[2].Check1 = PointCheck.Bad;
            badLs[3].Check1 = PointCheck.Bad;

            badLs[0].RepairCount++;
            badLs[1].RepairCount++;
            badLs[2].RepairCount++;
            badLs[3].RepairCount++;
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            badLs[0].Check2 = PointCheck.Bad;
            badLs[1].Check2 = PointCheck.Ok;
            badLs[2].Check2 = PointCheck.Bad;
            badLs[3].Check2 = PointCheck.Ok;

            badLs[0].RepairCount++;
            badLs[2].RepairCount++;
        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            badLs[0].Check3 = PointCheck.Ok;
            badLs[2].Check3 = PointCheck.Ok;

            badLs[0].RepairCount++;
            badLs[2].RepairCount++;
        }

        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            badLs.RemoveAt(3);
        }
    }

    public class visUnity:NotifyPropertyChanged
    {
        private string _str = "";
        public string StrVal
        {
            get
            {
                return _str;
            }
            set
            {
                _str = value;
                OnPropertyChanged("StrVal");
            }
        }
        int Index = -1;
        public visUnity(int id ,string str)
        {
            Index = id;
            StrVal = str;
        }
    }

    public class testUnity: NotifyPropertyChanged
    {
        private int _id = -1;
        private int _checkCount = 0;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        public int RepairCount
        {
            get
            {
                return _checkCount;
            }
            set
            {
                _checkCount = value;
                OnPropertyChanged("RepairCount");
            }
        }
    }


    public class BadPointsCheck : NotifyPropertyChanged
    {
        private int _id = -1;
        private PointCheck _check1 = PointCheck.Uncheck;
        private PointCheck _check2 = PointCheck.Uncheck;
        private PointCheck _check3 = PointCheck.Uncheck;
        private int _checkCount = 0;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        public int RepairCount
        {
            get
            {
                return _checkCount;
            }
            set
            {
                _checkCount = value;
                OnPropertyChanged("RepairCount");
            }
        }

        public PointCheck Check1
        {
            get
            {
                return _check1;
            }
            set
            {
                _check1 = value;
                OnPropertyChanged("Check1");
            }
        }
        public PointCheck Check2
        {
            get
            {
                return _check2;
            }
            set
            {
                _check2 = value;
                OnPropertyChanged("Check2");
            }
        }
        public PointCheck Check3
        {
            get
            {
                return _check3;
            }
            set
            {
                _check3 = value;
                OnPropertyChanged("Check3");
            }
        }

        public BadPointsCheck(int id)
        {
            Check1 = PointCheck.Uncheck;
            Check2 = PointCheck.Uncheck;
            Check3 = PointCheck.Uncheck;
        }
    }
}
