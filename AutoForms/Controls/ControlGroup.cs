using AutoForms.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ControlGroup : ControlBase 
    {
        protected Grid _grid;
        protected Grid _controlGrid;
        protected const double _spacing = 30;

        public ControlGroup(ControlConfig config):base(config)
        { }

        protected override View CreateControl(string bindingName, Type fieldType) => null;

        protected override View InitializeControl()
        {
            _grid = new Grid
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto},
                        new RowDefinition {Height = GridLength.Auto},
                    },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = _spacing},
                    new ColumnDefinition {Width = GridLength.Auto},
                    new ColumnDefinition {Width = GridLength.Star},
                }
            };

            var b = new BoxView
            {
                Color = _config.SeparatorColor,
                WidthRequest = 8,
            };
            Grid.SetRow(b, 0);
            Grid.SetColumn(b, 1);
            _grid.Children.Add(b);

            if(string.IsNullOrEmpty(Label) == false)
            {
                var style = _itemStyle;
                if(style == null)
                {
                    style = LabelStyle;
                }

                var v = CreateLabel(Label, style, AutoFormsOrientation.Vertical, 0, 0) as Label;                
                if(v != null)
                {
                    Grid.SetColumnSpan(v, 3);
                    _grid.Children.Add(v);
                }
            }

            _controlGrid = new Grid
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto},
                    }
            };
            Grid.SetRow(_controlGrid, 0);
            Grid.SetColumn(_controlGrid, 2);
            _grid.Children.Add(_controlGrid);

            return _grid;
        }

        public override void SetGroupedProperty(PropertyInfo property)
        {
            var attributes = property.GetAttributes<AutoFormsAttribute>();

            var attribute = GetFilteredAttribute(_config.Filter, attributes);
            if (attribute == null)
            {
                Debug.WriteLine($"ControlGroup.SetGroupedProperty - unable to create filtered control of name: {property.Name}");
                return;
            }

            var config = new ControlConfig(
                _config.LayoutHorizontalPercentage,
                attribute,
                _config.LabelStyle,
                _config.EditorStyle,
                _config.SeparatorColor,
                property,
                _config.Filter);

            var v = CreateControl(config);
            if(v == null)
            {
                Debug.WriteLine($"ControlGroup.SetGroupedProperty - unable to create control of name: {property.Name}");
                return;
            }

            Groups.Add(v);

            v.Initialize();

            _controlGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            Grid.SetRow(v, _controlGrid.RowDefinitions.Count - 1);
            _controlGrid.Children.Add(v);
        }

    }
}
