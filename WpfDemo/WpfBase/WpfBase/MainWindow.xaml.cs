using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfBase
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<UserControl> userItemLs = new List<UserControl>();
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            TitleBar.MouseMove += MainWindow_MouseMove;
            BtnClose.Click += BtnClose_Click;
            BtnMin.Click += BtnMin_Click;

           // InitTreeView();
            InitUserControl();
        }
        private void InitTreeView()
        {
            foreach (TreeViewItem item in treeRoot.Items)
            {
                if (item.Items.Count <= 0)
                {
                    item.Selected += treeItem_Selected;
                }
                else
                {
                    foreach (TreeViewItem leefItem in item.Items)
                    {
                        leefItem.Selected += treeItem_Selected;
                    }
                }
            }
        }
        private void InitUserControl()
        {
            string fullPath = Directory.GetCurrentDirectory();           //"fullPath = "D:\\代码库\\WPF\\WpfBase\\WpfBase\\bin\\Debug"
            string filePath = fullPath.Remove(fullPath.Length - 10, 10); //"D:\\代码库\\WPF\\WpfBase\\WpfBase"
            string[] dir = Directory.GetDirectories(filePath);           //获取"D:\\代码库\\WPF\\WpfBase\\WpfBase"的所有子文件夹
            var pth = dir.Where(x => x.Contains("Chapter"));             //找到Chapter系列文件夹路径
            Dictionary<int, string> dictionaryPath = new Dictionary<int, string>(); //pth排序
            foreach (string tmp in pth)
            {
                string[] spt = tmp.Split('\\');
                string num = spt[spt.Length - 1].Replace("Chapter", "");
                int index = 0;
                int.TryParse(num, out index);
                dictionaryPath[index] = tmp;
            }
            List<int> sortLs = dictionaryPath.Keys.ToList();
            List<string> paths = new List<string>();
            sortLs.Sort();
            foreach (int id in sortLs)
            {
                paths.Add(dictionaryPath[id]);
            }
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;//获取程序集名称 WpfBase
            List<UserControl> controls = new List<UserControl>();
            foreach (string tmp in paths)
            {
                string[] ls = Directory.GetFiles(tmp);
                if (ls.Length == 0)
                {
                    return;
                }

                controls.Clear();
                foreach (string th in ls)
                {
                    //th = "D:\\代码库\\WPF\\WpfBase\\WpfBase\\Chapter2\\unit2_1.xaml"
                    //从th中解析出UserControl控件类型
                    if (th.EndsWith(".xaml"))
                    {
                        string gg = @"(" + assemblyName + @")+.*(xaml)$"; //正则表达式
                        string val = Regex.Match(th, gg, RegexOptions.RightToLeft).Value; //val = "WpfBase\\WpfBase\\Chapter2\\unit2_1.xaml"
                        val = val.TrimEnd(".xaml".ToCharArray());                         //val = "WpfBase\\WpfBase\\Chapter2\\unit2_1"
                        val = val.TrimStart((assemblyName).ToCharArray());                //val = "\\WpfBase\\Chapter2\\unit2_1"
                        val = val.TrimStart('\\');                                        //val = "WpfBase\\Chapter2\\unit2_1"
                        val = val.Replace('\\', '.');                                     //val = "WpfBase.Chapter2.unit2_1"
                        string[] spt = val.Split('.');
                        if (spt.Length != 3)
                        {
                            continue;
                        }
                        if (!spt[2].Contains("unit"))
                        {
                            continue;
                        }
                        UserControl p = GetControlInstance(assemblyName, val);
                        p.Name = spt[spt.Length - 1];
                        controls.Add(p);
                    }
                }

                //生成TreeView控件树，添加相应的点击事件
                if (controls.Count == 1)
                {
                    TreeViewItem rootItem = new TreeViewItem();
                    rootItem.Header = controls[0].Tag;
                    rootItem.Tag = controls[0].Name;
                    rootItem.Selected += treeItem_Selected;
                    treeRoot.Items.Add(rootItem);
                    userItemLs.Add(controls[0]);
                }
                else if (controls.Count > 1)
                {
                    TreeViewItem rootItem = new TreeViewItem();
                    string[] spt = controls[0].Tag.ToString().Split('#');
                    if (spt.Length < 1 || spt.Length > 2)
                    {
                        break;
                    }
                    else if (spt.Length == 1)
                    {
                        rootItem.Header = "默认值";
                        controls[0].Tag = spt[0];
                    }
                    else
                    {
                        rootItem.Header = spt[0];
                        controls[0].Tag = spt[1];
                    }
                    foreach (UserControl ctl in controls)
                    {
                        TreeViewItem childItem = new TreeViewItem();
                        childItem.Header = ctl.Tag;
                        childItem.Tag = ctl.Name;
                        childItem.Selected += treeItem_Selected;
                        rootItem.Items.Add(childItem);
                        userItemLs.Add(ctl);
                    }
                    treeRoot.Items.Add(rootItem);
                }
            }
        }
        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void treeItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            string tag = item.Tag.ToString();
            bool flg = false;
            foreach (UserControl ctl in userItemLs)
            {
                if (ctl.Name == tag)
                {
                    grid_ctlRoot.Children.Clear();
                    grid_ctlRoot.Children.Add(ctl);
                    flg = true;
                    return;
                }
            }
            if (flg == false)
            {
                Label lab = new Label();
                lab.Content = "选中单元无内容！";
                lab.FontSize = 20;
                lab.Foreground = Brushes.Red;
                lab.VerticalContentAlignment = VerticalAlignment.Center;
                lab.HorizontalContentAlignment = HorizontalAlignment.Center;
                grid_ctlRoot.DataContext = null;
                grid_ctlRoot.Children.Clear();
                grid_ctlRoot.Children.Add(lab);
            }
        }

        //private void InitUserControl()
        //{
        //    string fullPath = Directory.GetCurrentDirectory();           //"fullPath = "D:\\代码库\\WPF\\WpfBase\\WpfBase\\bin\\Debug"
        //    string filePath = fullPath.Remove(fullPath.Length - 10, 10); //"D:\\代码库\\WPF\\WpfBase\\WpfBase"
        //    string[] dir = Directory.GetDirectories(filePath);           //获取"D:\\代码库\\WPF\\WpfBase\\WpfBase"的所有子文件夹
        //    var pth = dir.Where(x => x.Contains("Chapter"));             //找到Chapter系列文件夹路径

        //    string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;//获取程序集名称 WpfBase
        //    List<string> objTypes = new List<string>();
        //    foreach (string tmp in pth)
        //    {
        //        string[] ls = Directory.GetFiles(tmp);
        //        if (ls.Length == 0)
        //        {
        //            return;
        //        }
        //        if (ls.Length == 1)
        //        {

        //        }

        //        foreach (string th in ls)
        //        {
        //            //th = "D:\\代码库\\WPF\\WpfBase\\WpfBase\\Chapter2\\unit2_1.xaml"
        //            //从th中解析出UserControl控件类型
        //            if (th.EndsWith(".xaml"))
        //            {
        //                string gg = @"(" + assemblyName + @")+.*(xaml)$"; //正则表达式
        //                string val = Regex.Match(th, gg, RegexOptions.RightToLeft).Value; //val = "WpfBase\\WpfBase\\Chapter2\\unit2_1.xaml"
        //                val = val.TrimEnd(".xaml".ToCharArray());                         //val = "WpfBase\\WpfBase\\Chapter2\\unit2_1"
        //                val = val.TrimStart((assemblyName).ToCharArray());                //val = "\\WpfBase\\Chapter2\\unit2_1"
        //                val = val.TrimStart('\\');                                        //val = "WpfBase\\Chapter2\\unit2_1"
        //                val = val.Replace('\\', '.');                                     //val = "WpfBase.Chapter2.unit2_1"
        //                objTypes.Add(val);                                                //获得控件对象的具体类型

        //                UserControl p = GetControlInstance(assemblyName, val);


        //                userItemLs.Add(p);
        //            }
        //        }
        //    }
        //    //for (int i = 0; i < objTypes.Count; ++i)
        //    //{
        //    //    //assemblyName程序集，files[i]具体类型
        //    //    ObjectHandle handle = Activator.CreateInstance(assemblyName, objTypes[i]);
        //    //    UserControl p = handle.Unwrap() as UserControl;
        //    //    userItemLs.Add(p);
        //    //}
        //}

        private UserControl GetControlInstance(string assemblyName,string typeName)
        {
            ObjectHandle handle = Activator.CreateInstance(assemblyName, typeName);
            return handle.Unwrap() as UserControl;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }
    }
}
