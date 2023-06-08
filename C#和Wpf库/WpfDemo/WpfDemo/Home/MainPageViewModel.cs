using Common;
using Common.Interface;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfDemo.Logic;

namespace WpfDemo.Home
{
    public class MainPageViewModel : Screen
    {
        private Dictionary<string, IPage> pageItems = new Dictionary<string, IPage>();
        public Dictionary<string, IPage> PageItems
        {
            get { return pageItems; }
            set
            {
                pageItems = value;
                NotifyOfPropertyChange(nameof(PageItems));
            }
        }

        private Router _router;
        public MainPageViewModel()
        {
            _router= IOC.Get<Router>();
            var ls = IOC.GetAll<IPage>(null);
            foreach (var item in ls)
            {
                PageItems[item.Name] = item;
            }
        }

        public void btnChoise(string arg)
        {
            try
            {
                if (!PageItems.ContainsKey(arg))
                    return;
                _router.Push(arg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
