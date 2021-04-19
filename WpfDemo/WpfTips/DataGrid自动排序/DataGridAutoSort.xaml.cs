using Common;
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

namespace WpfTips.DataGridSort
{
    /// <summary>
    /// DataGridAutoSort.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridAutoSort : UserControl
    {
        public ObservableCollection<TestDataItem> Param = new ObservableCollection<TestDataItem>();

        public DataGridAutoSort()
        {
            InitializeComponent();
            dgt.ItemsSource = Param;
        }

        char dsc = 'a';
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TestDataItem item = new TestDataItem();
            item.Description = dsc++.ToString();
            item.TestKey = 1;
            item.TestValue = 2;
            Param.Add(item);
        }

        private void dgt_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            List<TestDataItem> ls = new List<TestDataItem>();
            foreach (var item in Param.OrderBy(p => p.TestKey))
            {
                ls.Add(item.clone() as TestDataItem);
            }
            for (int i = 0; i < ls.Count; i++)
            {
                Param[i].Description = ls[i].Description;
                Param[i].TestKey = ls[i].TestKey;
                Param[i].TestValue = ls[i].TestValue;
            }
        }
    }
    public class TestDataItem : NotifyPropertyChanged
    {
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private double testKey;
        public double TestKey
        {
            get
            {
                return testKey;
            }
            set
            {
                testKey = value;
                OnPropertyChanged(nameof(TestKey));
            }
        }

        private double testValue;
        public double TestValue
        {
            get
            {
                return testValue;
            }
            set
            {
                testValue = value;
                OnPropertyChanged(nameof(TestValue));
            }
        }

        public TestDataItem()
        {
        }

        public TestDataItem clone()
        {
            TestDataItem clon = new TestDataItem();
            clon.Description = this.Description;
            clon.TestKey = this.TestKey;
            clon.TestValue = this.TestValue;
            return clon;
        }
    }
}
