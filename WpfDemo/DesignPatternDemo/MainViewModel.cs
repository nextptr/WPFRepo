using Common;
using DesignPatternDemo.common;
using DesignPatternDemo.StatusMachine;
using Stylet;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DesignPatternDemo
{
    
    public class MainViewModel : Conductor<Screen>
    {
        MainPageViewModel mainPage = new MainPageViewModel();
        private IPage CoverPageView = null;

        public MainViewModel(Router router)
        {
            this.ActivateItem(mainPage);
            router.Main = this;
        }
        public void Push(string name)
        {
            IPage page = mainPage.Items.FirstOrDefault((IPage item) => item != null && item.Name == name);
            Screen screen = page as Screen;
            if (screen != null)
            {
                this.ActivateItem(screen);
            }
            this.CoverPageView = page;
        }
        public void Top()
        {
            Screen screen = this.CoverPageView as Screen;
            if (screen != null)
            {
                this.DeactivateItem(screen);
            }
            this.CoverPageView = null;
            this.ActivateItem(mainPage);
        }
        //public void Top()
        //{
        //    Screen screen = this.CoverPageView as Screen;
        //    if (screen != null)
        //    {
        //        this.DeactivateItem(screen);
        //    }
        //    this.CoverPageView = null;
        //}
        public IPage GetPageInstance(string name)
        {
            return null;
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return this.mainPage.Items.FirstOrDefault((IPage p) => p.Name == name);
        }
    }
}
