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
    /// TreeHelper.xaml 的交互逻辑
    /// </summary>
    public partial class TreeHelper : UserControl
    {
        public TreeHelper()
        {
            InitializeComponent();
            btn_logic.Click += Btn_logic_Click;
            btn_visual.Click += Btn_visual_Click;
        }


        private void Btn_logic_Click(object sender, RoutedEventArgs e)
        {
            ls_box.Items.Clear();
            fondLogicParent(mid_grid_logic);
            fondLogicChild(mid_grid_logic);
        }
        private void fondLogicChild(DependencyObject obj)
        {
            msg($"当前逻辑节点： {obj.GetType()}");
            foreach (var log in LogicalTreeHelper.GetChildren(obj))
            {
                msg($"子节点：{log.GetType()}");
            }
            foreach (var  log in LogicalTreeHelper.GetChildren(obj))
            {
                try
                {
                    DependencyObject dpo = log as DependencyObject;
                    fondLogicChild(dpo);
                }
                catch (Exception ex)
                {

                }
            }
        }
        private void fondLogicParent(DependencyObject obj)
        {
            if (LogicalTreeHelper.GetParent(obj) != null)
            {
                msg($"当前逻辑节点： {obj.GetType()}  父节点：{LogicalTreeHelper.GetParent(obj).GetType()}");
                fondLogicParent(LogicalTreeHelper.GetParent(obj));
            }
        }


        private void Btn_visual_Click(object sender, RoutedEventArgs e)
        {
            ls_box.Items.Clear();
            fondVisuaParent(mid_grid_visua);
            fondVisuaChild(mid_grid_visua);
        }
        private void fondVisuaChild(DependencyObject obj)
        {
            msg($"当前Visual节点： {obj.GetType()}");
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                msg($"子节点： {VisualTreeHelper.GetChild(obj, i).GetType()}");
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                fondVisuaChild(VisualTreeHelper.GetChild(obj, i));
            }
        }
        private void fondVisuaParent(DependencyObject obj)
        {
            if (VisualTreeHelper.GetParent(obj) != null)
            {
                msg($"当前Visual节点： {obj.GetType()}  父节点：{VisualTreeHelper.GetParent(obj).GetType()}");
                fondVisuaParent(VisualTreeHelper.GetParent(obj));
            }
        }

        private void msg(object obj)
        {
            ls_box.Items.Insert(0, obj);
        }
    }
}
