using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace DatabaseHelper.Common
{
    public class CsvHelper
    {
        private CsvHelper()
        { }
        private static CsvHelper _instance = null;
        public static CsvHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CsvHelper();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        private Dictionary<object, List<object>> _buffer = new Dictionary<object, List<object>>();


        private string _saveDataFoldPath = RootPath.Root + @"\CsvFile";    //文件存放的路径
        private string _recoderLastLoadFoldPath
        {
            get
            {
                return _saveDataFoldPath + @"\LASTRECODER";
            }
        }                       //记录最后一次加载文件的路径
        private string _recoderLastLoadFileName = "LastLoadDataPath.txt";  //记录最后一次加载信息的文件名称
        private string lastLoadFilePath = "";                              //最后一次加载的文件


        private static readonly object obj = new object();
        public void Write(string fileName, Dictionary<object, List<object>> dic)
        {
            lock (obj)
            {
                checkAndCreateDirectory(_saveDataFoldPath);
                cleanAndFixData(ref dic);
                createAndWriteFile(dic, _saveDataFoldPath + @"\" + fileName + "#" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv");
            }
        }
        public Dictionary<object, List<object>> Read(string filePathAndName)
        {
            if (!System.IO.File.Exists(filePathAndName))
            {
                MessageBox.Show("读取的文件不存在");
                return null;
            }
            return readFile(filePathAndName);
        }
        public List<List<object>> ReadNoHeaderFile(string fullPathName)
        {
            List<List<object>> lstDic = new List<List<object>>();
            try
            {
                if (File.Exists(fullPathName))
                {
                    StreamReader sr = new StreamReader(fullPathName);
                    int index = 0;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] ls = line.Split(',');
                        if (ls.Length > 0)
                        {
                            lstDic.Add(new List<object>());
                            for (int i = 0; i < ls.Length; i++)
                            {
                                lstDic[index].Add(ls[i]);
                            }
                            index++;
                        }
                        else
                        {
                            sr.Close();
                            return null;
                        }
                    }
                    sr.Close();
                    return lstDic;
                }
                else
                {
                    throw new IOException("文件不存在: " + fullPathName);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("读取文件失败:" + e.Message);
                return null;
            }

        }

        public Dictionary<object, List<object>> ReadLastLoadData()
        {
            string filePath = _recoderLastLoadFoldPath + @"\" + _recoderLastLoadFileName;
            if (!System.IO.File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                fs.Close();
                return null;
            }
            else
            {
                StreamReader sr = new StreamReader(filePath);
                string record = sr.ReadLine();
                sr.Close();
                if (record != "")
                {
                    return readFile(record);
                }
            }
            return null;
        }


        private void cleanAndFixData(ref Dictionary<object, List<object>> dic)
        {
            int maxLen = -1;
            List<object> temp = dic.Keys.ToList<object>();
            List<object> keys = new List<object>();
            foreach (object ke in temp)
            {
                if (ke.ToString() != "")
                {
                    keys.Add(ke);
                }
            }
            //清洗数据
            foreach (string ke in keys)
            {
                for (int i = dic[ke].Count - 1; i > 0; i--)
                {
                    if (dic[ke][i].ToString() == " ")
                    {
                        dic[ke].RemoveAt(i);
                    }
                }
            }
            //补全数据
            foreach (string ke in keys)
            {
                if (maxLen < dic[ke].Count)
                {
                    maxLen = dic[ke].Count;
                }
            }
            foreach (string ke in keys)
            {
                if (dic[ke].Count < maxLen)
                {
                    for (int j = dic[ke].Count; j < maxLen; j++)
                    {
                        dic[ke].Add("#");
                    }
                }
            }
        }
        private void createAndWriteFile(Dictionary<object, List<object>> dic, string filePath)
        {
            try
            {
                lastLoadFilePath = filePath;
                recordFilePath();
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                StringBuilder sb = new StringBuilder();

                //把标题内容写入到文件流中
                List<object> temp = dic.Keys.ToList<object>();
                List<object> keys = new List<object>();
                foreach (object ke in temp)
                {
                    if (ke.ToString() != "")
                    {
                        keys.Add(ke);
                    }
                }
                foreach (string head in keys)
                {
                    sb.Append($"{head},");
                }
                sw.WriteLine(sb);
                sw.Flush();
                sb.Clear();

                //把数据内容写入到文件流中
                for (int i = 0; i < dic[keys[0]].Count; i++)
                {
                    foreach (string ke in keys)
                    {
                        sb.Append($"{dic[ke][i]}").Append(",");
                    }
                    sw.WriteLine(sb);
                    sw.Flush();
                    sb.Clear();
                }
                sw.Close();
                fs.Close();
                sb.Clear();
            }
            catch
            {
                MessageBox.Show("写入文件失败");
            }

        }
        private Dictionary<object, List<object>> readFile(string path)
        {
            Dictionary<object, List<object>> retDic = new Dictionary<object, List<object>>();
            try
            {
                lastLoadFilePath = path;
                recordFilePath();
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path);
                    //读取标题头
                    string head = sr.ReadLine();
                    string[] arr = head.Split(',');
                    List<object> keys = new List<object>();
                    if (arr.Length > 1)
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (arr[i] != "")
                            {
                                keys.Add(arr[i]);
                            }
                        }
                    }
                    else
                    {
                        sr.Close();
                        return null;
                    }

                    //读取文件内容
                    foreach (string ke in keys)
                    {
                        retDic[ke] = new List<object>();
                    }

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] ls = line.Split(',');
                        if (ls.Length > 0)
                        {
                            for (int i = 0; i < ls.Length; i++)
                            {
                                if (keys.Count > i)
                                {
                                    retDic[keys[i]].Add(ls[i]);
                                }
                            }
                        }
                        else
                        {
                            sr.Close();
                            return null;
                        }
                    }
                    sr.Close();
                    return retDic;
                }
                else
                {
                    throw new IOException("文件不存在: " + path);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("读取文件失败:" + e.Message);
                return null;
            }
        }
        private void checkAndCreateDirectory(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private static readonly object recordObj = new object();
        private void recordFilePath()
        {
            lock (recordObj)
            {
                string filePath = _recoderLastLoadFoldPath + @"\" + _recoderLastLoadFileName;
                checkAndCreateDirectory(_recoderLastLoadFoldPath);
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(lastLoadFilePath);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }
    }
}
