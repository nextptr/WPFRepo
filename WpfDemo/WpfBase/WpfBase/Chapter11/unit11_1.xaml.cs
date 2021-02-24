using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// unit11_1.xaml 的交互逻辑
    /// </summary>
    public partial class unit11_1 : UserControl
    {
        /// <summary>
        /// Style类中的重要属性
        /// 
        /// 1.Setter：设置属性值以及自动关联事件处理程序的Setter对象或EventSetter对象的集合
        /// 2.Triggers：继承自TriggerBase类并能自动改变样式设置的对象集合。例如，当另一个属性改变时，或者当发生某个事件时，可以修改样式
        /// 3.Resources：希望用于样式的资源集合。例如，可能需要使用一个对象设置多个属性，这时，更高效的做法是作为资源创建对象，
        ///              然后在Setter对象中使用该资源（而不是使用嵌套的标签作为每个Setter对象的一部分创建对象）
        /// 4.BaseOn：通过该属性可以创建继承自（可以有选择性的重写）其他样式设置的具体样式
        /// 5.TargetType：表明本Style应用于什么元素。可以创建只影响特定类型元素的设置器，还可以创建能够为恰当的元素类型自动起作用的设置器
        /// </summary>
        public unit11_1()
        {
            InitializeComponent();
            this.Tag = "第十一章样式和行为#样式";
            btn_dst.Style = (Style)btn_ori.FindResource("BigFontButtonStyle");
            this.Loaded += Unit11_1_Loaded;
        }

        Thread _thread = null;

        private void Unit11_1_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                start();
            }
        }

        private void element_MouseEnter(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
        }
        private void element_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).Background = null;
        }

        int click = 0;
        private void read()
        {
            while (IsVisible)
            {
                click++;
                click = click % 2;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    btn_bord.Tag = click;
                    btn_back.Tag = click;
                    if (click == 1)
                    {
                        btn_backrg.IsEnabled = true;
                    }
                    else
                    {
                        btn_backrg.IsEnabled = false;
                    }
                    btn_backrg1.Tag = click.ToString();
                    btn_backrg2.Tag = click.ToString();
                }));
                Thread.Sleep(1000);
            }
            _thread = null;
        }
        private void start()
        {
            if (IsVisible)
            {
                _thread = null;
                _thread = new Thread(read);
                _thread.SetApartmentState(ApartmentState.STA);
                _thread.IsBackground = true;
                _thread.Start();
            }
        }
    }
}
