using ComplexMvvm.Common;
using ComplexMvvm.Models;
using ComplexMvvm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComplexMvvm.ViewModels
{
    public class MainWindowViewModel : NotificationObject
    {

        //数据绑定
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                this.RaisePropertyChanged("Count");
            }
        }

        private Restaurant restaurant;
        public Restaurant Restaurant
        {
            get
            {
                return restaurant;
            }
            set
            {
                restaurant = value;
                this.RaisePropertyChanged("Restaurant");
            }
        }

        private List<DishMenuItemViewModel> dishMenu;
        public List<DishMenuItemViewModel> DishMenu
        {
            get
            {
                return dishMenu;
            }
            set
            {
                dishMenu = value;
                this.RaisePropertyChanged("DishMenu");
            }
        }


        //命令绑定
        public DelegateCommand PleaseOrderCommand { get; set; }
        public DelegateCommand SelectMenuItemCommand { get; set; }


        //构造函数
        public MainWindowViewModel()
        {
            LoadRestaurant();
            LoadDishMenu();
            this.PleaseOrderCommand = new DelegateCommand();
            this.SelectMenuItemCommand = new DelegateCommand();
            this.PleaseOrderCommand.ExecuteAction = new Action<object>(this.PleaseOrderCommandExecute);
            this.SelectMenuItemCommand.ExecuteAction = new Action<object>(this.SelectMenuItemExecute);
        }

        private void LoadRestaurant()
        {
            this.Restaurant = new Restaurant();
            this.Restaurant.Name = "Crazy大象";
            this.Restaurant.Address = "北京市海淀区万泉河路紫金庄园1号楼1层113室";
            this.Restaurant.PhoneNumber = "15210365423 or 820336";
        }
        private void LoadDishMenu()
        {
            XmlDataService ds = new XmlDataService();
            var dishes = ds.GetAllDishes();
            this.DishMenu = new List<DishMenuItemViewModel>();
            foreach (var dish in dishes)
            {
                DishMenuItemViewModel item = new DishMenuItemViewModel();
                item.Dish = dish;
                this.DishMenu.Add(item);
            }
        }


        //槽函数
        private void PleaseOrderCommandExecute(object param)
        {
            var selectedDishes = this.DishMenu.Where(i => i.IsSelected == true).Select(i => i.Dish.Name).ToList();
            IOrderService orderService = new MockOrderService();
            orderService.PleaseOrder(selectedDishes);
            MessageBox.Show("订餐成功");
        }
        private void SelectMenuItemExecute(object param)
        {
            this.Count = this.DishMenu.Count(i => i.IsSelected == true);
        }
    }
}
