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
    /// BindingImprove.xaml 的交互逻辑
    /// </summary>
    public partial class BindingImprove : UserControl
    {
        public BindingImprove()
        {
            InitializeComponent();
            btn_add.Click += Btn_add_Click;

            ObjectDataProvideBinding();
        }
        private void msg(object obj)
        {
            ls_box.Items.Insert(0, obj);
        }
        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {
            ObjectDataProvider odp = new ObjectDataProvider();
            odp.ObjectInstance = new Calculator();
            odp.MethodName = "Add";
            odp.MethodParameters.Add("100");
            odp.MethodParameters.Add("200");
            msg(odp.Data);
        }


        //类ObjectDataProvideBinding包装
        private void ObjectDataProvideBinding()
        {
            //创建并配置ObjectDataProvider对象
            ObjectDataProvider odp = new ObjectDataProvider();
            odp.ObjectInstance = new Calculator();
            odp.MethodName = "Add";
            odp.MethodParameters.Add("0");
            odp.MethodParameters.Add("0");

            //以ObjectDataProvider为对象为Source创建Binding
            Binding bindingToArg1 = new Binding("MethodParameters[0]")
            {
                Source = odp,
                BindsDirectlyToSource = true,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            Binding bindingToArg2 = new Binding("MethodParameters[1]")
            {
                Source = odp,
                BindsDirectlyToSource = true,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            Binding bindingToResult = new Binding(".") { Source = odp };

            //将Binding关联到UI元素上
            this.txt_data_arg1.SetBinding(TextBox.TextProperty, bindingToArg1);
            this.txt_data_arg2.SetBinding(TextBox.TextProperty, bindingToArg2);
            this.lab_data_result.SetBinding(Label.ContentProperty, bindingToResult);
        }
    }


    class Calculator
    {
        public string Add(string arg1, string arg2)
        {
            double x = 0;
            double y = 0;
            double z = 0;
            if (double.TryParse(arg1, out x) && double.TryParse(arg2, out y))
            {
                z = x + y;
                return z.ToString();
            }
            return "Input Error";
        }
    }

}
