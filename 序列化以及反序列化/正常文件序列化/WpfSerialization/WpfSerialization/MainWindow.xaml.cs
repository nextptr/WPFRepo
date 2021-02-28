using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace WpfSerialization
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public testItem testItem;
        public MainWindow()
        {
            InitializeComponent();
            btnRead.Click += BtnRead_Click;
            btnWrite.Click += BtnWrite_Click;
            testItem = new testItem();
        }

        private void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            testItem.age = 21;
            testItem.name = "wang";
            Write("test.xml");
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            string fileName = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.Filter = "*.xml|*.xml";
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    fileName = ofd.FileName;
                    if (Read(fileName))
                    {

                    }
                    else
                    {
                        MessageBox.Show("读取配置文件失败");
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
        }

        public bool Read(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    XmlSerializer ser = new XmlSerializer(testItem.GetType());//以testItem.GetType()类型进行反序列化读取
                    testItem = ser.Deserialize(fs) as testItem;//获取反序列化后的值
                    fs.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("读取文件失败:" + e.Message);
            }
            return false;
        }



        public bool Write(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create);
                XmlSerializer ser = new XmlSerializer(testItem.GetType());//以testItem.GetType()类型进行序列化
                ser.Serialize(fs, testItem);//将testItem对象序列化为fs文件
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("保存文件失败:" + e.Message);
                return false;
            }
        }
    }
}
