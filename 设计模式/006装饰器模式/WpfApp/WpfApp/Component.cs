using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp
{
    public abstract class Component
    {
        public abstract void operation();
    }
    public class ConcreateComponent : Component
    {
        ListBox lsbx;
        string nm;
        public ConcreateComponent(ListBox ls,string nam)
        {
            lsbx = ls;
            nm = nam;
        }
        public override void operation()
        {
            lsbx.Items.Add(nm+"的装饰");
        }
    }
}
