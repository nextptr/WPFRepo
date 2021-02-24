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

namespace WpfBase.Chapter11
{
    /// <summary>
    /// unit11_2.xaml 的交互逻辑
    /// </summary>
    public partial class unit11_2 : UserControl
    {
        /// <summary>
        /// Triggers类中的重要属性
        /// 
        /// 1.Trigger：这是一种最简单的触发器。可以监测依赖属性的变化，然后使用设置器改变样式
        /// 2.MultiTrigger：与Trigger类似，但是这种触发器联合了多个条件。只有满足了所有条件，才会启动触发器
        /// 3.DataTrigger：这种触发器使用数据绑定。与Trigger类似，只不过监视的是任意绑定数据的变化
        /// 4.MultiDataTrigger：联合多个数据触发器
        /// 5.EventTrigger：这是最复杂的触发器。当事件发生时，这种触发应用动画
        /// </summary>
        public unit11_2()
        {
            InitializeComponent();
            this.Tag = "触发器";
        }
    }
}
