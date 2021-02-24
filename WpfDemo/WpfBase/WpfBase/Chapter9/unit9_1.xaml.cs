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

namespace WpfBase.Chapter9
{
    /// <summary>
    /// unit9_1.xaml 的交互逻辑
    /// </summary>
    public partial class unit9_1 : UserControl
    {
        /// <summary>
        /// 在各个控件的的事件处理函数中编写业务逻辑尽管很合理，但是却没有减少任何工作，
        /// 如果不同控件使用相同的业务逻辑，以及控件之间的互斥操作，，，这种方式不够简洁
        /// 为了界面和业务逻辑分离如MVVM模式，最好使用命令。
        /// </summary>
        public unit9_1()
        {
            InitializeComponent();
            this.Tag = "第九章命令";
            InitBind();
        }

        /// <summary>
        /// System.Windows.Input.ICommand接口的定义
        /// 创建自己的命令时不会直接继承ICommand，而是使用RoutedCommand类
        /// </summary>
        public interface ICommd
        {
            void Execute(object parameter );          //包含程序业务逻辑
            bool CanExecute(object parameter);        //根据传入参数判断命令状态是否可用
            event EventHandler CanExecuteChanged;     //命令状态发生改变后，修改绑定此命令控件的状态
        }

        private bool isDirty = false;
        private void InitBind()
        {
            //由于ApplicationCommands.New命令还没有与之关联的绑定，所以xaml中的控件自动被禁用，为了使用控件需要
            //1.命令被触发时执行什么操作
            //2.如何确定命令是否能够被执行
            //3.命令在何处起作用，限制在单个按钮中还是整个窗口中
            CommandBinding binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += Binding_Executed;
            this.CommandBindings.Add(binding);

            txt_edit.TextChanged += Txt_edit_TextChanged;
        }

        private void Binding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("New command trigger by " + e.ToString());
        }

        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //open和save的使能状态通过textbox中的内容长度来进行变换，
            //cut,copy,past则根据是否选中文本，以及剪切板是否有内容来进行状态使能
            e.CanExecute = isDirty;
        }

        private void Txt_edit_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Text.ToString().Length <= 4)
            {
                isDirty = false;
            }
            else
            {
                isDirty = true;
            }
        }
    }
}
