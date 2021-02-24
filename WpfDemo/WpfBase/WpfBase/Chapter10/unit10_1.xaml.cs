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

namespace WpfBase.Chapter10
{
    /// <summary>
    /// unit10_1.xaml 的交互逻辑
    /// </summary>
    public partial class unit10_1 : UserControl
    {
        public unit10_1()
        {
            InitializeComponent();
            this.Tag = "第十章资源";
            DynamicResource();   //动态资源的创建和使用
        }

        private void DynamicResource()
        {
            this.Resources["dyn_sou"] = new SolidColorBrush(Colors.Blue);         //后台定义资源及使用
            btn_dynamic1.Background = (SolidColorBrush)this.Resources["dyn_sou"];

            ImageBrush brush = (ImageBrush)this.FindResource("HappyFace");        //资源查找
            btn_dynamic2.Background = brush;
        }



    }
}
