using AutoForms.Common;
using AutoForms.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ControlCombo : ControlBase
    {
        private Picker _picker;

        public ControlCombo(ControlConfig config) : base(config)
        {

        }

        protected override View CreateControl(string bindingName, Type fieldType)
        {
            _picker = new Picker
            {
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            SetBinding(bindingName, fieldType);

            return _picker;
        }

        public override void SetGroupedProperty(PropertyInfo property)
        {
            SetBinding(property.Name, property.PropertyType);
        }

        private void SetBinding(string bindingName, Type propertyType)
        {
            var t = Nullable.GetUnderlyingType(propertyType);

            if (t != null && t.IsEnum)
            {
                propertyType = t;

                var dict = EnumHelper.ToDictionary(propertyType);

                var items = new List<EnumItem>();
                items.Add(new EnumItem { Title = "Please Select", Value = -1 });

                int index = 0;

                foreach (var d in dict)
                {
                    if (d.Value != "Unspecified")
                    {
                        items.Add(new EnumItem { Title = d.Value, Value = index++ });
                    }
                }

                _picker.ItemsSource = items;

                _picker.SetBinding(
                Picker.SelectedIndexProperty,
                new Binding(BindingName, BindingMode.TwoWay, new EnumItemConverter(), propertyType));
            }
            else if (propertyType.IsEnum)
            {               
                var dict = EnumHelper.ToDictionary(propertyType);
                _picker.ItemsSource = dict.Values.ToList();

                _picker.SetBinding(
                Picker.SelectedIndexProperty,
                new Binding(BindingName, BindingMode.TwoWay, new EnumConverter(), propertyType));
            }
            else if (propertyType == typeof(int) || propertyType == typeof(int?))
            {
                _picker.SetBinding(Picker.SelectedIndexProperty, new Binding(bindingName, BindingMode.TwoWay, new NullableConverter(), propertyType));
            }
            else
            {
                _picker.SetBinding(Picker.ItemsSourceProperty, new Binding(bindingName));
            }
        }


        public class EnumItem
        {
            public string Title { get; set; }
            public int Value { get; set; }

            public override string ToString() => Title;

        }

        public class EnumItemConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
            {
                if (value == null)
                    return 0;

                var vals = Enum.GetValues((Type)parameter);
                int index = 1;
                foreach (var v in vals)
                {
                    if ((int)v == (int)value)
                    {
                        return index;
                    }
                    index++;
                }

                return 0;
            }

            public object ConvertBack(object value, Type targetType, object parameter,
                                      System.Globalization.CultureInfo culture)
            {
                int index = (int)value;
                if (index == 0)
                    return null;

                var vals = Enum.GetValues((Type)parameter);

                return vals.GetValue(index - 1);
            }


        }
    }
}
