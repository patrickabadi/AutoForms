using AutoForms.Controls;
using AutoForms.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Test.Controls
{
    // NOTE: I had an issue where basing a class off of my .NET standard library is causing an unknown error where I can no longer build for UWP
    // https://forums.xamarin.com/discussion/179510/vs-2017-starts-erroring-on-uwp-project-with-could-not-copy-the-file-obj-debug-not-found
    // https://github.com/xamarin/Xamarin.Forms/issues/4280
    // I resolved it by adding a reference to AutoForms libary in my UWP project

    public class DateAndTime : ControlCustom
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
