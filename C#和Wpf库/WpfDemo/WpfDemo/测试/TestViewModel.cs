using Common;
using Common.Interface;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDemo.测试
{
    public class TestViewModel : Screen, IPage
    {
        private string name = "test";
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
