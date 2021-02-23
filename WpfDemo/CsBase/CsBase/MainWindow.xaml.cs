using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using CsBase.Common;
using System.IO;

namespace CsBase
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow:Window
    {
        protected classBase obj_instance = null;
        protected int selectClassIndex = 0;
        protected int selectClassUnitIndex = 0;
        protected int tableIndex = 0;
        protected DataItem readItem = null;
        protected DataItem writItem = null;

        public delegate void ddMsgEventHandle(string msg);
        public ddMsgEventHandle ddMsgEvent;

        public MainWindow()
        {
            InitializeComponent();
            TitleBar.MouseMove += MainWindow_MouseMove;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ddMsgEvent += ShowMsg;

            textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            tabNote.PreviewMouseDown += Tab_PreviewMouseDown;
            tabCode.PreviewMouseDown += Tab_PreviewMouseDown;
            tabRun.PreviewMouseDown += Tab_PreviewMouseDown;
            BtnClose.Click += BtnClose_Click;
            BtnMin.Click += BtnMin_Click;
            foreach (var cla in treeRoot.Items)
            {
                TreeViewItem tmp = cla as TreeViewItem;
                foreach (var uit in tmp.Items)
                {
                    TreeViewItem tmpUnit = uit as TreeViewItem;
                    tmpUnit.Selected += treeUnit_Selected;
                }
            }
            string fullPath = Directory.GetCurrentDirectory();
            string filePath = fullPath.Remove(fullPath.Length - 10, 10);
            SQLiteHelper.Instance.CreateDB(filePath, "CsBase.db");
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e) 
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e) //富文本控件的按钮事件筛选
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.S) && Keyboard.Modifiers == ModifierKeys.Control)
            {
                char[] trim = { '\r', '\n' };
                writItem.Remark = GetRichText(textBox).TrimEnd(trim);
                if (string.Compare(writItem.Remark, readItem.Remark, false) != 0)
                {//保存数据
                    writItem.TableName = readItem.TableName;
                    writItem.Unit = readItem.Unit;
                    SQLiteHelper.Instance.UpdateTableValue(writItem);
                }
            }
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter) && Keyboard.Modifiers == ModifierKeys.Shift)
            {
                //textBox.AppendText("\n");
                //TextPointer sta = textBox.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);
                //if (sta != null)
                //{
                //    textBox.CaretPosition = sta/*.GetPositionAtOffset(1)*/;
                //}
                //e.Handled = true;

            textBox.CaretPosition.InsertLineBreak();
                TextPointer sta = textBox.CaretPosition.GetLineStartPosition(1);
                if (sta != null)
                {
                    textBox.AppendText($"   >");
                    textBox.CaretPosition = sta.GetPositionAtOffset(5);
                }
                e.Handled = true;
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter))
            {
                //textBox.CaretPosition.InsertLineBreak();
                //TextPointer sta = textBox.CaretPosition.GetLineStartPosition(1);
                //if (sta != null)
                //{
                //    textBox.AppendText($"   >");
                //    textBox.CaretPosition = sta.GetPositionAtOffset(5);
                //}
                //e.Handled = true;
            }
        }
        private void ShowMsg(string str)  //运行结果显示
        {
            lsRun.Items.Add(str);
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (readItem == null)
            {
                this.Close();
            }
            else
            {
                //上一次查看是否修改了数据
                char[] trim = { '\r', '\n' };
                writItem.Remark = GetRichText(textBox).TrimEnd(trim);
                if (string.Compare(writItem.Remark, readItem.Remark, false) != 0)
                {//写数据
                    writItem.TableName = readItem.TableName;
                    writItem.Unit = readItem.Unit;
                    SQLiteHelper.Instance.UpdateTableValue(writItem);
                }
            }
            this.Close();
        }

        private void Tab_PreviewMouseDown(object sender, MouseButtonEventArgs e) //tab窗口切换按钮事件
        {
            TabItem tab = sender as TabItem;
            if (tab != null)
            {
                if (tab.Name == "tabNote")
                {
                    tableIndex = 0;
                }
                if (tab.Name == "tabCode")
                {
                    tableIndex = 1;
                }
                if (tab.Name == "tabRun")
                {
                    tableIndex = 2;
                }
                ChoiseAnUnit(tableIndex);
            }
        }
        private void treeUnit_Selected(object sender, RoutedEventArgs e)         //树形控件点击事件
        {
            //切换单元显示时需要判断是否有对当前内容做修改，是否需要保存修改
            if (this.IsVisible == true)
            {
                TreeViewItem item = sender as TreeViewItem;
                string[] arr = item.Name.Split('_');
                int cId = int.Parse(arr[0].Remove(0, 4));
                int uId = int.Parse(arr[1]);

                if (readItem == null)
                {
                    readItem = new DataItem();
                    writItem = new DataItem();
                    readItem.TableName = "Class" + cId;
                    readItem.Unit = uId;
                    SQLiteHelper.Instance.GetTableValue(ref readItem);
                    LoadTextFile(textBox, readItem.Remark);
                }
                else
                {
                    //查看是否修改了数据
                    char[] trim = { '\r', '\n' };
                    writItem.Remark = GetRichText(textBox).TrimEnd(trim);
                    if (string.Compare(writItem.Remark, readItem.Remark, false) != 0)
                    {//保存数据
                        writItem.TableName = readItem.TableName;
                        writItem.Unit = readItem.Unit;
                        SQLiteHelper.Instance.UpdateTableValue(writItem);
                    }

                    //加载新数据
                    readItem.TableName = "Class" + cId;
                    readItem.Unit = uId;
                    SQLiteHelper.Instance.GetTableValue(ref readItem);
                    LoadTextFile(textBox, readItem.Remark);
                }

                //更新显示
                selectClassIndex = cId;
                selectClassUnitIndex = uId;
                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
                dynamic obj = assembly.CreateInstance("CsBase.Class" + cId + ".Class" + cId + '_' + uId);
                classBase classobj = (classBase)obj;
                obj_instance = classobj;
                ChoiseAnUnit(tableIndex);
            }
        }

        private void ChoiseAnUnit(int tab) //单元选择
        {
            if (obj_instance == null)
            {
                return;
            }
            if (tab == 1)
            {//源代码
                ShowOriCode();
            }
            else if (tab == 2)
            {//运行
                ShowRunCode();
            }
        }
        private void ShowOriCode()         //显示源码
        {
            //找到对应的Class.cs文件
            //"D:\\代码库\\WPF\\CsBase\\CsBase\\bin\\Debug"
            string fullPath = Directory.GetCurrentDirectory();
            string filePath = fullPath.Remove(fullPath.Length - 10, 10)+ "\\Class" + selectClassIndex;
            string[] arr= Directory.GetFiles(filePath);
            string objFile = "Class" + selectClassIndex + '_' + selectClassUnitIndex + ".cs";
            string path = filePath + "\\" + objFile;
            //解析Class.cs文件,打印特定区域
            string tmp;
            bool flag = false;
            int spaceCount = 0;
            lsCode.Items.Clear();
            if (!File.Exists(path))
            {
                return;
            }
            else
            {
                FileStream fs = File.OpenRead(path);
                StreamReader rd = new StreamReader(fs);
                while (!rd.EndOfStream)
                {
                    tmp = rd.ReadLine();
                    if (tmp.Contains("#region codeStart"))
                    {
                        spaceCount = tmp.Length - 17;
                        flag = true;
                        continue;
                    }
                    if (tmp.Contains("#endregion codeEnd"))
                    {
                        flag = false;
                    }
                    if (flag)
                    {
                        if (tmp.Length >= spaceCount)
                        {
                            if (tmp.Contains("ddh("))
                            {
                                ListViewItem item = new ListViewItem();
                                item.Content = tmp.Remove(0, spaceCount);
                                item.Foreground = Brushes.Red;
                                lsCode.Items.Add(item);
                            }
                            else if (tmp.Contains(@"//"))
                            {
                                ListViewItem item = new ListViewItem();
                                item.Content = tmp.Remove(0, spaceCount);
                                item.Foreground = Brushes.Green;
                                lsCode.Items.Add(item);
                            }
                            else
                            {
                                lsCode.Items.Add(tmp.Remove(0, spaceCount));
                            }
                        }
                        else
                        {
                            lsCode.Items.Add(" ");
                        }
                    }
                }
                rd.Close();
                fs.Close();
            }
        }
        private void ShowRunCode()         //显示运行结果
        {
            lsRun.Items.Clear();
            obj_instance.runList.Clear();
            obj_instance.RunTest();
            foreach (StringItem item in obj_instance.runList)
            {
                ListViewItem tm = new ListViewItem();
                tm.Content = item.Str;
                if (item.Type == StringType.Title)
                {
                    tm.Foreground = Brushes.Red;
                }
                lsRun.Items.Add(tm);
            }
        }

        private void LoadTextFile(RichTextBox richTextBox, string data) //富文本内容加载
        {
            richTextBox.Document.Blocks.Clear();
            Paragraph p = new Paragraph();
            Run r = new Run(data);
            p.Inlines.Add(r);
            richTextBox.Document.Blocks.Add(p);
        }
        private string GetRichText(RichTextBox richTextBox)             //富文本内容获取
        {
            TextRange a = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            //byte[] byteArray = Encoding.Default.GetBytes(a.Text); //string转byte
            //string str = Encoding.Default.GetString(byteArray);   //byte转string
            //MessageBox.Show(str+":"+byteArray.Length);
            return a.Text;
        }
    }
}
