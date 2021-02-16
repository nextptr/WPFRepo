using Sqlite;
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

namespace SqliteDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        protected int rowCount = 0;

        public MainWindow()
        {
            InitializeComponent();
            string fullPath = Directory.GetCurrentDirectory();
            string filePath = fullPath.Remove(fullPath.Length - 10, 10);
            SQLiteHelper.Instance.CreateDB(filePath, "sqtest.db");
            SQLiteHelper.Instance.CreateTabel("class1");
            SQLiteHelper.Instance.CreateTabel("class2");
            SQLiteHelper.Instance.CreateTabel("class3");
            TableInfo tableInfo = SQLiteHelper.Instance.GetTableInfo("class1");
            List<TableInfo> tableInfos = SQLiteHelper.Instance.GetTableInfos();
            List<string> ls = SQLiteHelper.Instance.GetSqlstr();
            bool flag = SQLiteHelper.Instance.IsExisTable("class1");
            flag = SQLiteHelper.Instance.IsExisTableField("class2", "unit");
            flag = SQLiteHelper.Instance.IsExisTableField("class2", "unit1");
            flag = SQLiteHelper.Instance.IsExisTableField("class1", "unit");
            SQLiteHelper.Instance.InsertTableValue("class1", 1, "test1");
            SQLiteHelper.Instance.InsertTableValue("class1", 1, "test11");
            SQLiteHelper.Instance.InsertTableValue("class1", 1, "test111");
            SQLiteHelper.Instance.InsertTableValue("class1", 1, "test1111");
            SQLiteHelper.Instance.InsertTableValue("class1", 2, "test2");
            SQLiteHelper.Instance.InsertTableValue("class1", 2, "test22");
            SQLiteHelper.Instance.InsertTableValue("class1", 2, "test222");
            SQLiteHelper.Instance.InsertTableValue("class1", 3, "test3");
            SQLiteHelper.Instance.InsertTableValue("class2", 7, "test7");
            SQLiteHelper.Instance.InsertTableValue("class2", 7, "test7");
            SQLiteHelper.Instance.InsertTableValue("class2", 7, "test7");
            SQLiteHelper.Instance.UpdateTableValue("class1", 1, 3, "修改测试");
            ls = SQLiteHelper.Instance.GeTableItemValue("class1", 1);
            {
                SQLiteHelper.Instance.CreateTabelNoKey("class4");
                SQLiteHelper.Instance.InsertTableValueNokey("class4", 1, "test1");
                SQLiteHelper.Instance.InsertTableValueNokey("class4", 1, "test2");
                SQLiteHelper.Instance.InsertTableValueNokey("class4", 1, "test3");
                SQLiteHelper.Instance.InsertTableValueNokey("class4", 1, "test4");
                SQLiteHelper.Instance.InsertTableValueNokey("class4", 1, "test5");
                SQLiteHelper.Instance.InsertTableValueNokey("class4", 2, "test1");
                SQLiteHelper.Instance.InsertTableValueNokey("class4", 2, "test2");
                SQLiteHelper.Instance.InsertTableValueNokey("class4", 2, "test3");
                ls = SQLiteHelper.Instance.GeTableItemValue("class4", 1);
                SQLiteHelper.Instance.DeleteTableValueNokey("class4", 1, 3);
                ls = SQLiteHelper.Instance.GeTableItemValue("class4", 1);
            }

            richTextBox.PreviewKeyDown += RichTextBox_KeyDown;

        }
        private void RichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter) && Keyboard.Modifiers == ModifierKeys.Shift)
            {
                rowCount = 0;
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter))
            {
                e.Handled = true;
                rowCount++;

                richTextBox.CaretPosition.InsertLineBreak();
                Paragraph para = richTextBox.CaretPosition.Paragraph;
                TextPointer sta = richTextBox.CaretPosition.GetLineStartPosition(1);
                richTextBox.AppendText($"   {rowCount}、");
                richTextBox.CaretPosition = sta.GetPositionAtOffset(6);
            }
            //richTextBox.CaretPosition = richTextBox.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);
            //richTextBox.Focus();
        }
    }
}
