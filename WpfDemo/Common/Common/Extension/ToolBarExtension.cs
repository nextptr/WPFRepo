using System.Windows;
using System.Windows.Controls;

namespace  Common.Extension
{
    public static class ToolBarExtension
    {
        public static void ShowOverflow(this ToolBar tlb, bool show)
        {
            FrameworkElement fe = tlb.Template.FindName("OverflowGrid", tlb) as FrameworkElement;
            if (fe != null)
            {
                fe.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
