using System.Windows.Controls;
using System.Windows.Data;

namespace WpfBase.Chapter8
{
    /// <summary>
    /// unit8_1.xaml 的交互逻辑
    /// </summary>
    public partial class unit8_1 : UserControl
    {
        public unit8_1()
        {
            InitializeComponent();
            this.Tag = "第八章元素绑定#绑定实例";

            Binding binding = new Binding();
            binding.Source = slider_fontsize2;
            binding.Path = new System.Windows.PropertyPath("Value");
            binding.Mode = BindingMode.OneWay;
            txt_codeBind.SetBinding(TextBlock.FontSizeProperty, binding);
        }

        private void ComboBoxItem_Selected(object sender, System.Windows.RoutedEventArgs e)
        {
            ComboBoxItem item = sender as ComboBoxItem;
            com_color.Background = item.Background;
        }
    }
}
