using Common.Interface;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDemo.WPF动画
{
    public class WPFAnimationViewModel : Screen, IPage
    {
        private string name = "WPF动画";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyOfPropertyChange(nameof(Name));
            }
        }
    }
}
