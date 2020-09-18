using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoForms.Common
{
    public static class StringHelper
    {
        public static string AppendComma(this object item, object addition)
        {
            return item.Append(addition, ", ");
        }

        public static string AppendSpace(this object item, object addition)
        {
            return item.Append(addition, " ");
        }

        public static string AppendHtmlBreak(this object item, object addition)
        {
            return item.Append(addition, "&#13;&#10;");
        }

        public static string Append(this object item, object addition, string spacer = "\r\n")
        {
            var val = item.Convert();

            var addval = addition.Convert();

            if (string.IsNullOrEmpty(addval))
                return val;

            if (string.IsNullOrEmpty(val))
                return addval;

            return val + spacer + addval;
        }

        public static string GetNumbers(this string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        public static string GetPhoneNumber(this string number, string countryCode = null, string extension = null)
        {
            var phoneNumber = number.GetNumbers().PhoneNumberFormatter().Append(extension, " ext:");

            return (!string.IsNullOrEmpty(countryCode) ? "+" + countryCode + " " : "") + phoneNumber;
        }

        public static string PhoneNumberFormatter(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = new System.Text.RegularExpressions.Regex(@"\D")
                .Replace(value, string.Empty);
            //value = value.TrimStart('1');
            if (value.Length == 7)
                return System.Convert.ToInt64(value).ToString("###-####");
            if (value.Length == 10)
                return System.Convert.ToInt64(value).ToString("###-###-####");
            if (value.Length > 10)
                return System.Convert.ToInt64(value)
                    .ToString("###-###-#### " + new String('#', (value.Length - 10)));
            return value;
        }

        public static string Convert(this object value, Type propertyType = null)
        {
            if(propertyType != null && value == null)
            {
                var t = Nullable.GetUnderlyingType(propertyType);

                if (t != null)
                {
                    if(t == typeof(bool))
                    {
                        return "NO";
                    }
                    else if(t == typeof(int) || t == typeof(float) || t == typeof(double) || t == typeof(decimal))
                    {
                        return "0";
                    }
                }
            }

            if (value == null)
                return null;

            var type = value.GetType();

            if (type.IsEnum)
            {
                var enums = EnumHelper.ToDictionary(type);

                if (enums.TryGetValue((int)value, out string resp))
                {
                    return resp;
                }
            }
            else if (type == typeof(string))
            {
                return (string)value;
            }
            else if(type == typeof(bool?))
            {
                var val = value as bool?;
                return val.HasValue ? val.Value ? "YES" : "NO" : "NO";
            }
            else if (type == typeof(bool))
            {
                var val = (bool)value;
                return val ? "YES" : "NO";
            }
            else if(type == typeof(DateTimeOffset?))
            {
                var val = value as DateTimeOffset?;
                return val.HasValue ? val.Value.Date.ToString("d") : null;
            }
            else if (type == typeof(DateTimeOffset))
            {
                var val = (DateTimeOffset)value;
                return val.Date.ToString("d");
            }
            else if (type == typeof(DateTime?))
            {
                var val = value as DateTime?;
                return val.HasValue ? val.Value.ToString("d") : null;
            }
            else if (type == typeof(DateTime))
            {
                var val = (DateTime)value;
                return val.ToString("d");
            }

            return value?.ToString();
        }

        public static object ConvertBack(string value, Type type)
        {
            var t = Nullable.GetUnderlyingType(type);

            if(t != null)
            {
                type = t;
            }

            if(value == null && type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            else if(value == null)
            {
                return null;
            }

            if (type == typeof(int))
            {
                int.TryParse(value, out int val);
                return val;
            }
            else if(type == typeof(float))
            {
                float.TryParse(value, out float val);
                return val;
            }
            else if(type == typeof(double))
            {
                double.TryParse(value, out double val);
                return val;
            }
            else if(type == typeof(decimal))
            {
                decimal.TryParse(value, out decimal val);
                return val;
            }
            return value;
        }
    }
}
