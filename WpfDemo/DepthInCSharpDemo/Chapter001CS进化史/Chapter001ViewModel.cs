using Common.Stylet;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepthInCSharpDemo
{
    public class Chapter001ViewModel : Screen, IPage
    {
        private string name = "C#进化史";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private ObservableCollection<string> datas = new ObservableCollection<string>();
        public ObservableCollection<string> Datas
        {
            get
            {
                return datas;
            }
            set
            {
                datas = value;
                OnPropertyChanged(nameof(Datas));
            }
        }
        private void msg(object obj)
        {
            Datas.Add(obj.ToString());
        }
        public void btnSortAndFilt()
        {
            msg("product类");
            List<Product> ls = Product.GetSampleProducts();
            foreach (var item in ls)
            {
                msg(item);
            }

            msg("\nIComparer排序");
            ls.Sort(new ProductNameComparer());
            foreach (var item in ls)
            {
                msg(item);
            }

            msg("\nDelegate排序");
            ls.Sort(delegate(Product x,Product y)
            { return y.Name.CompareTo(x.Name); });
            foreach (var item in ls)
            {
                msg(item);
            }

            msg("\nLambda排序");
            ls.Sort((x,y)=> x.Name.CompareTo(y.Name));
            foreach (var item in ls)
            {
                msg(item);
            }

            msg("\nOrderby排序");
            foreach (var item in ls.OrderBy(p=>p.Name))
            {
                msg(item);
            }
        }
        public void btnFind()
        {
            msg("c#1");
            List<Product> ls = Product.GetSampleProducts();
            foreach (var item in ls)
            {
                if (item.Price > 10m)
                {
                    msg("c#1:" + item.ToString());
                }
            }

            msg("c#2");
            Predicate<Product> test = delegate (Product p) { return p.Price > 10m; };
            List<Product> matches = ls.FindAll(test);
            matches.ForEach(p => { msg("c#2:"+ p.ToString()); });

            msg("c#3");
            ls.FindAll(p => { return p.Price > 10m; }).ForEach(p => msg("c#3:" + p.ToString()));

            msg("c#4");
            foreach (var item in ls.Where(p=>p.Price>10m))
            {
                msg("c#4:" + item.ToString());
            }
        }
        public void btnBack()
        {
            var router = IoC.Get<IRouter>();
            router.GoBack();
        }
    }
}