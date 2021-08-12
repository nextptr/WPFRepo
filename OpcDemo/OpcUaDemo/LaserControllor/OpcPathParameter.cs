using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserControllor
{
    public class OpcPathParameterItem 
    {
        public string IpAddress
        {
            get { return _ipAddress; }
            set { if (_ipAddress != value) { _ipAddress = value; } }
        }

        public string KeyName
        {
            get { return _keyName; }
            set { if (_keyName != value) { _keyName = value; } }
        }

        public string Path
        {
            get { return _path; }
            set { if (_path != value) { _path = value; } }
        }

        public string PathValue
        {
            get { return _pathValue; }
            set { if (_pathValue != value) { _pathValue = value; } }
        }





        private string _ipAddress;
        private string _keyName;
        private string _path;
        private string _pathValue;

        public OpcPathParameterItem()
        {

            _ipAddress = "";
            _keyName = "";
            _path = "";
            _pathValue = "";
        }

        //public IParameterItem Clone()
        //{
        //    OpcPathParameterItem clone = new OpcPathParameterItem();
        //    clone._ipAddress = _ipAddress;
        //    clone._keyName = _keyName;
        //    clone._path = _path;
        //    clone._pathValue = _pathValue;
        //    return clone;
        //}

        //public void Copy(IParameterItem other)
        //{
        //    OpcPathParameterItem o = other as OpcPathParameterItem;
        //    _ipAddress = o.IpAddress;
        //    _keyName = o.KeyName;
        //    _path = o.Path;
        //    _pathValue = o.PathValue;
        //}
    }
    public class OpcPathParameter
    {
        public OpcPathParameter()
        {
        }


        //public override void Create()
        //{
        //    Add(new OpcPathParameterItem());
        //    Add(new OpcPathParameterItem());
        //    Add(new OpcPathParameterItem());
        //    Add(new OpcPathParameterItem());
        //}

    }
}
