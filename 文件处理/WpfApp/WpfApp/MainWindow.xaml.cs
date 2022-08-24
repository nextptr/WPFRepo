using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand ClickCommand { get; set; }
        private bool hasFile = false;
        public bool HasFile
        {
            get { return hasFile; }
            set
            {
                hasFile = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasFile)));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            ClickCommand = new DelegateCommand(Button_Click);
            this.DataContext = this;
        }
        private void addMsg(string str)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ls_box.Items.Insert(0, str);
            }));
        }

        private string filePath;
        private string fileName;
        private string fileFullPath;
        private void openFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog().Value)
            {
                fileFullPath = openFileDialog1.FileName;
                fileName = openFileDialog1.SafeFileName;
                filePath = fileFullPath.TrimEnd(fileName.ToCharArray());
                addMsg(fileFullPath);
                HasFile = true;
            }
        }
        public void Button_Click(object obj)
        {
            string arg = (string)obj;
            switch (arg)
            {
                case "clear":
                    ls_box.Items.Clear();
                    break;
                case "t1":
                    openFile();
                    break;
                case "t2":
                    cutLine();
                    break;
                case "t3":
                    clearStepData();
                    break;
                case "t4":
                    clearHtmlData();
                    break;
                default:
                    break;
            }
        }

        private void cutLine()
        {
            FileStream fs = new FileStream(fileFullPath, FileMode.Open);
            FileStream fw = new FileStream(Path.Combine(filePath, "New" + fileName), FileMode.Create);
            StreamReader sr = new StreamReader(fs);
            StreamWriter sw = new StreamWriter(fw);
            string line = "";
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Length > 55)
                {
                    var s1 = line.Substring(0, 55);
                    line = line.Remove(0, 55);
                    sw.WriteLine(s1);
                    if (line.Length < 50)
                    {
                        sw.WriteLine("    " + line);
                        continue;
                    }
                    do
                    {
                        s1 = "    " + line.Substring(0, 50);
                        line = line.Remove(0, 50);
                        sw.WriteLine(s1);
                        if (line.Length < 50)
                        {
                            sw.WriteLine("    " + line);
                            break;
                        }
                    } while (true);
                }
                else
                {
                    sw.WriteLine(line);
                }
            }
            sw.Close();
            sr.Close();
            fw.Close();
            fs.Close();
            addMsg("全部修改位55字符长度");
        }
        private void clearStepData()
        {
            FileStream fs = new FileStream(fileFullPath, FileMode.Open);

            string nam = fileName.Replace("txt", "csv");
            FileStream fw = new FileStream(Path.Combine(filePath, "New" + nam), FileMode.Create);
            StreamReader sr = new StreamReader(fs);
            StreamWriter sw = new StreamWriter(fw);

            List<string> buffer1 = new List<string>();
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            string line = "";
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("各流程"))
                {
                    buffer1.Add(line.Replace(" #", "#"));
                }
            }

            string head="";
            foreach (var item in buffer1)
            {
                head = "时间";
                var arr = item.Split('#');
                string ky = arr[0].Substring(0, 19);
                dic[ky] = new Dictionary<string, string>();
                for (int i = 1; i < arr.Length; i++)
                {
                    var tmp = arr[i].Split('*');
                    dic[ky][tmp[0]] = tmp[1];
                    head += $",{tmp[0]}";
                }
            }

            string str;
            sw.WriteLine(head);
            foreach (var ky in dic.Keys)
            {
                str = ky;
                foreach (var item in dic[ky].Values)
                {
                    str += $",{item}";
                }
                sw.WriteLine(str);
            }
            sw.Close();
            sr.Close();
            fw.Close();
            fs.Close();
            addMsg("数据清理完成");
        }

        private void clearHtmlData()
        {
            FileStream fs = new FileStream(fileFullPath, FileMode.Open);
            FileStream fw = new FileStream(Path.Combine(filePath, "New" + fileName), FileMode.Create);
            StreamReader sr = new StreamReader(fs);
            StreamWriter sw = new StreamWriter(fw);

            string line = "";
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                var arr = RegxPait(line);
                foreach (var item in arr)
                {
                    sw.WriteLine($"{item.Key}***{item.Value}");
                }
            }
            sw.Close();
            sr.Close();
            fw.Close();
            fs.Close();
            addMsg("数据清理完成");
        }
        public static List<KeyValuePair<string, string>> RegxPait(string input)
        {
            List<KeyValuePair<string, string>> ls = new List<KeyValuePair<string, string>>();
            var arr = Regex.Split(input, @"<tr>");
            string tmpKey = "";
            string tmpVal = "";
            foreach (var item in arr)
            {
                var mach = Regex.Matches(item, @"(?<=((<td).*>))[^<>]*(?=</td>)");
                for (int i = 0; i < mach.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        tmpKey = mach[i].ToString();
                    }
                    else
                    {
                        tmpVal = mach[i].ToString();
                        ls.Add(new KeyValuePair<string, string>(tmpKey, tmpVal));
                    }
                }
            }
            return ls;
        }

        #region old
        private void Btn_motion_analyse_Click(object sender, RoutedEventArgs e)
        {
            FileAnalyseHelper hp = new FileAnalyseHelper();
            filePathObj path = hp.SelectFileDialog();
            addMsg("开始分析:" + path.fileName);
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
        private void Btn_dot_count_Click(object sender, RoutedEventArgs e)
        {
            string rawData = File.ReadAllText(@"D:\GitWell\WPFRepo\文件处理\WpfApp\WpfApp\bin\x64\Debug\Script\RawDot.txt");
            string countData = @"D:\GitWell\WPFRepo\文件处理\WpfApp\WpfApp\bin\x64\Debug\Script\CountData.txt";
            string[] datas = rawData.Split('\r');
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = datas[i].TrimStart('\n');
            }
            string[] dot;

            Dictionary<string, int> dic = new Dictionary<string, int>();

            foreach (var item in datas)
            {
                if (dic.ContainsKey(item))
                {
                    dic[item]++;
                }
                else
                {
                    dic.Add(item, 1);
                }
            }

            FileStream fs = new FileStream(countData, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            foreach (var item in dic.Keys)
            {
                sw.WriteLine(item + "  Count:  " + dic[item].ToString());
            }
            sw.Close();
            fs.Close();
            addMsg("Dot计数完成");
        }
        private void Btn_file_clean_Click(object sender, RoutedEventArgs e)
        {
            string rawData = File.ReadAllText(@"D:\GitWell\WPFRepo\文件处理\WpfApp\WpfApp\bin\x64\Debug\Script\Log-20220219.txt");
            string cleantData = @"D:\GitWell\WPFRepo\文件处理\WpfApp\WpfApp\bin\x64\Debug\Script\Log-20220219Clean.txt";
            string[] datas = rawData.Split('\r');


            FileStream fs = new FileStream(cleantData, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < datas.Length; i++)
            {
                if (!datas[i].Contains("DriverLib"))
                {
                    sw.WriteLine(datas[i].TrimStart('\n'));
                }
            }
            sw.Close();
            fs.Close();
            addMsg("数据清洗完成");
        }
        private void fileClean()
        {
            try
            {
                string SrcFile = DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string DstFile = DateTime.Now.ToString("yyyyMMdd") + "Clean.txt";

                string rawData = File.ReadAllText(@"E:\SMK\Release\SMKTraceLogs\Log-" + SrcFile);
                string cleantData = @"E:\SMK\Release\SMKTraceLogs\Log-" + DstFile;
                string[] datas = rawData.Split('\r');


                FileStream fs = new FileStream(cleantData, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                for (int i = 0; i < datas.Length; i++)
                {
                    if (!datas[i].Contains("DriverLib"))
                    {
                        sw.WriteLine(datas[i].TrimStart('\n'));
                    }
                }
                sw.Close();
                fs.Close();
            }
            catch
            {

            }
        }


        //VVVVVVVVVVVVVV脚本处理
        private void Btn_script_dot_Click(object sender, RoutedEventArgs e)
        {
            addMsg("开始Dot处理");
            string pth = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Script/RawScript.txt");
            string ori = File.ReadAllText(pth);
            ScanSettingItem sett = new ScanSettingItem();
            string nw = proc(ori, sett, BodyEditDotTo100, GeneralEdit);

            string wpth = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Script/NewScript.txt");
            FileStream fs = new FileStream(wpth, FileMode.OpenOrCreate);
            StreamWriter wfs = new StreamWriter(fs);
            wfs.Write(nw);
            wfs.Close();
            fs.Close();
            addMsg("Dot处理结束");
        }

        private string proc(string rawData, ScanSettingItem generalSetting, Func<string, string> bodyEdit, Func<string, ScanSettingItem, string> generalEdit)
        {
            //1.文本解析
            bool flag = true;
            int stIndex = 0;
            int edIndex = 0;
            char[] arr = rawData.ToCharArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if (flag
                    && i + 4 < arr.Length
                    && arr[i] == 'I'
                    && arr[i + 1] == 'm'
                    && arr[i + 2] == 'a'
                    && arr[i + 3] == 'g'
                    && arr[i + 4] == 'e'
                    )
                {
                    stIndex = i;
                    flag = false;
                }

                if (i + 2 < arr.Length
                  && arr[i] == 'e'
                  && arr[i + 1] == 'n'
                  && arr[i + 2] == 'd'
                  )
                {
                    edIndex = i;
                    break;
                }
            }

            //2.函数体
            string body = rawData.Substring(stIndex, edIndex - stIndex);
            string reverseBody = bodyEdit(body);
            //3.函数头
            string strHead = rawData.Substring(0, stIndex);

            //4.函数尾
            string strTail = rawData.Substring(edIndex);
            strTail = generalEdit(strTail, generalSetting);
            //5.组装
            string reverseData = strHead + reverseBody + strTail;
            return reverseData;
        }

        private string BodyEditDotTo100(string rawStr)
        {
            string[] dots = rawStr.Split('\n');
            string[] buf = new string[dots.Length];
            dots[0] = "\t" + dots[0];
            for (int i = 0; i < dots.Length; i++)
            {
                buf[i] = dots[i].Replace("1000)", "100)");
            }
            string reverseBody = string.Join("\n", buf).TrimStart("\n\n\t".ToCharArray());
            return reverseBody;
        }

        private string GeneralEdit(string rawStr, ScanSettingItem param)
        {
            string[] dots = rawStr.Split('\n');
            string[] buf = new string[dots.Length];
            dots[0] = "\t" + dots[0];
            for (int i = 0; i < dots.Length; i++)
            {
                if (dots[i].Contains("Laser.Power"))
                {
                    buf[i] = $"Laser.Power = {Math.Round(param.Power, 3)}";
                }
                else if (dots[i].Contains("Laser.MarkSpeed"))
                {
                    buf[i] = $"Laser.MarkSpeed = {param.MarkSpeed}";
                }
                else if (dots[i].Contains("Laser.JumpSpeed"))
                {
                    buf[i] = $"Laser.JumpSpeed = {param.JumpSpeed}";
                }
                else if (dots[i].Contains("Laser.JumpDelay"))
                {
                    buf[i] = $"Laser.JumpDelay = {param.JumpDelay}";
                }
                else if (dots[i].Contains("Laser.LaserOnDelay"))
                {
                    buf[i] = $"Laser.LaserOnDelay = {param.LaserOnDelay}";
                }
                else if (dots[i].Contains("Laser.LaserOffDelay"))
                {
                    buf[i] = $"Laser.LaserOffDelay = {param.LaserOffDelay}";
                }
                else if (dots[i].Contains("Laser.MarkDelay"))
                {
                    buf[i] = $"Laser.MarkDelay = {param.MarkDelay}";
                }
                else if (dots[i].Contains("Laser.PolyDelay"))
                {
                    buf[i] = $"Laser.PolyDelay = {param.PolyDelay}";
                }
                else if (dots[i].Contains("Laser.Frequency"))
                {
                    buf[i] = $"Laser.Frequency = {param.Frequency}";
                }
                else if (dots[i].Contains("Laser.DutyCycle1"))
                {
                    buf[i] = $"Laser.DutyCycle1 = {param.DutyCycle1}";
                }
                else if (dots[i].Contains("Laser.DutyCycle2"))
                {
                    buf[i] = $"Laser.DutyCycle2 = {param.DutyCycle2}";
                }
                else
                {
                    buf[i] = dots[i];
                }
            }
            string reverseBody = string.Join("\n", buf);
            return reverseBody;
        }

        private string reverseScrewScript(string rawData)
        {
            bool flag = true;
            int stIndex = 0;
            int edIndex = 0;
            char[] arr = rawData.ToCharArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if (flag
                    && i + 4 < arr.Length
                    && arr[i] == 'I'
                    && arr[i + 1] == 'm'
                    && arr[i + 2] == 'a'
                    && arr[i + 3] == 'g'
                    && arr[i + 4] == 'e'
                    )
                {
                    stIndex = i;
                    flag = false;
                }

                if (i + 2 < arr.Length
                  && arr[i] == 'e'
                  && arr[i + 1] == 'n'
                  && arr[i + 2] == 'd'
                  )
                {
                    edIndex = i;
                    break;
                }
            }

            string body = rawData.Substring(stIndex, edIndex - stIndex);

            string[] dots = body.Split('\n');
            string[] buf = new string[dots.Length];
            dots[0] = "\t" + dots[0];
            for (int i = 0; i < dots.Length; i++)
            {
                buf[i] = dots[dots.Length - i - 1];
            }
            string reverseBody = string.Join("\n", buf).TrimStart("\n\n\t".ToCharArray());

            string strHead = rawData.Substring(0, stIndex);
            string strTail = "\n\n" + rawData.Substring(edIndex);
            string reverseData = strHead + reverseBody + strTail;
            return reverseData;
        }
        private string repeatScrewScript(string rawData)
        {
            bool flag = true;
            int stIndex = 0;
            int edIndex = 0;
            char[] arr = rawData.ToCharArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if (flag
                    && i + 4 < arr.Length
                    && arr[i] == 'I'
                    && arr[i + 1] == 'm'
                    && arr[i + 2] == 'a'
                    && arr[i + 3] == 'g'
                    && arr[i + 4] == 'e'
                    )
                {
                    stIndex = i;
                    flag = false;
                }

                if (i + 2 < arr.Length
                  && arr[i] == 'e'
                  && arr[i + 1] == 'n'
                  && arr[i + 2] == 'd'
                  )
                {
                    edIndex = i;
                    break;
                }
            }

            string body = rawData.Substring(stIndex, edIndex - stIndex);
            string[] dots = body.Split('\n');
            string[] buf = new string[dots.Length];
            dots[0] = "\t" + dots[0];
            for (int i = 0; i < dots.Length; i++)
            {
                if (i % 2 == 1)
                {
                    string[] rawStr = dots[i].Split('(');
                    if (rawStr.Length < 2)
                    {
                        //buf[i] = dots[i];
                        continue;
                    }
                    else
                    {
                        string head = rawStr[0];
                        string tail = rawStr[1];
                        string[] rawArry = tail.Split(',');
                        //0 1  3 4
                        string st1 = rawArry[0];
                        string st2 = rawArry[1];
                        rawArry[0] = rawArry[3];
                        rawArry[1] = rawArry[4];
                        rawArry[3] = st1;
                        rawArry[4] = st2;

                        string newStr = head + '(' + string.Join(",", rawArry);
                        buf[i] = newStr;
                    }
                }
                else
                {
                    buf[i] = dots[i];
                }
            }
            string reverseBody = string.Join("\n", buf).TrimStart("\n\n\t".ToCharArray());

            string strHead = rawData.Substring(0, stIndex);
            string strTail = rawData.Substring(edIndex);
            string reverseData = strHead + reverseBody + strTail;
            return reverseData;
        }
        #endregion
    }

    public class ScanSettingItem
    {
        //private double _power;
        //private int _markSpeed = 1000;
        //private int _jumpSpeed = 2000;
        //private int _jumpDelay = 100;
        //private int _laserOnDelay = 50;
        //private int _laserOffDelay = 75;
        //private int _markDelay = 100;
        //private int _polyDelay = 50;
        //private int _frequency;
        //private int _dutyCycle1;
        //private int _dutyCycle2;

        private double _power = 1;
        private int _markSpeed = 2;
        private int _jumpSpeed = 3;
        private int _jumpDelay = 4;
        private int _laserOnDelay = 5;
        private int _laserOffDelay = 6;
        private int _markDelay = 7;
        private int _polyDelay = 8;
        private int _frequency = 9;
        private int _dutyCycle1 = 10;
        private int _dutyCycle2 = 11;

        public double Power
        {
            get { return _power; }
            set
            {
                _power = value;
            }
        }
        public int MarkSpeed
        {
            get { return _markSpeed; }
            set { if (_markSpeed != value) { _markSpeed = value; } }
        }
        public int JumpSpeed
        {
            get { return _jumpSpeed; }
            set { if (_jumpSpeed != value) { _jumpSpeed = value; } }
        }
        public int JumpDelay
        {
            get { return _jumpDelay; }
            set { if (_jumpDelay != value) { _jumpDelay = value; } }
        }
        public int LaserOnDelay
        {
            get { return _laserOnDelay; }
            set { if (_laserOnDelay != value) { _laserOnDelay = value;  } }
        }
        public int LaserOffDelay
        {
            get { return _laserOffDelay; }
            set { if (_laserOffDelay != value) { _laserOffDelay = value; } }
        }
        public int MarkDelay
        {
            get { return _markDelay; }
            set { if (_markDelay != value) { _markDelay = value; } }
        }
        public int PolyDelay
        {
            get { return _polyDelay; }
            set { if (_polyDelay != value) { _polyDelay = value; } }
        }
        public int Frequency
        {
            get { return _frequency; }
            set
            {
                _frequency = value;
            }
        }
        public int DutyCycle1
        {
            get { return _dutyCycle1; }
            set
            {
                _dutyCycle1 = value;
            }
        }
        public int DutyCycle2
        {
            get { return _dutyCycle2; }
            set
            {
                _dutyCycle2 = value;
            }
        }

        public ScanSettingItem()
        {

        }
    }
}
