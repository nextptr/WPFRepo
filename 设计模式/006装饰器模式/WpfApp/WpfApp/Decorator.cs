using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp
{
    public class Decorator : Component
    {
        protected ListBox lsbx;
        protected Component component;
        public void SetComponent(Component component)
        {
            this.component = component;
        }
        public override void operation()
        {
            if (component != null)
            {
                component.operation();
            }
        }
    }
    public class ConcreteDecoratorWhiteHat : Decorator
    {
        public ConcreteDecoratorWhiteHat(ListBox ls)
        {
            this.lsbx = ls;
        }
        public override void operation()
        {
            lsbx.Items.Add("戴一顶白帽子");
            base.operation();
        }
    }
    public class ConcreteDecoratorBlackHat : Decorator
    {
        public ConcreteDecoratorBlackHat(ListBox ls)
        {
            this.lsbx = ls;
        }
        public override void operation()
        {
            lsbx.Items.Add("戴一顶黑帽子");
            base.operation();
        }
    }

    public class ConcreteDecoratorTShirt : Decorator
    {
        public ConcreteDecoratorTShirt(ListBox ls)
        {
            this.lsbx = ls;
        }
        public override void operation()
        {
            lsbx.Items.Add("穿T恤");
            base.operation();
        }
    }
    public class ConcreteDecoratorSuit : Decorator
    {
        public ConcreteDecoratorSuit(ListBox ls)
        {
            this.lsbx = ls;
        }
        public override void operation()
        {
            lsbx.Items.Add("穿西装");
            base.operation();
        }
    }
    public class ConcreteDecoratorTie : Decorator
    {
        public ConcreteDecoratorTie(ListBox ls)
        {
            this.lsbx = ls;
        }
        public override void operation()
        {
            lsbx.Items.Add("戴领带");
            base.operation();
        }
    }

    public class ConcreteDecoratorBigTrouser : Decorator
    {
        public ConcreteDecoratorBigTrouser(ListBox ls)
        {
            this.lsbx = ls;
        }
        public override void operation()
        {
            lsbx.Items.Add("垮裤");
            base.operation();
        }
    }
}
