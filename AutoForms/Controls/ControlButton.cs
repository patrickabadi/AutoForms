using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ControlButton : ControlBase
    {
        public ControlButton(ControlConfig config) : base(config)
        {
        }

        protected override View InitializeControl()
        {
            var g = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                ColumnDefinitions =
                    {
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    },
                RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto}
                    }
            };

            var v = _control = CreateControl(BindingName, _propertyType);
            if (v != null)
                g.Children.Add(v);

            return g;
        }

        public override View CreateControlLabel() => null;

        protected override View CreateControl(string bindingName, Type fieldType)
        {
            if (fieldType != typeof(ICommand))
            {
                Debug.WriteLine($"field:{bindingName} error. Wrong type {fieldType.ToString()} should be ICommand");
                return null;
            }

            var t = new Button
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = GetLocalizedString(Label),
                Style = _itemStyle,
            };

            t.SetBinding(Button.CommandProperty, new Binding(bindingName));

            if (_attribute.HeightRequest >= 0)
            {
                t.HeightRequest = _attribute.HeightRequest;
            }

            if (_attribute.ControlWidthRequest >= 0)
            {
                t.WidthRequest = _attribute.ControlWidthRequest;
            }

            return t;
        }
    }
}
