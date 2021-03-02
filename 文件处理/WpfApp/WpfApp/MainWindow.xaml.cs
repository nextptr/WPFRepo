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
            btn_clear.Click += Btn_clear_Click;
            btn_motion_analyse.Click += Btn_motion_analyse_Click;
            btn_align_analyse.Click += Btn_align_analyse_Click;
            btn_memory_analyse.Click += Btn_memory_analyse_Click;
        }

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ls_box.Items.Clear();
        }
        private void addMsg(string str)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ls_box.Items.Insert(0, str);
            }));
        }

        private void Btn_motion_analyse_Click(object sender, RoutedEventArgs e)
        {
            FileAnalyseHelper hp = new FileAnalyseHelper();
            filePathObj path = hp.SelectFileDialog();
            addMsg("开始分析:"+path.fileName);
            hp.analyseMotionData(path);
            addMsg("处理完成");
        }
        private void Btn_align_analyse_Click(object sender, RoutedEventArgs e)
        {
            FileAnalyseHelper hp = new FileAnalyseHelper();
            filePathObj path = hp.SelectFileDialog();
            addMsg("开始分析:" + path.fileName);
            hp.analyseAlignData(path);
            addMsg("处理完成");
        }
        private void Btn_memory_analyse_Click(object sender, RoutedEventArgs e)
        {
            FileAnalyseHelper hp = new FileAnalyseHelper();
            filePathObj path = hp.SelectFileDialog();
            addMsg("开始分析:" + path.fileName);
            hp.analyseMemoryData(path);
            addMsg("处理完成");
        }
    }
}
