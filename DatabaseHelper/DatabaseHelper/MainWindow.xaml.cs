using DatabaseHelper.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace DatabaseHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            meission();
        }
        private void addMsg(object str)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ls_box.Items.Insert(0, str);
            }));
        }

        public void meission()
        {
            //打开数据库
            if (!OpenDatabase("MaxwellDatabase.db"))
                return;

            //获取数据
            if (!analyseCsvFile("warningPlcData.csv"))
                return;

            ////修改数据库文件
            //if (!replaceDatabase("MaxwellDatabase.db", "AlarmLookupTab"))
            //    return;

            ////补充300空文件
            //if (writeEmptyDateToDatabase("MaxwellDatabase.db"))
            //    return;

            //写入数据库文件
            if (!writeToDatabase("MaxwellDatabase.db"))
                return;

            addMsg($"ok");
        }
        List<List<object>> lsRawData = new List<List<object>>();

        public bool OpenDatabase(string dbName)
        {
            try
            {
                string fullPath = Path.Combine(RootPath.Root, "DataFIle");
                SQLiteHelper.Instance.CreateDB(fullPath, dbName);

                bool flag = SQLiteHelper.Instance.IsExisTable("AlarmLookupTab");
                addMsg($"是否存在AlarmLookupTab表:{flag}");
                return flag;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool analyseCsvFile(string csvFileName)
        {
            try
            {
                string fileName = Path.Combine(RootPath.Root, "DataFIle", csvFileName);
                addMsg(fileName);
                lsRawData = CsvHelper.Instance.ReadNoHeaderFile(fileName);
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool writeToDatabase(string dbName)
        {
            try
            {
                if (!SQLiteHelper.Instance.ReadyInsertTable("AlarmLookupTab"))
                {
                    return false;
                } 
                foreach (var item in lsRawData)
                {
                    SQLiteHelper.Instance.ExecuteInsertTableValue(item);
                }
                SQLiteHelper.Instance.FinishInsertTableValue();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool writeEmptyDateToDatabase(string dbName)
        {
            try
            {
                List<object> lsobj = new List<object>();
                lsobj.Add(0);
                lsobj.Add("Null");
                lsobj.Add("Reason");
                lsobj.Add("Solution");
                lsobj.Add("Device");
                lsobj.Add("Alarm");
                for (int i = 37; i <= 300; i++)
                {
                    lsobj[0] = i;
                    SQLiteHelper.Instance.AtomInsertTableValue("AlarmLookupTab", lsobj);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool replaceDatabase(string dbName,string tabName)
        {
            try
            {
             
                foreach (var item in lsRawData)
                {
                    SQLiteHelper.Instance.ExecuteInsertTableValue(item);
                    SQLiteHelper.Instance.UpdateTableValue(tabName, "AlarmMessage", item[1], "AlarmID", item[0]);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
