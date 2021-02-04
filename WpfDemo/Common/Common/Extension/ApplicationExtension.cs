using System;
using System.Windows;
using System.Windows.Threading;

namespace Common.Extension
{
    public static class ApplicationExtension
    {
        public static void DoEvents(this Application me)
        {
            me.Dispatcher.Invoke(DispatcherPriority.Background,
                                 new Action(delegate { }));
        }
    }
}
