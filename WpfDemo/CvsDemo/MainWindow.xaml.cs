using Common;
using Common.Csv;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;

namespace CvsDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<object, List<object>> strDic = new Dictionary<object, List<object>>();
        private Dictionary<object, List<object>> intDic = new Dictionary<object, List<object>>();

        public MainWindow()
        {
            InitializeComponent();
            lab_root.Content += RootPath.Root;

            btn_raed.Click += Btn_raed_Click;
            btn_write.Click += Btn_write_Click;
            btn_lastLoad.Click += Btn_lastLoad_Click;
            init();
        }

        private void Btn_lastLoad_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<object, List<object>> readDic = CsvHelper.Instance.ReadLastLoadData();
            if (readDic != null)
            {
                showDic(readDic);
            }
        }

        private void Btn_raed_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog
            {
                Title = "功率测试文件",
            };
            if (d.ShowDialog() == true)
            {
                var ext = System.IO.Path.GetExtension(d.FileName);
                if (ext == ".csv")
                {
                    Dictionary<object, List<object>> readDic = CsvHelper.Instance.Read(d.FileName);
                    if (readDic != null)
                    {
                        showDic(readDic);
                    }
                    else
                    {
                        MessageBox.Show("读取文件失败");
                    }
                }
            }
            else
            {
                MessageBox.Show("读取文件失败");
            }
        }
        private void Btn_write_Click(object sender, RoutedEventArgs e)
        {
            CsvHelper.Instance.Write("csvTest", strDic);
        }

        private void init()
        {
            strDic["head1"] = new List<object>();
            strDic["head1"].Add("h1str1 ");
            strDic["head1"].Add("h1str2 ");
            strDic["head1"].Add("h1str3 ");

            strDic["head2"] = new List<object>();
            strDic["head2"].Add("h2str1 ");
            strDic["head2"].Add("h2str2 ");
            strDic["head2"].Add("h2str3 ");

            strDic["head3"] = new List<object>();
            strDic["head3"].Add("h3str1 ");
            strDic["head3"].Add("h3str2 ");
            strDic["head3"].Add("h3str3 ");

            intDic["head1"] = new List<object>();
            intDic["head1"].Add(1);
            intDic["head1"].Add(2);
            intDic["head1"].Add(3);

            intDic["head2"] = new List<object>();
            intDic["head2"].Add(4);
            intDic["head2"].Add(5);
            intDic["head2"].Add(6);

            intDic["head3"] = new List<object>();
            intDic["head3"].Add(7);
            intDic["head3"].Add(8);
            intDic["head3"].Add(9);

            showDic(strDic);
            showDic(intDic);
        }



        private void showDic(Dictionary<object, List<object>> dic)
        {
            int maxSize = -1;
            foreach (var ke in dic.Keys)
            {
                if (dic[ke].Count > maxSize)
                {
                    maxSize = dic[ke].Count;
                }
            }

            for (int i = 0; i < maxSize; i++)
            {
                string str = "";
                foreach (var ke in dic.Keys)
                {
                    if (dic[ke].Count <= i)
                    {
                        str += "   ";
                    }
                    else
                    {
                        str += dic[ke][i];
                    }
                }
                ls_view.Items.Add(str);
            }

        }

    }
}
