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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btn_clear.Click += Btn_clear_Click;
            btn_parent.Click += Btn_parent_Click;
            btn_child.Click += Btn_child_Click;
        }

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ls_view.Items.Clear();
        }

        private void Btn_parent_Click(object sender, RoutedEventArgs e)
        {
            string parentName = "";
            List<Grid> gridList = FindVisualParent<Grid>(btn_parent);
            foreach (Grid item in gridList)
            {
                parentName += string.IsNullOrEmpty(parentName) ? item.Name.ToString() : "," + item.Name.ToString();
            }
            showMsg(string.Format(btn_parent.Name.ToString() + "共有{0}个逻辑父级,名称分别为{1}", gridList.Count, parentName));
            List<string> ls = FindVisualParent(btn_parent);
            showMsg("******************************************");
            foreach (string st in ls)
            {
                showMsg(st);
            }
        }

        private void Btn_child_Click(object sender, RoutedEventArgs e)
        {
            string btnName = "";
            List<Button> btnList = FindVisualChild<Button>(top_unif);
            foreach (Button item in btnList)
            {
                btnName += string.IsNullOrEmpty(btnName) ? item.Name.ToString() : "," + item.Name.ToString();
            }
            showMsg(string.Format(top_unif.Name.ToString() + "共有{0}个Button,名称分别为{1}", btnList.Count, btnName));

            List<string> ls = FindVisualChild(top_grid);
            showMsg("******************************************");
            foreach (string st in ls)
            {
                showMsg(st);
            }
        }

        private void showMsg(string str)
        {
            ls_view.Items.Add(str);
        }
        private List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new List<T>();
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        TList.Add((T)child);
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                    else
                    {
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }
        }

        private List<string> FindVisualChild(DependencyObject obj)
        {
            try
            {
                List<string> TList = new List<string>();
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null)
                    {
                        TList.Add(child.GetType().ToString());
                        List<string> childOfChildren = FindVisualChild(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }
        }

        private List<T> FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new List<T>();
                DependencyObject parent = VisualTreeHelper.GetParent(obj);
                if (parent != null && parent is T)
                {
                    TList.Add((T)parent);
                    List<T> parentOfParent = FindVisualParent<T>(parent);
                    if (parentOfParent != null)
                    {
                        TList.AddRange(parentOfParent);
                    }
                }
                else if (parent != null)
                {
                    List<T> parentOfParent = FindVisualParent<T>(parent);
                    if (parentOfParent != null)
                    {
                        TList.AddRange(parentOfParent);
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }
        }
        private List<string> FindVisualParent(DependencyObject obj)
        {
            try
            {
                List<string> TList = new List<string>();
                DependencyObject parent = VisualTreeHelper.GetParent(obj);
                if (parent != null)
                {
                    TList.Add(parent.GetType().ToString());
                    List<string> parentOfParent = FindVisualParent(parent);
                    if (parentOfParent != null)
                    {
                        TList.AddRange(parentOfParent);
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }
        }

    }
}
