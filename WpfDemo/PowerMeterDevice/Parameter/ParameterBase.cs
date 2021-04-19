using PowerMeterDevice.Interf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace PowerMeterDevice.Common
{
    /// <summary>
    /// 所有参数的基类
    /// </summary>
    public abstract class ParameterBase : NotifyPropertyChanged, IParameter
    {
        /// <summary>
        /// 参数的文件名
        /// </summary>
        [XmlIgnore]
        public virtual string FileName
        {
            get
            {
                return GetType().Name + ".xml";
            }
        }
        /// <summary>
        /// 参数的目录
        /// </summary>
        [XmlIgnore]
        public virtual string Dir
        {
            get
            {
                return RootPath.G_ParameterPath;
            }
        }

        public ParameterBase()
        {
        }

        /// <summary>
        /// 参数的读取
        /// </summary>
        public void Read()
        {
            string path = MakePath(Dir, FileName);
            Read(path);
        }
        /// <summary>
        /// 参数的读取的重载
        /// </summary>
        /// <param name="path"></param>
        public void Read(string path)
        {
            IParameter var = this;
            if (!File.Exists(path))
            {
                Write();
            }
            else
            {
                try
                {
                    XmlSerializer ser = new XmlSerializer(GetType());
                    FileStream fs = new FileStream(path, FileMode.Open);
                    var = ser.Deserialize(fs) as IParameter;
                    fs.Close();
                }
                catch
                {
                }
                Copy(var);
            }
        }

        /// <summary>
        /// 文件目录和文件名称的组合
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string MakePath(string directory, string fileName)
        {
            string path = null;

            if (!string.IsNullOrWhiteSpace(directory))
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                path = directory + "\\" + fileName;
            }
            else
            {
                path = fileName;
            }

            return path;
        }
        /// <summary>
        /// 写入到本地s
        /// </summary>
        public void Write()
        {
            try
            {
                string path = MakePath(Dir, FileName);
                Write(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 写入到本地的重载
        /// </summary>
        /// <param name="fileName"></param>
        public void Write(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(GetType());
            FileInfo fileInfo = new FileInfo(fileName);
            if (!fileInfo.Directory.Exists)
                fileInfo.Directory.Create();
            FileStream fs = new FileStream(fileName, FileMode.Create);
            xs.Serialize(fs, this);
            fs.Close();
        }
        /// <summary>
        /// 参数的拷贝
        /// </summary>
        /// <param name="source"></param>
        public abstract void Copy(IParameter source);
    }
}
