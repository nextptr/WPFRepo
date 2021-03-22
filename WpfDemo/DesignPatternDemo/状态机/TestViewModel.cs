using DesignPatternDemo.common;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternDemo.StatusMachine
{
    public class TestViewModel : Screen, IPage
    {
        public string Name { get; set; } = "测试";

        public void btnBack()
        {
            var router = IoC.Get<IRouter>();
            router.GoBack();
        }
    }
}
