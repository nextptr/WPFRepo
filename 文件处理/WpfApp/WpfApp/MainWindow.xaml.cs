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
        public void NotifyOfPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DelegateCommand ClickCommand { get; set; }
        public DelegateCommand PlcClickCommand { get; set; }

        private bool hasFile = false;
        private bool hasHtmlFile = false;

        public bool HasFile
        {
            get { return hasFile; }
            set
            {
                hasFile = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasFile)));
            }
        }
        public bool HasHtmlFile
        {
            get { return hasHtmlFile; }
            set
            {
                hasHtmlFile = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasHtmlFile)));
            }
        }

        private string modeFile = "";
        private string editFile = "";
        private bool canGroup = false;

        public string ModeFile
        {
            get { return modeFile; }
            set
            {
                modeFile = value;
                NotifyOfPropertyChange(nameof(ModeFile));
            }
        }
        public string EditFile
        {
            get { return editFile; }
            set
            {
                editFile = value;
                CanGroup = ModeFile != "" & EditFile != "";
                NotifyOfPropertyChange(nameof(EditFile));
            }
        }
        public bool CanGroup
        {
            get { return canGroup; }
            set
            {
                canGroup = value;
                NotifyOfPropertyChange(nameof(CanGroup));
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            ClickCommand = new DelegateCommand(Button_Click);
            PlcClickCommand = new DelegateCommand(PlcButton_Click);
            ExportToExcelHelper.Instance.msg = addMsg;
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
                case "t5":
                    break;
                default:
                    break;
            }
        }
        public void PlcButton_Click(object obj)
        {
            try
            {
                string arg = (string)obj;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                switch (arg)
                {
                    case "t1":
                        if (openFileDialog1.ShowDialog().Value)
                        {
                            fileFullPath = openFileDialog1.FileName;
                            fileName = openFileDialog1.SafeFileName;
                            filePath = fileFullPath.TrimEnd(fileName.ToCharArray());
                            addMsg(fileFullPath);
                            ExportToExcelHelper.Instance.ReadPlc(fileFullPath, Path.Combine(filePath, "New" + fileName.Replace("xlsx", "txt")));
                        }
                        break;
                    case "t2":
                        int count = 0;
                        int.TryParse(CountLab.Text.ToString(), out count);
                        if (count==0)
                        {
                            MessageBox.Show("请输入数据长度");
                            return;
                        }
                        FileAnalyseHelper.Instance.CreateAlarmTxt(count);
                        addMsg("文件已生成");
                        break;
                    case "t3":
                        break;
                    case "t10":
                        if (openFileDialog1.ShowDialog().Value)
                        {
                            fileFullPath = openFileDialog1.FileName;
                            fileName = openFileDialog1.SafeFileName;
                            filePath = fileFullPath.TrimEnd(fileName.ToCharArray());
                            if (!File.Exists(Path.Combine(filePath, "MaxwellDatabase.db")))
                            {
                                addMsg("选中路径中不存在MaxwellDatabase.db数据库文件");
                                return;
                            }
                            addMsg(fileFullPath);
                            DateTime staTim = DateTime.Now;
                            ExportToExcelHelper.Instance.AddPcAlarmDatabase(fileFullPath, filePath);
                            DateTime endTim = DateTime.Now;
                            var d = endTim - staTim;
                            addMsg($"报警文件处理完成，用时:{d.Minutes}分{d.Seconds}秒");
                        }
                        break;

                    case "t11":
                        if (openFileDialog1.ShowDialog().Value)
                        {
                            fileFullPath = openFileDialog1.FileName;
                            fileName = openFileDialog1.SafeFileName;
                            filePath = fileFullPath.TrimEnd(fileName.ToCharArray());
                            if (!File.Exists(Path.Combine(filePath, "MaxwellDatabase.db")))
                            {
                                addMsg("选中路径中不存在MaxwellDatabase.db数据库文件");
                                return;
                            }
                            addMsg(fileFullPath);
                            DateTime staTim = DateTime.Now;
                            ExportToExcelHelper.Instance.CreateAlarmDatabase(fileFullPath, filePath);
                            DateTime endTim = DateTime.Now;
                            var d = endTim - staTim;
                            addMsg($"报警文件处理完成，用时:{d.Minutes}分{d.Seconds}秒");
                        }
                        break;
                    case "g1":

                        break;
                    case "g2":

                        break;
                    case "g3":
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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

    }

    public class ScanSettingItem
    {
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
