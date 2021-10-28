using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsDemo
{
    public class ResultAttach
    {
        public static bool GetResult(DependencyObject obj)
        {
            return (bool)obj.GetValue(ResultProperty);
        }

        public static void SetResult(DependencyObject obj, bool value)
        {
            obj.SetValue(ResultProperty, value);
        }

        // Using a DependencyProperty as the backing store for Result.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.RegisterAttached("Result", typeof(bool), typeof(ResultAttach), new PropertyMetadata(OnResultChanged));
       
        private static void OnResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as ContentControl;
            if (element != null)
            {
                var rt = (bool)e.NewValue;
                if (rt == true)
                {
                    element.Content = element.Tag + "ON";
                    element.Background = Brushes.Red;
                }
                else
                {
                    element.Content = element.Tag + "OFF";
                    element.Background = Brushes.Blue;
                }
            }
        }

    }
}
