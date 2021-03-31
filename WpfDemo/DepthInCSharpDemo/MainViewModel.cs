using Common.Stylet;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepthInCSharpDemo
{
    public class MainViewModel : Conductor<Screen>, IMainViewModel
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
