using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            var t = Nullable.GetUnderlyingType((Type)parameter);
            
            if(t != null && value == null)
            {
                return DateTime.UtcNow;
            }
            else if(t != null && t == typeof(DateTimeOffset))
            {
                var offset = (DateTimeOffset?)value;
                return offset.Value.DateTime;
            }
            else if ((Type)(parameter) == typeof(DateTimeOffset))
            {
                var offset = (DateTimeOffset)value;
                return offset.DateTime;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var t = Nullable.GetUnderlyingType((Type)parameter);
            if(t != null && t == typeof(DateTimeOffset) || (Type)(parameter) == typeof(DateTimeOffset))
            {
                return new DateTimeOffset((DateTime)value);
            }
            else
            {
                return value;
            }
        }
    }
}
