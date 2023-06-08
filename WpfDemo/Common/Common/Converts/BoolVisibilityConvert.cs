using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Converts
{
    public class BoolVisibilityConvert : DependencyObject, IValueConverter
    {
        public static readonly BoolVisibilityConvert Instance;

        public static readonly BoolVisibilityConvert InverseInstance;

        public static readonly DependencyProperty TrueVisibilityProperty;

        public static readonly DependencyProperty FalseVisibilityProperty;

        public Visibility TrueVisibility
        {
            get
            {
                return (Visibility)GetValue(TrueVisibilityProperty);
            }
            set
            {
                SetValue(TrueVisibilityProperty, value);
            }
        }

        public Visibility FalseVisibility
        {
            get
            {
                return (Visibility)GetValue(FalseVisibilityProperty);
            }
            set
            {
                SetValue(FalseVisibilityProperty, value);
            }
        }

        static BoolVisibilityConvert()
        {
            TrueVisibilityProperty = DependencyProperty.Register("TrueVisibility", typeof(Visibility), typeof(BoolVisibilityConvert), new PropertyMetadata(Visibility.Visible));
            FalseVisibilityProperty = DependencyProperty.Register("FalseVisibility", typeof(Visibility), typeof(BoolVisibilityConvert), new PropertyMetadata(Visibility.Collapsed));
            Instance = new BoolVisibilityConvert();
            InverseInstance = new BoolVisibilityConvert();
            InverseInstance.TrueVisibility = Visibility.Collapsed;
            InverseInstance.FalseVisibility = Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag;
            if (value == null)
            {
                flag = false;
            }
            else if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is IEnumerable)
            {
                flag = ((IEnumerable)value).GetEnumerator().MoveNext();
            }
            else if (!(value is ValueType))
            {
                flag = true;
            }
            else
            {
                try
                {
                    flag = !value.Equals(System.Convert.ChangeType(0, value.GetType()));
                }
                catch
                {
                    flag = true;
                }
            }

            return flag ? TrueVisibility : FalseVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                throw new ArgumentException("Can't ConvertBack on BoolToVisibilityConverter when TargetType is not bool");
            }

            if (!(value is Visibility))
            {
                return null;
            }

            Visibility visibility = (Visibility)value;
            if (visibility == TrueVisibility)
            {
                return true;
            }

            if (visibility == FalseVisibility)
            {
                return false;
            }

            return null;
        }
    }
}
