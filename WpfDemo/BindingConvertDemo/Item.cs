using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingConvertDemo
{
    public class Item : NotifyPropetryChanged
    {
        private int _color;
        public int Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }
    }
}
