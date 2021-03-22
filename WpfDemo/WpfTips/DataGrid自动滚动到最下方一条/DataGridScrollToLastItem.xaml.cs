using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace WpfTips.DataGridScroll
{
    /// <summary>
    /// DataGridScrollToLastItem.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridScrollToLastItem : UserControl ,INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public DataGridScrollToLastItem()
        {
            InitializeComponent();
            dgt.ItemsSource = Param;

            CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(dgt.Items);
            ((INotifyCollectionChanged)myCollectionView).CollectionChanged += PowerStartUpView_CollectionChanged;
        }
        public static ScrollViewer GetScrollViewer(UIElement element)
        {
            if (element == null) return null;

            ScrollViewer retour = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element) && retour == null; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is ScrollViewer)
                {
                    retour = (ScrollViewer)(VisualTreeHelper.GetChild(element, i));
                }
                else
                {
                    retour = GetScrollViewer(VisualTreeHelper.GetChild(element, i) as UIElement);
                }
            }
            return retour;
        }
        private void PowerStartUpView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ScrollViewer sc = GetScrollViewer(dgt);
            if (sc != null)
            {
                sc.ScrollToEnd();
            }
        }

        public ObservableCollection<PowerStartUpDataItem> Param = new ObservableCollection<PowerStartUpDataItem>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PowerStartUpDataItem item = new PowerStartUpDataItem();
            item.MeasureTime = DateTime.Now.ToString("yyyy_MM_dd:hh_mm_ss");
            item.LaserFrequency = 1;
            item.LaserPowerOut = 2;
            item.LaserPowerRange = 3;
            Param.Add(item);
        }
    }
    public class PowerStartUpDataItem : NotifyPropertyChanged
    {
        private string measureTime;
        public string MeasureTime
        {
            get
            {
                return measureTime;
            }
            set
            {
                measureTime = value;
                OnPropertyChanged(nameof(MeasureTime));
            }
        }

        private double laserFrequency;
        public double LaserFrequency
        {
            get
            {
                return laserFrequency;
            }
            set
            {
                laserFrequency = value;
                OnPropertyChanged(nameof(LaserFrequency));
            }
        }

        private double laserPowerOut;
        public double LaserPowerOut
        {
            get
            {
                return laserPowerOut;
            }
            set
            {
                laserPowerOut = value;
                OnPropertyChanged(nameof(LaserPowerOut));
            }
        }

        private double laserPowerRange;
        public double LaserPowerRange
        {
            get
            {
                return laserPowerRange;
            }
            set
            {
                laserPowerRange = value;
                OnPropertyChanged(nameof(LaserPowerRange));
            }
        }

        public PowerStartUpDataItem()
        {
        }

        public PowerStartUpDataItem clone()
        {
            PowerStartUpDataItem clon = new PowerStartUpDataItem();
            clon.MeasureTime = this.MeasureTime;
            clon.LaserFrequency = this.LaserFrequency;
            clon.LaserPowerOut = this.LaserPowerOut;
            clon.LaserPowerRange = this.LaserPowerRange;
            return clon;
        }
    }

}
