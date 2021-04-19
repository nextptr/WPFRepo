using PowerMeterDevice.Parameter;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PowerMeterDevice
{
    /// <summary>
    /// PowerAdjustView.xaml 的交互逻辑
    /// </summary>
    public partial class PowerAdjustView : UserControl
    {
        public PowerAdjustView()
        {
            InitializeComponent();
            CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(dgt.Items);
            ((INotifyCollectionChanged)myCollectionView).CollectionChanged += PowerStartUpView_CollectionChanged;
            this.Loaded += PowerAdjustView_Loaded;
        }

        private void PowerAdjustView_Loaded(object sender, RoutedEventArgs e)
        {
            ParameterManager.Instance.ReadFile();
        }

        private void PowerStartUpView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ScrollViewer sc = GetScrollViewer(dgt);
            if (sc != null)
            {
                sc.ScrollToEnd();
            }
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

        public void PowerAdjustView_Unloaded(object sender, RoutedEventArgs e)
        {
            ParameterManager.Instance.WriteFile();

            PowerAdjustModel mod = this.DataContext as PowerAdjustModel;
            if (mod == null || mod.PowerMeterDevice == null)
                return;

            if (mod.PowerMeterDevice == null)
            {
                return;
            }
            if (mod.PowerMeterDevice.IsConnected == false)
            {
                return;
            }
            if (mod.PowerMeterDevice.IsSampling == true)
            {
                mod.PowerMeterDevice.StopSampling();
            }
        }
    }
}
