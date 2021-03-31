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

        public void btnSortAndFilt()
        {
            List<Product> ls = Product.GetSampleProducts();
            foreach (var item in ls)
            {
                Datas.Add(item.ToString());
            }
        }

        private double roughX;
        public double RoughX
        {
            get
            {
                return roughX;
            }
            set
            {
                roughX = value;
                OnPropertyChanged(nameof(RoughX));
            }
        }
        private double roughY;
        public double RoughY
        {
            get
            {
                return roughY;
            }
            set
            {
                roughY = value;
                OnPropertyChanged(nameof(RoughY));
            }
        }

        private double fineX;
        public double FineX
        {
            get
            {
                return fineX;
            }
            set
            {
                fineX = value;
                OnPropertyChanged(nameof(FineX));
            }
        }
        private double fineY;
        public double FineY
        {
            get
            {
                return fineY;
            }
            set
            {
                fineY = value;
                OnPropertyChanged(nameof(FineY));
            }
        }

        public void btnConvert()
        {
            FineX = RoughX - 51.728;
            FineY = RoughY - 1.675;
        }
    }
}
