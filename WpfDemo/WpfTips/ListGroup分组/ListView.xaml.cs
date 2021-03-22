using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WpfTips.ListGroup
{
    /// <summary>
    /// ListView.xaml 的交互逻辑
    /// </summary>
    public partial class ListView : UserControl
    {
        public ListView()
        {
            InitializeComponent();
        }

        private DataList dataList;
        public DataList DataList
        {
            set
            {
                dataList = value;
                IOInit();
            }
            get => dataList;
        }

        private void IOInit()
        {
            //if (DataList != null)
            //{
            //    this.Dispatcher.Invoke(() =>
            //    {
            //        //设置列表控件资源，并定义组的规则和排序
            //        its.ItemsSource = DataList;
            //        ICollectionView cv = CollectionViewSource.GetDefaultView(its.ItemsSource);
            //        cv.SortDescriptions.Clear();
            //        cv.GroupDescriptions.Clear();
            //        cv.SortDescriptions.Add(new SortDescription("Attr.IOIndex", ListSortDirection.Ascending));
            //        cv.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            //        cv.GroupDescriptions.Add(new PropertyGroupDescription("Attr.GroupName"));
            //    }, System.Windows.Threading.DispatcherPriority.Send);
            //}

            //if (IO != null)
            //{
            //    this.Dispatcher.Invoke(() =>
            //    {
            //        //设置列表控件资源，并定义组的规则和排序
            //        //ICollectionView cv = CollectionViewSource.GetDefaultView(ioList.ItemsSource);
            //        //cv.SortDescriptions.Clear();
            //        //cv.GroupDescriptions.Clear();
            //        ////cv.SortDescriptions.Add(new SortDescription("Attr.Priority", ListSortDirection.Ascending));
            //        //cv.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            //        //cv.GroupDescriptions.Add(new PropertyGroupDescription("IO.GroupName"));
            //        var groups = IO.GroupBy(s => s.IO.GroupName).Select(g => g.Key).ToList();
            //        groups.Insert(0, "全部");
            //        groupList.ItemsSource = groups;
            //        groupList.SelectedIndex = 0;
            //        ioList.ItemsSource = IO;
            //    }, System.Windows.Threading.DispatcherPriority.Send);
            //}
        }
    }

    public class DataList : IEnumerable<DataItem>
    {
        public IEnumerator<DataItem> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class DataItem
    {

    }
}
