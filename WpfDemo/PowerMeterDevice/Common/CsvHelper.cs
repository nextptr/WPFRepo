using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System;

namespace PowerMeterDevice.Common
{
    public class CsvHelper
    {
        private string FilePath = "";
        public CsvHelper(string filePath)
        {
            FilePath = filePath;
        }

        private static readonly object obj = new object();
        public void Write(string fileName, List<KeyValuePair<double, double>> dat)
        {
            lock (obj)
            {
                checkAndCreateDirectory(FilePath);
                createAndWriteFile(dat, FilePath + @"\" + fileName + ".csv");
            }
        }
        public List<KeyValuePair<double, double>> Read(string fileName)
        {
            string pth = FilePath + @"\" + fileName + ".csv";
            if (!System.IO.File.Exists(pth))
            {
                MessageBox.Show("读取的文件不存在");
                return null;
            }
            return readFile(pth);
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
        private void createAndWriteFile(List<KeyValuePair<double, double>> dat, string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                StringBuilder sb = new StringBuilder();

                //把数据内容写入到文件流中
                for (int i = 0; i < dat.Count; i++)
                {
                    sb.Append($"{dat[i].Key}").Append(",").Append($"{dat[i].Value}").Append(",");
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
        private List<KeyValuePair<double, double>> readFile(string path)
        {
            List<KeyValuePair<double, double>> retDic = new List<KeyValuePair<double, double>>();
            try
            {
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path);

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] ls = line.Split(',');
                        if (ls.Length > 0)
                        {
                            retDic.Add(new KeyValuePair<double, double>(Convert.ToDouble(ls[0]), Convert.ToDouble(ls[1])));
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
    }
}
