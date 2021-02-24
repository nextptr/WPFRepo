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
    /// UseCommandDemo.xaml 的交互逻辑
    /// </summary>
    public partial class UseCommandDemo : UserControl
    {
        public UseCommandDemo()
        {
            InitializeComponent();
            InitUserDefineCommand();
            InitMicrosoftCommand();
        }

        private RoutedCommand clearCmd = new RoutedCommand("clear", typeof(UseCommandDemo));
        private  void InitUserDefineCommand()
        {
            //button 作为命令源
            btn_sendCmd.Command = clearCmd;

            //button的命令发送给txtbox
            btn_sendCmd.CommandTarget = txt_RecvCmd;

            //命令的判断和执行
            CommandBinding cb = new CommandBinding();
            cb.Command = clearCmd;
            //给命令添加回调函数
            //cb.CanExecute += new CanExecuteRoutedEventHandler(Cb_CanExecute);
            //cb.Executed += new ExecutedRoutedEventHandler(Cb_Executed);

            //简化
            cb.CanExecute += Cb_CanExecute;
            cb.Executed += Cb_Executed;


            //外部包装控件来连接Binding
            stack_wrap.CommandBindings.Add(cb);
        }
        private void Cb_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_RecvCmd.Text))
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
            e.Handled = true;
        }
        private void Cb_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txt_RecvCmd.Clear();
            e.Handled = true;
        }

        //微软定义的命令
        private void InitMicrosoftCommand()
        {
            //RoutedCommand copy = new RoutedCommand("copy", typeof(UseCommandDemo));

            //RoutedCommand cmd=ApplicationCommands.Delete;
            //btn_command_src.Command = ApplicationCommands.Copy;
            //btn_command_src.CommandTarget = txt_copy_dst;

            //CommandBinding cb = new CommandBinding();
            //cb.Command = ApplicationCommands.Copy; ;
            //cb.CanExecute += Cb_CanExecute1;
            //cb.Executed += Cb_Executed1;

            //unif_wrap.CommandBindings.Add(cb);
        }

        private void Cb_CanExecute1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void Cb_Executed1(object sender, ExecutedRoutedEventArgs e)
        {
            string str = Clipboard.GetText();
            txt_copy_dst.Text = str;
        }

        //CommandParameter用例
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Name.Text))
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
            e.Handled = true;
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string name = txt_Name.Text;
            if (e.Parameter.ToString() == "Teacher")
            {
                ls_NewItems.Items.Add($"New Teacher:{name},学而不厌，诲人不倦。");
            }
            if (e.Parameter.ToString() == "Student")
            {
                ls_NewItems.Items.Add($"New Student:{name},好好学习，天天向上。");
            }
        }

        private void CommandBinding_CanExecute_MyDeleteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (txt_command_dest != null)
            {
                e.CanExecute = !string.IsNullOrEmpty(this.txt_command_dest.Text);
            }
        }

        private void CommandBinding_Executed_MyDeleteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            this.txt_command_dest.Text = string.Empty;
        }
    }

    public class MyDeleteCommand
    {
        public static RoutedUICommand DeleteCommand = new RoutedUICommand();
    }
}
