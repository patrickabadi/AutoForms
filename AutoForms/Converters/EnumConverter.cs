using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Converters
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return -1;

            if(targetType.IsEnum)
            {
                Enum enumValue = default(Enum);
                if (parameter is Type)
                {
                    enumValue = (Enum)Enum.Parse((Type)parameter, value.ToString());
                }
                return enumValue;
            }
            else
            {
                int returnValue = -1;
                if (parameter is Type)
                {
                    try
                    {
                        var vals = Enum.GetValues((Type)parameter);
                        var val = value.ToString();
                        int index = 0;
                        foreach (var d in vals)
                        {
                            if (val == d.ToString())
                            {
                                returnValue = index;
                                break;
                            }
                            index++;
                        }
                    }
                    catch
                    {
                        returnValue = -1;
                    }

                }
                return returnValue;
            }            
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            var target = targetType;
            var t = Nullable.GetUnderlyingType(targetType);
            if (t != null)
                target = t;

            if(target.IsEnum)
            {
                var index = (int)value;
                var vals = Enum.GetValues(target);
                var v = vals.GetValue(index);

                return (Enum)v;
            }
            else
            {
                int returnValue = 0;
                if (parameter is Type)
                {
                    returnValue = (int)Enum.Parse(target, value.ToString());
                }
                return returnValue;
            }
            
        }
    }

    
}
