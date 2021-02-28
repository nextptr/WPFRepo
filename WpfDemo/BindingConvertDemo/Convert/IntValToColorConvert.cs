using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
namespace BindingConvertDemo.Convert
{
    class IntValToColorConvert : IValueConverter
    {
        public object Convert(object Value, Type targetType, object parameter, CultureInfo culture)
        {
            if ("0" == Value.ToString())
            {
                return Brushes.Red;
            }
            if ("1" == Value.ToString())
            {
                return Brushes.LimeGreen;
            }
            if ("2" == Value.ToString())
            {
                return Brushes.Yellow;
            }
            if ("3" == Value.ToString())
            {
                return Brushes.Pink;
            }
            else
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
