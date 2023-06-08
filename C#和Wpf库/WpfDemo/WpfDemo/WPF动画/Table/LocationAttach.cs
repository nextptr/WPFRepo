using System.Windows;
using System.Windows.Controls;

namespace WpfDemo.WPF动画.Table
{
    public class LocationAttach
    {

        // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.RegisterAttached("Location", typeof(Point), typeof(LocationAttach), new PropertyMetadata(new Point(), OnLocationChanged));

        private static void OnLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element != null)
            {
                var newloc = (Point)e.NewValue;
                element.SetValue(Canvas.LeftProperty, newloc.X);
                element.SetValue(Canvas.TopProperty, newloc.Y);
            }
        }

        public static void SetLocation(DependencyObject element, Point value)
        {
            element.SetValue(LocationProperty, value);
        }

        public static Point GetLocation(DependencyObject element)
        {
            return (Point)element.GetValue(LocationProperty);
        }
    }
}
