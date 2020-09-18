using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AutoForms.Common
{
    public static class AttributeHelper
    {
        public static List<(PropertyInfo, T [])> GetPropertyAttributes<T>(this object obj) where T : class
        {
            return GetPropertyAttributes<T>(obj.GetType());
        }

        public static List<(PropertyInfo, T [])> GetPropertyAttributes<T> (Type type) where T : class
        {
            var list = new List<(PropertyInfo, T [])>();

            var props = type.GetProperties();

            foreach(var prop in props)
            {
                var attribs = prop.GetAttributes<T>();
                if (attribs == null || attribs.Length == 0)
                    continue;

                list.Add((prop, attribs));
            }

            return list;
        }

        public static T [] GetAttributes<T>(this PropertyInfo prop) where T : class
        {
            if (prop == null)
                return null;

            var attribs = prop.GetCustomAttributes(typeof(T), false) as T[];
            if (attribs == null || attribs.Length == 0)
                return null;

            return attribs;
        }

        public static T GetAttribute<T>(this PropertyInfo prop) where T : class
        {
            if (prop == null)
                return null;

            var attribs = prop.GetAttributes<T>();
            if (attribs == null || attribs.Length == 0)
                return null;

            return attribs[0];
        }

        public static T [] GetAttributes<T>(this Type type) where T : class
        {
            if (type == null)
                return null;

            var attribs = type.GetCustomAttributes(typeof(T), false) as T[];
            if (attribs == null || attribs.Length == 0)
                return null;

            return attribs;
        }

        public static T GetAttribute<T>(this Type type) where T : class
        {
            if (type == null)
                return null;

            var attribs = GetAttributes<T>(type);
            if (attribs == null || attribs.Length == 0)
                return null;

            return attribs[0];
        }
    }
}
