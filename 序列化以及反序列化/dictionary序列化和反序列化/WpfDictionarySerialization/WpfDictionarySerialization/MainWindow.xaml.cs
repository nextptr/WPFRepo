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

namespace WpfDictionarySerialization
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public myDictionary<string, int> keyValuePairs = new myDictionary<string, int>();
        public MainWindow()
        {
            InitializeComponent();
            btnWrite.Click += BtnWrite_Click;
            btnRead.Click += BtnRead_Click;
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            ReadParameter();
        }
        private void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            keyValuePairs["1"] =11;
            keyValuePairs["2"] =22;
            keyValuePairs["3"] =33;
            keyValuePairs["4"] =44;
            keyValuePairs["5"] =55;
            keyValuePairs["6"] =66;
            WriteParameter();
        }

        public void ReadParameter()
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
        public void WriteParameter()
        {
            Write("test.xml");
        }

        protected bool Write(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create);
                XmlSerializer ser = new XmlSerializer(keyValuePairs.GetType());
                ser.Serialize(fs, keyValuePairs);
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("保存文件失败:" + e.Message);
                return false;
            }
        }
        protected bool Read(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    XmlSerializer ser = new XmlSerializer(keyValuePairs.GetType());
                    myDictionary<string,int> tmp = ser.Deserialize(fs) as myDictionary<string,int>;
                    if (tmp != null)
                    {
                        //this.GripeAxis = tmp.GripeAxis;
                        //this.LifterAxis = tmp.LifterAxis;
                        //this.SorterAxis = tmp.SorterAxis;
                        //this.TransforAxis = tmp.TransforAxis;
                        //this.GripeBtn = tmp.GripeBtn;
                        //this.LifterBtn = tmp.LifterBtn;
                        //this.SorterBtn = tmp.SorterBtn;
                        //this.TransforBtn = tmp.TransforBtn;
                    }
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
    }
}
