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
        }
        private int count = 0;
        private double price = 0.0;
        private double rebate = 0.0;
        private double condin = 0.0;
        private double retn = 0.0;
        private double total = 0.0;
       
        private void getTxt()
        {
            int.TryParse(txt_count.Text, out count);
            double.TryParse(txt_price.Text, out price);
            ComboBoxItem itm = cbx_rebate.SelectedItem as ComboBoxItem;
            double.TryParse(itm.Content.ToString(), out rebate);
            double.TryParse(txt_condi.Text, out condin);
            double.TryParse(txt_ret.Text, out retn);
        }

        private void msg(object obj)
        {
            ls_box.Items.Add(obj);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            getTxt();
            double tmp = 0.0;
            switch (btn.Tag.ToString())
            {
                case "factory":
                    cashBase cashRebat = cashFactory.Create(cashEnum.Rebate, rebate);
                    cashBase cashReturn = cashFactory.Create(cashEnum.Return, rebate, condin, retn);
                    total = count * price;
                    tmp = total;
                    msg($"count:{count} price:{price} total:{total}");

                    total = cashRebat.AcceptMoney(total);
                    msg($"折扣:{rebate} total:{total} save:{tmp-total}");
                    tmp = total;

                    total = cashReturn.AcceptMoney(total);
                    msg($"满({condin})减:{retn} total:{total} save:{tmp - total}");
                    break;
                case "strategy":
                    cashContex ctxRebt = new cashContex(cashEnum.Rebate, rebate, condin, retn);
                    cashContex ctxCond = new cashContex(cashEnum.Return, rebate, condin, retn);
                    total = count * price;
                    tmp = total;
                    msg($"count:{count} price:{price} total:{total}");

                    total = ctxRebt.Cash(total);
                    msg($"折扣:{rebate} total:{total} save:{tmp - total}");
                    tmp = total;

                    total = ctxCond.Cash(total);
                    msg($"满({condin})减:{retn} total:{total} save:{tmp - total}");

                    break;
            }
        }
    }
}
