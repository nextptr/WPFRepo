using CVbox.Common;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CVbox.Panel.ProjPage.ViewModel
{
    public class DllTestViewModel : IPage
    {
        public string Name { get; set; } = "WPF与CPP调用测试";
        public void btnTest(string arg)
        {
            switch (arg)
            {
                case "1":
                    //CVAlgorithm.CVProxy.CVProxyInstance.Test();
                    break;
                case "2":
                    CVWrapper.CVproxy.CVproxyInstance.CVTest1(@"D:\GitWell\WPFRepo\C#混用C++\CVproj\CVbox\IMG\lena.jpg");
                    break;
                case "3":
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    if (openFileDialog1.ShowDialog().Value)
                    {
                        string fileFullPath = openFileDialog1.FileName;
                        string fileName = openFileDialog1.SafeFileName;
                        string filePath = fileFullPath.TrimEnd(fileName.ToCharArray());
                        List<string> imgLs = GetDirectoryFiles(filePath);

                        int i = 0;
                        foreach (var item in imgLs)
                        {
                            i++;
                            CVWrapper.CVproxy.CVproxyInstance.AttachPrint(item, i.ToString());
                        }
                    }

                  
                    break;
                default:
                    break;
            }
        }

        public List<string> GetDirectoryFiles(string filePath)
        {
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] fi = di.GetFiles();
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var item in fi)
            {
                string fileName = item.Name;
                int index = -1;
                if (fileName.Contains(".jpg"))
                {
                    index = int.Parse(fileName.TrimEnd(".jpg".ToCharArray()));
                }
                if (fileName.Contains(".png"))
                {
                    index = int.Parse(fileName.TrimEnd(".png".ToCharArray()));
                }
                if (fileName.Contains(".bmp"))
                {
                    index = int.Parse(fileName.TrimEnd(".bmp".ToCharArray()));
                }
                if (index != -1)
                {
                    dic[index] = item.FullName;
                }
            }

            List<int> lsKey = dic.Keys.ToList<int>();
            List<string> lsFiles = new List<string>();
            lsKey.Sort();
            foreach (var ky in lsKey)
            {
                lsFiles.Add(dic[ky]);
            }
            return lsFiles;

        }


    }
}
