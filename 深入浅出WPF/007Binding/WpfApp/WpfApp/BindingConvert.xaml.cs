using System;
using System.Collections.Generic;
using System.Globalization;
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
using WpfApp.Convert;

namespace WpfApp
{
    /// <summary>
    /// BindingConvert.xaml 的交互逻辑
    /// </summary>
    public partial class BindingConvert : UserControl
    {
        public int intValue = 10;
        public BindingConvert()
        {
            InitializeComponent();
            test_check();
            test_convert();
            test_multBinding();
        }


        //binding Check
        private void test_check()
        {
            Binding binding = new Binding("Value")
            {
                Source = this.slid_check
            };

            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            RangeValidationRule rvr = new RangeValidationRule();
            //rvr.ValidatesOnTargetUpdated = false;  //默认只在target修改source时进行校验
            rvr.ValidatesOnTargetUpdated = true;     //source改变target时也进行校验
            binding.ValidationRules.Add(rvr);
            binding.NotifyOnValidationError = true;  //校验错误时触发事件
            txt_check.SetBinding(TextBox.TextProperty, binding);

            txt_check.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(checkError));

        }
        private void checkError(object sender,RoutedEventArgs arg)
        {
            lab_error.Content = $"当前值:{slid_check.Value.ToString("F3")} 校验0<val<100错误 ";

            if (Validation.GetErrors(this.txt_check).Count > 0)
            {
                slid_check.ToolTip = Validation.GetErrors(txt_check)[0].ErrorContent.ToString();
            }
        }

        //convert

        private void test_convert()
        {
            cmx_color.SelectionChanged += Cmx_color_SelectionChanged;
        }

        private void Cmx_color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            ComboBoxItem item = box.SelectedItem as ComboBoxItem;
            if (item.Content.ToString().Equals("Red"))
            {
                rec_color.Fill = Brushes.Red;
            }
            if (item.Content.ToString().Equals("Green"))
            {
                rec_color.Fill = Brushes.Green;
            }
            if (item.Content.ToString().Equals("Blue"))
            {
                rec_color.Fill = Brushes.Blue;
            }
            if (item.Content.ToString().Equals("Black"))
            {
                rec_color.Fill = Brushes.Black;
            }
            if (item.Content.ToString().Equals("White"))
            {
                rec_color.Fill = Brushes.White;
            }
        }

        //multBinding
        private void test_multBinding()
        {
            Binding b1 = new Binding("Text") { Source = txt_userName };
            Binding b2 = new Binding("Text") { Source = txt_confirm_name };
            Binding b3 = new Binding("Text") { Source = txt_userEmil };
            Binding b4 = new Binding("Text") { Source = txt_confirm_emil };

            MultiBinding mb = new MultiBinding() { Mode = BindingMode.OneWay };
            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);
            mb.Bindings.Add(b3);
            mb.Bindings.Add(b4);
            mb.Converter = new MultConvert();
            btn_submt.SetBinding(Button.IsEnabledProperty, mb);
        }
    }


    public class RangeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double val = 0;
            if (double.TryParse(value.ToString(), out val))
            {
                if (val >= 0 && val <= 100)
                {
                    return new ValidationResult(true, null);
                }
            }
            return new ValidationResult(false, "校验失败");
        }
    }
}
