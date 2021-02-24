using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfBase.Chapter4
{
    //附加属性
    //https://www.cnblogs.com/DebugLZQ/p/3153098.html
    public class RotationManager:DependencyObject
    {
        //此时只是添加了一个附加属性，给它付了一个初值，当值改变时并没有添加相应的处理逻辑
        public static double GetAngle(DependencyObject obj)
        {
            return (double)obj.GetValue(AngleProperty);
        }
        public static void SetAngle(DependencyObject obj,double value)
        {
            obj.SetValue(AngleProperty, value);
        }
        //public static readonly DependencyProperty AngleProperty =
        //    DependencyProperty.RegisterAttached("Angle", typeof(double), typeof(RotationManager), new PropertyMetadata(0.0)); //只是添加了一个附加属性
       
            
        //为了处理了xaml中的赋值操作，需要在添加相应的值改变事件及事件处理逻辑
        public static readonly DependencyProperty AngleProperty =
          DependencyProperty.RegisterAttached("Angle", typeof(double), typeof(RotationManager), new PropertyMetadata(0.0,OnAngleChanged));    //添加了一个处理事件的回调函数

        private static void OnAngleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as UIElement;
            if (element != null)
            {
                element.RenderTransformOrigin = new Point(0.5, 0.5);
                element.RenderTransform = new RotateTransform((double)e.NewValue);
            }
        }

    }
}
