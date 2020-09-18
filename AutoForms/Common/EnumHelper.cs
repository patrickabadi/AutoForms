using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AutoForms.Common
{
    public static class EnumHelper
    {
        public static Dictionary<int, string> ToDictionary(Type enumType)
        {
            if (!enumType.IsEnum) return null;

            bool isInt = Enum.GetUnderlyingType(enumType) == typeof(int);

            var result = new Dictionary<int, string>();
            foreach (var key in Enum.GetValues(enumType))
            {

                var name = Enum.GetName(enumType, key);
                var fieldInfo = enumType.GetField(name);
                object[] attributes = fieldInfo.GetCustomAttributes(false);
                if (attributes.Length > 0)
                {
                    var attrib = attributes[0] as DescriptionAttribute;

                    if (attrib != null && string.IsNullOrWhiteSpace(attrib.Description) == false)
                        name = attrib.Description;
                }

                int numericKey = isInt ? (int)key : Convert.ToInt32(key);

                result.Add(numericKey, name);
            }
            return result;
        }

        public static Type GetEnumType(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(name);
                if (type == null)
                    continue;
                if (type.IsEnum)
                    return type;
            }
            return null;
        }

        public static TEnum GetValue<TEnum>(string value) where TEnum : struct
        {
            if(Enum.TryParse(value, out TEnum result))
            {
                return result;
            }

            var vals = Enum.GetValues(typeof(TEnum));

            var val = vals.GetValue(0);
            foreach (var v in vals)
            {
                if ((int)v < (int)val)
                    val = v;
            }

            return (TEnum)val;
        }

        public static TEnum? GetNullableValue<TEnum>(string value) where TEnum : struct
        {
            if (Enum.TryParse(value, out TEnum result))
            {
                return result;
            }

            return null;
        }

        public static string GetDescription<TEnum>(int enumValue) where TEnum : struct
        {
            var enums = ToDictionary(typeof(TEnum));

            if (enums.TryGetValue(enumValue, out string resp))
            {
                return resp;
            }

            return enumValue.ToString();
        }

        public static string SetValue<TEnum>(TEnum enumValue) where TEnum : struct
        {
            int val = (int)Enum.Parse(typeof(TEnum), enumValue.ToString());
            if (val < 0)
                return null;

            return enumValue.ToString();
        }

        public static string SetNullableValue<TEnum>(TEnum? enumValue) where TEnum : struct
        {
            if(enumValue.HasValue)
            {
                int val = (int)Enum.Parse(typeof(TEnum), enumValue.Value.ToString());
                if (val < 0)
                    return null;
            }

            return enumValue?.ToString() ?? null;
        }
    }
}
