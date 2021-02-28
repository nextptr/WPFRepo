using Common.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Parameter
{
    public interface IParameterItem
    {
        IParameterItem Clone();

        void Copy(IParameterItem other);
    }

    public abstract class IParameter : List<IParameterItem>, IXmlSerializable
    {
        protected string _fileName;
        protected string _directory;
        public string FileName { get { return _fileName; } }
        protected IParameter()
        {
            SetFileName();
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
            this.Copy(var);
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
        public virtual IParameter Clone()
        {
            IParameter clone = Activator.CreateInstance(GetType()) as IParameter;

            foreach (var i in this)
            {
                clone.Add(i.Clone());
            }

            return clone;
        }


        public virtual void Copy(IParameter other)
        {
            Clear();

            for (int i = 0; i < other.Count; ++i)
            {
                IParameterItem item = other[i].Clone();
                Add(item);
            }
        }
        public abstract void Create();
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
    }
}
