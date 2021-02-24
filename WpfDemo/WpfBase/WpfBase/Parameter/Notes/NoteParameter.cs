using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfBase.Parameter.Notes
{
    public class NoteItem : NotifyPropertyChanged, IParameterItem
    {
        private string _noteName = "";
        private ObservableCollection<string> _noteData = new ObservableCollection<string>();

        public string NoteName
        {
            get { return _noteName; }
            set { if (_noteName == value) { return; } _noteName = value; OnPropertyChanged("_noteName"); }
        }
   
        public ObservableCollection<string> NoteData
        {
            get
            {
                return _noteData;
            }
            set
            {
                _noteData.Clear();
                for (int i = 0; i < value.Count; i++)
                {
                    _noteData.Add(value[i]);
                }
            }
        }


        public IParameterItem Clone()
        {
            NoteItem clone = new NoteItem();
            clone.NoteName = this.NoteName;
            clone.NoteData = this.NoteData;

            return clone;
        }
        public void Copy(IParameterItem other)
        {
            NoteItem ot = other as NoteItem;
            if (ot == null)
            {
                return;
            }
            else
            {
                this.NoteName = ot.NoteName;
                this.NoteData = ot.NoteData;
            }
        }

    }
    public class NoteParameter : IParameter
    {
        private List<NoteItem> _data = null;
        public List<NoteItem> Data
        {
            get
            {
                if (this.Count <= 1)
                {
                    return null;
                }
                if (_data == null)
                {
                    _data = new List<NoteItem>();
                    for (int i = 1; i < this.Count; i++)
                    {
                        _data.Add(this[i] as NoteItem);
                    }
                }
                else
                {
                    _data.Clear();
                    for (int i = 1; i < this.Count; i++)
                    {
                        _data.Add(this[i] as NoteItem);
                    }
                }
                return _data;
            }
        }
        public override void Create()
        {
            NoteItem itm = new NoteItem();
            itm.NoteName = "占位dxf";
            itm.NoteData = new ObservableCollection<string>();
            this.Add(itm);
        }
    }

}
