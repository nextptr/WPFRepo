using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CsBase.Common
{
    public interface IParameterItem
    {
        IParameterItem Clone();
        void Copy(IParameterItem other);
    }

    public abstract class IParameter:List<IParameterItem>, IXmlSerializable
    {
        public string FileName { get { return _fileName; } }
        protected string _fileName;
        protected string _directory;

        protected IParameter()
        {
            SetFileName();
        }
        protected virtual void SetFileName()
        {
            _fileName = GetType().Name + ".xml";
            _directory = "Parameters";
        }


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
        private void CreateIfNotExist(string path)
        {
            if (!File.Exists(path))
            {
                Create();
                Write(path);
            }
        }


        public virtual void Read()
        {
            string path = MakePath(_directory, _fileName);
            Read(path);
        }
        public virtual void Write()
        {
            string path = MakePath(_directory, _fileName);
            File.Copy(path, path + ".bak", true);
            Write(path);
        }
        public virtual void Read(string path)
        {
            CreateIfNotExist(path);
            XmlSerializer ser = new XmlSerializer(GetType());
            FileStream fs = new FileStream(path, FileMode.Open);
            IParameter var = ser.Deserialize(fs) as IParameter;
            fs.Close();
            Clear();
            for (int i = 0; i < var.Count; ++i)
            {
                IParameterItem item = var[i].Clone();
                Add(item);
            }
        }
        public virtual void Write(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(GetType());
            FileStream fs = new FileStream(fileName, FileMode.Create);
            xs.Serialize(fs, this);
            fs.Close();
        }

        public void ReadXml(XmlReader reader)
        {
            string startElement = GetType().Name;
            reader.ReadStartElement(startElement);
            while (reader.IsStartElement("IParameterItem"))
            {
                Type type = Type.GetType(reader.GetAttribute("AssemblyQualifiedName"));
                XmlSerializer serial = new XmlSerializer(type);

                reader.ReadStartElement("IParameterItem");
                this.Add((IParameterItem)serial.Deserialize(reader));
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }
        public void WriteXml(XmlWriter writer)
        {
            foreach (IParameterItem item in this)
            {
                writer.WriteStartElement("IParameterItem");
                writer.WriteAttributeString("AssemblyQualifiedName", item.GetType().AssemblyQualifiedName);
                XmlSerializer xmlSerializer = new XmlSerializer(item.GetType());
                xmlSerializer.Serialize(writer, item);
                writer.WriteEndElement();
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
        public abstract void Create();

    }
}
