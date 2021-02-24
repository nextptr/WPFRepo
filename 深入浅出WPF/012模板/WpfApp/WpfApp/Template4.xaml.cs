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
    /// Template4.xaml 的交互逻辑
    /// </summary>
    public partial class Template4 : UserControl
    {
        public Template4()
        {
            InitializeComponent();

            List<axisEntity> ls = new List<axisEntity>();
            ls.Add(new axisEntity("123", 44));
            ls.Add(new axisEntity("456", 44));
            ls.Add(new axisEntity("789", 44));
            ls.Add(new axisEntity("321", 44));
            ls.Add(new axisEntity("654", 44));
            axis_ls.ItemsSource = ls;

            axis_ls.SelectionChanged += Axis_ls_SelectionChanged;
        }

        private void Axis_ls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int a = 10;
            

            ListBox tb = e.OriginalSource as ListBox;
            
            ContentPresenter cp = tb.TemplatedParent as ContentPresenter;
            axisEntity stu = cp.DataContext as axisEntity;
            MessageBox.Show(stu.AxisName);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBlock tb = this.uc.Template.FindName("txt_box1", this.uc) as TextBlock;
            tb.Text = "Hellow WPF";
            StackPanel sp = tb.Parent as StackPanel;
            (sp.Children[1] as TextBlock).Text = "Hello ControlTemplate";
            (sp.Children[2] as TextBlock).Text = "I Can Find You";
        }

        private void Txt_boxName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = e.OriginalSource as TextBox;
            ContentPresenter cp = tb.TemplatedParent as ContentPresenter;
            Student stu = cp.DataContext as Student;
            this.lisView.SelectedItem = stu;

            ListViewItem lvi = this.lisView.ItemContainerGenerator.ContainerFromItem(stu) as ListViewItem;
            CheckBox chb = FindVisualChild<CheckBox>(lvi);
            MessageBox.Show(chb.Name);

        }

        private childType FindVisualChild<childType>(DependencyObject obj) where childType : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is childType)
                    return child as childType;
                else
                {
                    childType childOfChild = FindVisualChild<childType>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void Label_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label tb = e.OriginalSource as Label;
            ContentPresenter cp = tb.TemplatedParent as ContentPresenter;
            axisEntity stu = cp.DataContext as axisEntity;
            MessageBox.Show(stu.AxisName);
        }
    }

    public class axisEntity : NotifyPropertyChanged
    {
        private string axisName = "";
        public int AxisId = -1;

        public string AxisName
        {
            get
            {
                return axisName;
            }
            set
            {
                axisName = value;
                OnPropertyChanged("AxisName");
            }
        }

        public axisEntity(string name, int id)
        {
            AxisName = name;
            AxisId = id;
        }
    }
}
