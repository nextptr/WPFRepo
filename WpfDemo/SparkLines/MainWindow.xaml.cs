using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Xml.Serialization;


namespace SparkLines
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread localThread = null;
        private static bool WorkFlag = false;
        private static bool ThreadFlag = false;
        private List<double> testValues = new List<double>();
        private Dictionary<int, double> testVal = new Dictionary<int, double>();
        private int index = 0;
        private int indexCount = 0;
        public MainWindow()
        {
            InitializeComponent();
            ThreadFlag = true;
            btnStart.Click += BtnStart_Click;
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (localThread != null)
            {
                ThreadFlag = false;
                WorkFlag = false;
                localThread.Join();
                localThread = null;
            }
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ReadData();
            localThread = new Thread(MOnitor);
            localThread.Start();
            this.Loaded -= MainWindow_Loaded;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (btnStart.Content.ToString() == "开始")
            {
                WorkFlag = true;
                btnStart.Content = "暂停";
            }
            else
            {
                WorkFlag = false;
                btnStart.Content = "开始";
            }
        }

        //数据监控线程函数
        private void MOnitor()
        {
            while (ThreadFlag)
            {
                while (WorkFlag)
                {
                    index++;
                    if (index < 200)
                        continue;
                    index = index % indexCount;
                    WaveLine.Add(testVal[index]);
                    Thread.Sleep(600);
                }
            }

        }
        //读取测试数据
        public void ReadData()
        {
            string fileName = Directory.GetCurrentDirectory() + "\\testValue.xml";
            try
            {
                if (File.Exists(fileName))
                {
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    XmlSerializer ser = new XmlSerializer(testValues.GetType());
                    testValues = ser.Deserialize(fs) as List<double>;
                    indexCount = testValues.Count;
                    for (int i = 0; i < indexCount; i++)
                    {
                        testVal[i] = testValues[i];
                    }
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("读取文件失败:" + e.Message);
            }
            return;
        }
    }
}
