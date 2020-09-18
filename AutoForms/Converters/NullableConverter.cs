using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Converters
{
    public class NullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            var t = Nullable.GetUnderlyingType((Type)parameter);

            if(t != null && value == null)
            {
                if (targetType.IsValueType)
                    return Activator.CreateInstance(targetType);

                return null;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return value;

        }
    }
}
