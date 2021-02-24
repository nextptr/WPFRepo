using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    public class CheckConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string uriStr = "";
            PointCheck ck = (PointCheck)value;
            if (ck == PointCheck.Uncheck)
            {
                uriStr = string.Format(@"/Resources/empty.png");
            }
            if (ck == PointCheck.Bad)
            {
                uriStr = string.Format(@"/Resources/error.png");
            }
            if (ck == PointCheck.Ok)
            {
                uriStr = string.Format(@"/Resources/valid.png");
            }
            return new BitmapImage(new Uri(uriStr, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
