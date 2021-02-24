using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class IPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(object obj)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(obj)));
        }
    }
}
