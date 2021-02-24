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

namespace WpfBase.Common
{
    /// <summary>
    /// ListMessagePanel.xaml 的交互逻辑
    /// </summary>
    public partial class ListMessagePanel : UserControl
    {
        public ListMessagePanel()
        {
            InitializeComponent();
            btn_clear.Click += Btn_clear_Click;
        }

        public void AddMsgInFront(string msg)
        {
            ListViewItem itm = new ListViewItem();
            itm.Content = msg;
            ls_View.Items.Insert(0, itm);
        }

        public void AddMsg(string msg)
        {
            ListViewItem itm = new ListViewItem();
            itm.Content = msg;
            ls_View.Items.Add(itm);
            ls_View.ScrollIntoView(itm);
        }

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ls_View.Items.Clear();
        }
    }
}
