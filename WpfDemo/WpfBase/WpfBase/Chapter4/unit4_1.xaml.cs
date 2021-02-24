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

namespace WpfBase.Chapter4
{
    /// <summary>
    /// unit4_1.xaml 的交互逻辑
    /// </summary>
    public partial class unit4_1 : UserControl
    {
        //相关博客
        //依赖属性
        //https://www.cnblogs.com/fly-bird/p/8552795.html
        //https://www.cnblogs.com/ssvip/p/9184199.html
        //https://blog.csdn.net/NA_OnlyOne/article/details/53308243
        //https://www.cnblogs.com/shen119/p/3396388.html
        //https://www.cnblogs.com/zhaoxixi/p/4947996.html
        //附加属性
        //https://www.cnblogs.com/DebugLZQ/p/3153098.html

        Person person = new Person();
        Person person2 = new Person();
        public unit4_1()
        {
            InitializeComponent();
            this.Tag = "第四章依赖项属性";
            txt_input.TextChanged += Txt_input_TextChanged;
            //绑定依赖属性
            txt_input2.DataContext = person2;
            lab_view2.DataContext = person2;
        }

        private void Txt_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            //依赖属性赋值
            person.SetValue(Person.NameProperty, txt_input.Text);
            lab_view.Content = person.GetValue(Person.NameProperty);
        }
    }
}
