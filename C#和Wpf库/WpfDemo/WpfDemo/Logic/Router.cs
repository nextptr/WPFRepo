using Common.Interface;
using System.Collections.Generic;
using WpfDemo.Home;

namespace WpfDemo.Logic
{
    public class Router : IRouter
    {
        public void GoBack()
        {
            if (this.stack.Count > 0)
            {
                this.stack.Pop();
                if (this.stack.Count == 0)
                {
                    this.Main.Top();
                    return;
                }
                string name = this.stack.Peek();
                this.Main.Push(name);
            }
        }
        public void Push(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            if (this.stack.Count > 0)
            {
                string b = this.stack.Peek();
                if (name == b)
                {
                    return;
                }
            }
            this.stack.Push(name);
            this.Main.Push(name);
        }
        public void BackToTop()
        {
            this.stack.Clear();
            this.Main.Top();
        }
        public IPage GetPageInstance(string name)
        {
            return this.Main.GetPageInstance(name);
        }
        private Stack<string> stack = new Stack<string>();
        public MainViewModel Main;
    }
}
