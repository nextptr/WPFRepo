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
}
