using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using WpfLifeGame.Common;

namespace WpfLifeGame.CellBase
{
    public class Pattern:NotifyPropertyChanged
    {
        private string _patternName = "";
        public int Size = 0;
        public string BitMaps = "";
        public string PatternName
        {
            get
            {
                return _patternName;
            }
            set
            {
                _patternName = value;
                OnPropertyChanged("PatternName");
            }
        }

        public Pattern()
        {
            _patternName = "";
            BitMaps = "";
            Size = 0;
        }
        public Pattern(string nam, int size)
        {
            PatternName = nam;
            BitMap map = new BitMap(size * size);
            BitMaps = map.ToStr;
            Size = size;
        }
        public Pattern(string str, List<List<cell>> cells)
        {
            PatternName = str;
            List<bool> ls = new List<bool>();
            for (int i = 0; i < cells.Count; i++)
            {
                for (int j = 0; j < cells[i].Count; j++)
                {
                    ls.Add(cells[i][j].LiveNow);
                }
            }
            BitMap map = new BitMap(ls);
            BitMaps = map.ToStr;
            Size = cells.Count;
        }
        public List<List<cell>> ToCells()
        {
            BitMap map = new BitMap(Size * Size, BitMaps);
            List<List<cell>> tmp = new List<List<cell>>();
            List<bool> ls = map.ToList;
            for (int i = 0; i < Size; i++)
            {
                tmp.Add(new List<cell>());
                for (int j = 0; j < Size; j++)
                {
                    tmp[i].Add(new cell(ls[i * Size + j]));
                }
            }
            return tmp;
        }

        public Pattern Clone()
        {
            Pattern clone = new Pattern();
            clone._patternName = this._patternName;
            clone.BitMaps = this.BitMaps;
            clone.Size = this.Size;
            return clone;
        }
        public void Copy(Pattern other)
        {
            Pattern ot = other as Pattern;
            if (ot == null)
            {
                return;
            }
            else
            {
                this._patternName = ot._patternName;
                this.BitMaps = ot.BitMaps;
                this.Size = ot.Size;
            }
        }
    }

    public class CellsParameter:List<Pattern>, IXmlSerializable
    {
        protected static CellsParameter _instance = null;
        public static CellsParameter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CellsParameter();
                }
                return _instance;
            }
        }
        public CellsParameter()
        {

        }
        public bool IsExitPattern(string nam)
        {
            foreach (var tmp in this)
            {
                if (tmp.PatternName == nam)
                {
                    return true;
                }
            }
            return false;
        }


        protected string GetRootPath()
        {
            string currentPath = Directory.GetCurrentDirectory();
            string str = Directory.GetDirectoryRoot(currentPath);
            string[] arr = currentPath.Split('\\');
            int count = arr.Length - 2;
            string ret = "";
            for (int i = 0; i < count; i++)
            {
                ret += arr[i];
                if (i != count - 1)
                {
                    ret += '\\';
                }
            }
            return ret;
        }
        public bool Read(string name)
        {
            string fileName = GetRootPath() + "\\" + name;
            try
            {
                if (File.Exists(fileName))
                {
                    XmlSerializer ser = new XmlSerializer(GetType());
                    FileStream fs = new FileStream(fileName, FileMode.Open);
                    CellsParameter var = ser.Deserialize(fs) as CellsParameter;
                    fs.Close();
                    this.Copy(var);
                    return true;
                }
                else
                {
                    this.Add(new Pattern("初始图形", 50));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("读取文件失败:" + e.Message);
            }
            return false;
        }
        public bool Write(string name)
        {
            try
            {
                string fileName = GetRootPath() + "\\" + name;
                XmlSerializer xs = new XmlSerializer(GetType());
                FileStream fs = new FileStream(fileName, FileMode.Create);
                xs.Serialize(fs, this);
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("保存文件失败:" + e.Message);
                return false;
            }
        }


        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }
        public void ReadXml(XmlReader reader)
        {
            string startElement = GetType().Name;
            reader.ReadStartElement(startElement);
            while (reader.IsStartElement("Pattern"))
            {
                Type type = Type.GetType(reader.GetAttribute("AssemblyQualifiedName"));
                XmlSerializer serial = new XmlSerializer(type);
                reader.ReadStartElement("Pattern");
                this.Add((Pattern)serial.Deserialize(reader));
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }
        public void WriteXml(XmlWriter writer)
        {
            foreach (Pattern item in this)
            {
                writer.WriteStartElement("Pattern");
                writer.WriteAttributeString("AssemblyQualifiedName", item.GetType().AssemblyQualifiedName);
                XmlSerializer xmlSerializer = new XmlSerializer(item.GetType());
                xmlSerializer.Serialize(writer, item);
                writer.WriteEndElement();
            }
        }
        public virtual CellsParameter Clone()
        {
            CellsParameter clone = Activator.CreateInstance(GetType()) as CellsParameter;
            foreach (var i in this)
            {
                clone.Add(i.Clone());
            }
            return clone;
        }
        public virtual void Copy(CellsParameter other)
        {
            Clear();
            for (int i = 0; i < other.Count; ++i)
            {
                Pattern item = other[i].Clone();
                Add(item);
            }
        }
    }
}
