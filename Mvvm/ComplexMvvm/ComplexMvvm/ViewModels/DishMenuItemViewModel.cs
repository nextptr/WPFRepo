using ComplexMvvm.Common;
using ComplexMvvm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexMvvm.ViewModels
{
    public class DishMenuItemViewModel:NotificationObject
    {
        //1.将Dish作为数据成员，有一个的设计模式
        //2.也可以选择继承Dish，是一个的设计模式，需要在Dish中继承NotificationObject，破坏了model和viewModel的分离
        //3.属性拷贝，在本类中重新声明相关变量
        public Dish Dish { get; set; }  //不需要实现NotificationObject，界面只读

        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }
    }
}
