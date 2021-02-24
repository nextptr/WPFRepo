using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Maxwell.LaserCutter.Common.Converts
{
    public class BoolColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? Brushes.LimeGreen : Brushes.Red;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

