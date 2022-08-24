using CVbox.Common;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVbox.Panel.Home
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
            IPage page = mainPage.PageItems.Values.FirstOrDefault((IPage item) => item != null && item.Name == name);
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
        public IPage GetPageInstance(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return this.mainPage.PageItems.Values.FirstOrDefault((IPage p) => p.Name == name);
        }
    }
}
