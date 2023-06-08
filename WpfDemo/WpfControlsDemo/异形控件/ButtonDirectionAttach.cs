using System.Windows;

namespace WpfControlsDemo
{
    public enum ShapeDirectionType
    {
        Up,
        Down,
        Left,
        Right,
        Cent
    }
    public class ButtonDirectionAttach
    {
        public static ShapeDirectionType GetShapeDirection(DependencyObject element)
        {
            return (ShapeDirectionType)element.GetValue(ShapeDirectionProperty);
        }

        public static void SetShapeDirection(DependencyObject ele, object value)
        {
            ele.SetValue(ShapeDirectionProperty, value);
        }

        public static readonly DependencyProperty ShapeDirectionProperty = DependencyProperty.RegisterAttached("ShapeDirection", typeof(ShapeDirectionType), typeof(ButtonDirectionAttach));

    }
}
