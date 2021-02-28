using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parameter.ProjName
{
    public class TotalInformation : NotifyPropertyChanged, IParameterItem
    {
        private string _itemName;
        private string _itemAddress;
        private int _itemPhonNumber;

        public string ItemName
        {
            get { return _itemName; }
            set { if (_itemName == value) { return; } _itemName = value; OnPropertyChanged("ItemName"); }
        }
        public string ItemAddress
        {
            get { return _itemAddress; }
            set { if (_itemAddress == value) { return; } _itemAddress = value; OnPropertyChanged("ItemAddress"); }
        }
        public int ItemPhonNumber
        {
            get { return _itemPhonNumber; }
            set { if (_itemPhonNumber == value) { return; } _itemPhonNumber = value; OnPropertyChanged("ItemPhonNumber"); }
        }

        public IParameterItem Clone()
        {
            TotalInformation clon = new TotalInformation();
            clon.ItemName = this.ItemName;
            clon.ItemAddress = this.ItemAddress;
            clon.ItemPhonNumber = this.ItemPhonNumber;
            return clon;
        }
        public void Copy(IParameterItem other)
        {
            TotalInformation tmp = other as TotalInformation;
            if (tmp != null)
            {
                this.ItemName = tmp.ItemName;
                this.ItemAddress = tmp.ItemAddress;
                this.ItemPhonNumber = tmp.ItemPhonNumber;
            }
        }
    }

    public class ParameterItem : NotifyPropertyChanged, IParameterItem
    {
        private bool _itemStatus;
        private string _itemName;
        private double _itemPosX;
        private double _itemPosY;
        private double _itemPosZ;


        public bool ItemStatus
        {
            get { return _itemStatus; }
            set { if (_itemStatus == value) { return; } _itemStatus = value; OnPropertyChanged("ItemStatus"); }
        }
        public string ItemName
        {
            get { return _itemName; }
            set { if (_itemName == value) { return; } _itemName = value; OnPropertyChanged("ItemName"); }
        }
        public double ItemPosX
        {
            get { return _itemPosX; }
            set { if (_itemPosX == value) { return; } _itemPosX = value; OnPropertyChanged("ItemPosX"); }
        }                                                                                       
        public double ItemPosY
        {                                                                                       
            get { return _itemPosY; }                                                           
            set { if (_itemPosY == value) { return; } _itemPosY = value; OnPropertyChanged("ItemPosY"); }
        }                                                                                       
        public double ItemPosZ
        {                                                                                       
            get { return _itemPosZ; }                                                           
            set { if (_itemPosZ == value) { return; } _itemPosZ = value; OnPropertyChanged("ItemPosZ"); }
        }


        public IParameterItem Clone()
        {
            ParameterItem clon = new ParameterItem();
            clon.ItemName = this.ItemName;
            clon.ItemStatus = this.ItemStatus;
            clon.ItemPosX = this.ItemPosX;
            clon.ItemPosY = this.ItemPosY;
            clon.ItemPosZ = this.ItemPosZ;
            return clon;
        }
        public void Copy(IParameterItem other)
        {
            ParameterItem tmp = other as ParameterItem;
            if (tmp != null)
            {
                this.ItemName = tmp.ItemName;
                this.ItemStatus = tmp.ItemStatus;
                this.ItemPosX = tmp.ItemPosX;
                this.ItemPosY = tmp.ItemPosY;
                this.ItemPosZ = tmp.ItemPosZ;
            }
        }
    }


    public class testParameter : IParameter
    {
        public TotalInformation Head //下标0
        {
            get
            {
                return this[0] as TotalInformation;
            }
        }

        public List<ParameterItem> Data//下标1开始
        {
            get
            {
                List<ParameterItem> tmp = new List<ParameterItem>();
                for (int i = 1; i < this.Count; i++)
                {
                    tmp.Add(this[i] as ParameterItem);
                }
                return tmp;
            }
        }

        public testParameter()
        {
        }

        public override void Create()
        {
            //公司
            TotalInformation fac = new TotalInformation();
            fac.ItemName = "西安工业";
            fac.ItemAddress = "西安市未央区";
            fac.ItemPhonNumber = 725300;
            this.Add(fac);
            //参数
            AddItem("食堂",  true, 1000, 2000, 300);
            AddItem("宿舍", false, 0, 0, 0);
            AddItem("教室", false, -100, 2000, 30);
        }

        private void AddItem(string nam, bool sta, double posx, double posy, double posz)
        {
            ParameterItem itm = new ParameterItem();
            itm.ItemName = nam;
            itm.ItemStatus = sta;
            itm.ItemPosX =posx;
            itm.ItemPosY =posy;
            itm.ItemPosZ =posz;
            this.Add(itm);
        }
    }
}
