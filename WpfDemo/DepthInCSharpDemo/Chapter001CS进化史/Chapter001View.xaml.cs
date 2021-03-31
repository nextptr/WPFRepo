using System;
using System.Collections.Generic;
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

namespace DepthInCSharpDemo
{
    /// <summary>
    /// Chapter001View.xaml 的交互逻辑
    /// </summary>
    public partial class Chapter001View : UserControl
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public Chapter001View()
        {
            InitializeComponent();
            CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(lsBox.Items);
            ((INotifyCollectionChanged)myCollectionView).CollectionChanged += ControlCollectionChanged;
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
        private void ControlCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ScrollViewer sc = GetScrollViewer(lsBox);
            if (sc != null)
            {
                sc.ScrollToEnd();
            }
        }
    }
}
