using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp.Convert
{
    public class TwoWayConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = value.ToString();
            if (color == "Red")
            {
                return Brushes.Red;
            }
            if (color == "Green")
            {
                return Brushes.Green;
            }
            if (color == "Blue")
            {
                return Brushes.Blue;
            }
            if (color == "Black")
            {
                return Brushes.Black;
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush bsh = (SolidColorBrush)value;
            if (bsh.Color.Equals(Brushes.Red.Color))
            {
                return "Red";
            }
            if (bsh.Color.Equals(Brushes.Green.Color))
            {
                return "Green";
            }
            if (bsh.Color.Equals(Brushes.Blue.Color))
            {
                return "Blue";
            }
            if (bsh.Color.Equals(Brushes.Black.Color))
            {
                return "Black";
            }
            if (bsh.Color.Equals(Brushes.White.Color))
            {
                return "White";
            }
            return "NUll";
        }
    }
}
