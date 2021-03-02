using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class filePathObj
    {
        public string fileName = "";
        public string fullName = "";
        public string fileDir = "";
    }

    public class FileAnalyseHelper
    {
        public filePathObj SelectFileDialog()
        {
            string str = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog().Value)
            {
                str = openFileDialog.FileName;
            }

            filePathObj ret = new filePathObj();
            ret.fullName = str;
            string[] arr = str.Split('\\');
            if (arr.Length != 0)
            {
                string end = arr[arr.Length - 1];
                ret.fileDir = str.TrimEnd(('\\' + end).ToCharArray());
                ret.fileName = arr[arr.Length - 1].TrimEnd(".log".ToCharArray());
            }
            return ret;
        }

        private List<List<string>> getMotionRawData(filePathObj obj)
        {
            FileStream fs = new FileStream(obj.fullName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            List<List<string>> qrLs = new List<List<string>>();
            string beg = "";
            string tmp = "";
            while (!sr.EndOfStream)
            {
                beg = sr.ReadLine();
                while (beg.Contains("开始上料"))
                {
                    List<string> ls = new List<string>();
                    ls.Add(beg.Substring(26, 10));
                    while (!sr.EndOfStream)
                    {
                        tmp = sr.ReadLine();

                        if (!tmp.Contains("开始上料"))
                        {
                            if (tmp.Contains("点灯压接") ||
                                tmp.Contains("二次点灯") ||
                                tmp.Contains("结束点灯"))
                            {
                                ;
                            }
                            else
                            {
                                ls.Add(tmp.Substring(26, 10));
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    beg = tmp;
                    tmp = "";
                    qrLs.Add(ls);
                }
            }
            sr.Close();
            fs.Close();
            return qrLs;
        }
        private List<string> getAlignRawData(filePathObj obj)
        {
            FileStream fs = new FileStream(obj.fullName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            List<string> qrLs = new List<string>();
            string beg = "";
            while (!sr.EndOfStream)
            {
                beg = sr.ReadLine();
                if (beg.Contains("图像处理#"))
                {
                    string[] arr = beg.Split('#');
                    qrLs.Add(arr[1]);
                }
            }
            sr.Close();
            fs.Close();
            return qrLs;
        }
        private List<string> getMemRawData(filePathObj obj)
        {
            FileStream fs = new FileStream(obj.fullName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            List<string> qrLs = new List<string>();
            string beg = "";
            while (!sr.EndOfStream)
            {
                beg = sr.ReadLine();
                if (beg.Contains("#内存#"))
                {
                    qrLs.Add(beg);
                }
            }
            sr.Close();
            fs.Close();
            return qrLs;
        }

        private List<List<int>> converToint(List<List<string>> qrLs)
        {
            List<List<int>> tmpls = new List<List<int>>();
            for (int i = 0; i < qrLs.Count; i++)
            {
                List<int> tmpLs = new List<int>();
                foreach (string str in qrLs[i])
                {
                    int.TryParse(str, out int D);
                    tmpLs.Add(D);
                }
                tmpls.Add(tmpLs);
            }

            int[] lengthDic = new int[5000];
            foreach (var ls in tmpls)
            {
                lengthDic[ls.Count]++;
            }

            int maxSize = 0;
            int tmpCount = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (tmpCount < lengthDic[i])
                {
                    tmpCount = lengthDic[i];
                    maxSize = i;
                }
            }

            List<List<int>> retLs = new List<List<int>>();
            foreach (var ls in tmpls)
            {
                if (ls.Count == maxSize)
                {
                    retLs.Add(ls);
                }
            }
            return retLs;
        }

        private List<List<int>> converToRalaData(List<List<int>> qrLs)
        {
            List<List<int>> retLs = new List<List<int>>();
            for (int i = 0; i < qrLs.Count; i++)
            {
                List<int> tmpLs = new List<int>();
                for (int j = 1; j < qrLs[i].Count; j++)
                {
                    tmpLs.Add(qrLs[i][j] - qrLs[i][j - 1]);
                }
                retLs.Add(tmpLs);
            }
            return retLs;
        }

        private void saveAsRawData(filePathObj obj,List<List<int>> dataLs, string fileName)
        {
            FileStream fsw = new FileStream(obj.fileDir + $"\\{obj.fileName}_{fileName}.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fsw);

            int maxSize = dataLs[0].Count;
            List<List<string>> outLs = new List<List<string>>();
            for (int i = 0; i < maxSize; i++)
            {
                List<string> tmpLs = new List<string>();
                foreach (var ls in dataLs)
                {
                    if (ls.Count > i)
                    {
                        tmpLs.Add(ls[i].ToString()+" ");
                    }
                    else
                    {
                        tmpLs.Add("  ");
                    }
                }
                outLs.Add(tmpLs);
            }

            foreach (var ls in outLs)
            {
                string lin = "";
                foreach (string st in ls)
                {
                    lin += st;
                }
                sw.WriteLine(lin);
            }
            sw.Close();
            fsw.Close();
        }
        private void saveAsRawData(filePathObj obj, List<string> dataLs, string fileName)
        {
            FileStream fsw = new FileStream(obj.fileDir + $"\\{obj.fileName}_{fileName}.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fsw);
            foreach (var d in dataLs)
            {
                sw.WriteLine(d);
            }
            sw.Close();
            fsw.Close();
        }


        public void analyseMotionData(filePathObj obj)
        {
            List<List<string>> rawLs = getMotionRawData(obj);
            List<List<int>> rawDatas = converToint(rawLs);
            saveAsRawData(obj, rawDatas, "初始数据");

            List<List<int>> relDatas = converToRalaData(rawDatas);
            saveAsRawData(obj, relDatas, "相对用时");
        }
        public void analyseAlignData(filePathObj obj)
        {
            List<string> rawLs = getAlignRawData(obj);
            saveAsRawData(obj, rawLs, "焦点数据");
        }
        public void analyseMemoryData(filePathObj obj)
        {
            List<string> rawLs = getMemRawData(obj);
            saveAsRawData(obj, rawLs, "内存数据");
        }
    }
}
