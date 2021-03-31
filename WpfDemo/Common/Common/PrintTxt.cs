using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PrintTxt
    {
        protected FileStream fs;
        protected StreamWriter writ;
        public PrintTxt(string filename)
        {
            string filepath = Directory.GetCurrentDirectory() + "\\测试文件\\" + filename + ".txt";
            if (!File.Exists(filepath))
            {
                fs = File.Create(filepath);
            }
            else
            {
                fs = File.Open(filepath, FileMode.Open);
            }
            writ = new StreamWriter(fs);
        }
        public void Wr(string str)
        {
            writ.WriteLine(str);
        }
        public void close()
        {
            writ.Close();
            fs.Close();
        }
    }

    public class PrintTxtFile
    {
        protected FileStream fs;
        protected StreamWriter writ;
        public PrintTxtFile(string filename)
        {
            string filepath = Directory.GetCurrentDirectory() + "\\测试文件\\" + filename + ".txt";
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\测试文件"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\测试文件");
            }
            if (!File.Exists(filepath))
            {
                fs = File.Create(filepath);
            }
            else
            {
                fs = File.Open(filepath, FileMode.Append);
            }
            writ = new StreamWriter(fs);
        }

        public PrintTxtFile(string filePath, string file, FileMode mod)
        {
            if (filePath == "" || file == null)
            {
                return;
            }
            if (!file.Contains(".txt"))
            {
                file += ".txt";
            }
            string fullPath = filePath + "\\" + file;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (!File.Exists(fullPath))
            {
                fs = File.Create(fullPath);
            }
            else
            {
                fs = File.Open(fullPath, mod);
            }
            writ = new StreamWriter(fs);
        }

        public void Wr(string str)
        {
            //writ.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fff") + ":" + str);
            writ.WriteLine(str);
        }
        public void OntimeWr(string str)
        {
            writ.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fff ") + str);
        }
        public void close()
        {
            writ.Close();
            fs.Close();
        }
    }
}
