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

namespace WpfBase.Chapter2
{
    /// <summary>
    /// unit2_1.xaml 的交互逻辑
    /// </summary>

    //使用partial关键字生成部分类
    public partial class unit2_1 : UserControl
    {
        public unit2_1()
        {
            InitializeComponent(); //调用 System.Windows.Application.LoadComponent()函数提取编译过的unit2_1.xaml文件，统一.xaml和.cs组合成一个完整的类
            this.Tag = "第二章xaml";
            //xmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation"  wpf的核心名称空间，包含所有wpf类
            //xmlns: x = "http://schemas.microsoft.com/winfx/2006/xaml"            xaml名称空间，包含各种xaml实用特性，使用前缀x表示，如x:name,x:key
        }
    }
}
