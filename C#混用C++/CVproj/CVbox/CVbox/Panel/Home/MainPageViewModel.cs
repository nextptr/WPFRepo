using CVbox.Common;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CVbox.Panel.Home
{
    public class MainPageViewModel : Screen
    {
        public Dictionary<string, IPage> PageItems = new Dictionary<string, IPage>();
        private ObservableCollection<PageItem> items = new ObservableCollection<PageItem>();
        public ObservableCollection<PageItem> Items
        {
            get { return items; }
            set
            {
                items = value;
                NotifyOfPropertyChange(nameof(Items));
            }
        }

        private IPage activeItem;
        public IPage ActiveItem
        {
            get { return activeItem; }
            set
            {
                activeItem = value;
                NotifyOfPropertyChange(nameof(ActiveItem));
            }
        }


        public MainPageViewModel()
        {
            var ls = IoC.GetAll<IPage>(null);
            foreach (var item in ls)
            {
                Items.Add(new PageItem(item.Name, item));
                PageItems[item.Name] = item;
            }
        }

        public void btnClick(object sender,RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn == null)
                return;
            PageItem pag = btn.DataContext as PageItem;
            if (pag == null)
                return;
            ActiveItem = pag.Page;
        }
    }

    public class PageItem : NotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyOfPropertyChange(nameof(Name));
            }
        }

        private IPage page;
        public IPage Page
        {
            get { return page; }
            set
            {
                page = value;
                NotifyOfPropertyChange(nameof(Page));
            }
        }

        public PageItem(){}
        public PageItem(string nam,IPage pag)
        {
            Name = nam;
            Page = pag;
        }
    }
}
