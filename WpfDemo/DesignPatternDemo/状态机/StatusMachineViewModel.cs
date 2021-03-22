using DesignPatternDemo.common;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DesignPatternDemo.StatusMachine
{
    public class StatusMachineViewModel : Screen, IPage
    {
        private string name = "状态机";
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

        public void btnBack()
        {
            var router = IoC.Get<IRouter>();
            router.GoBack();
        }
    }
}
