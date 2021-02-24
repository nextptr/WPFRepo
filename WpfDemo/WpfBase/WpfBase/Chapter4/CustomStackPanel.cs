using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfBase.Chapter4
{
    public class CustomStackPanel:StackPanel
    {
        public static readonly DependencyProperty MinDateProperty;

        static CustomStackPanel()
        {
            MinDateProperty = DependencyProperty.Register("MinDate", typeof(DateTime), typeof(CustomStackPanel),
                new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.Inherits));
        }

        public DateTime MinDate
        {
            get
            {
                return (DateTime)GetValue(MinDateProperty);
            }
            set
            {
                SetValue(MinDateProperty, value);
            }
        }
    }
}
