using Common.Stylet;
using Stylet;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DepthInCSharpDemo
{
    public class MainPageViewModel : Screen
    {
        private ObservableCollection<IPage> items;
        public ObservableCollection<IPage> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public MainPageViewModel()
        {
            Items = new ObservableCollection<IPage>();
            var ls = IoC.GetAll<IPage>(null);
            foreach (var item in ls)
            {
                Items.Add(item);
            }
        }

        public void btnChoise(object sender, RoutedEventArgs args)
        {
            try
            {
                Button btn = sender as Button;
                if (btn == null)
                    return;
                IPage dis = btn.DataContext as IPage;
                if (dis == null)
                    return;

                var router = IoC.Get<IRouter>();
                router.Push(dis.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
