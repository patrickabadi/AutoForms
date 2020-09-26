using AutoForms.Controls;
using AutoForms.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Test.Controls
{
    public class DateAndTime: ControlCustom
    {
        Grid _grid => Content as Grid;

        public DateAndTime() : base()
        { }

        protected override View CreateControl(string bindingName, Type fieldType)
        {
            if (fieldType != typeof(DateTime) &&
                fieldType != typeof(DateTimeOffset) &&
                fieldType != typeof(DateTime?) &&
                fieldType != typeof(DateTimeOffset?))
            {
                Debug.WriteLine($"field:{bindingName} error. Wrong type {fieldType.ToString()} should be DateTime or DateTimeOffset");
                return null;
            }

            var date = new DatePicker
            {
                Style = _itemStyle
            };
            date.SetBinding(DatePicker.DateProperty, new Binding(bindingName, BindingMode.TwoWay, new DateTimeConverter(), fieldType));

            var time = new TimePicker
            {
                Style = _itemStyle
            };
            time.SetBinding(TimePicker.TimeProperty, new Binding(bindingName, BindingMode.TwoWay, new DateTimeConverter(), fieldType));
            Grid.SetRow(time, 1);

            var g = new Grid
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto},
                        new RowDefinition {Height = GridLength.Auto},
                    },
                Children =
                {
                    date,
                    time,
                }
            };

            return g;
        }

    }
}
