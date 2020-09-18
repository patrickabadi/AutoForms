using AutoForms.Common;
using AutoForms.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ControlSelectButton : ControlBase
    {
        public ControlSelectButton(ControlConfig config) : base(config)
        {
          
        }

        protected override View InitializeControl()
        {
            var propertyType = _propertyType;
            var t = Nullable.GetUnderlyingType(propertyType);
            if (t != null)
                propertyType = t;

            if (!propertyType.IsEnum)
            {
                Debug.WriteLine($"field:{BindingName} error. Wrong type {propertyType.ToString()} should be an Enum");
                return null;
            }

            Grid g;
            if (_attribute.Orientation == AutoFormsOrientation.Horizontal)
            {
                g = new Grid
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition {Width = new GridLength(_config.LayoutHorizontalPercentage, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength((1-_config.LayoutHorizontalPercentage), GridUnitType.Star)},
                    },
                    RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto}
                    }
                };
            }
            else
            {
                g = new Grid
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition {Width = GridLength.Star}
                    },
                    RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto},
                        new RowDefinition {Height = GridLength.Auto},
                    },
                };
            }

            if (string.IsNullOrEmpty(Label) == false)
            {
                View label;

                if (_attribute.Orientation == AutoFormsOrientation.Horizontal)
                {
                    label = CreateLabel(Label, LabelStyle, _attribute.Orientation, 0, 1);
                }
                else
                {
                    label = CreateLabel(Label, LabelStyle, _attribute.Orientation, 0, 0);
                }

                g.Children.Add(label);
            }
            
            Content = g;

            AddControlContainers(BindingName, propertyType, _attribute.Orientation);

            return g;
        }

        private void AddControlContainers(string bindingName, Type propertyType, AutoFormsOrientation orientation)
        {
            var dict = EnumHelper.ToDictionary(propertyType);

            var g = new SelectButtonGroup()
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.StartAndExpand,
                ColumnDefinitions =

                {
                   new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                   new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                   new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                },
                ColumnSpacing = 10,
                RowSpacing = 10
            };
            g.SetBinding(
                SelectButtonGroup.SelectedIndexProperty, 
                new Binding(bindingName, BindingMode.TwoWay, new EnumConverter(), propertyType));

            for (var i = 0; i < (dict.Count / 3) + 1; i++)
            {
                g.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
            }


            if (orientation == AutoFormsOrientation.Horizontal)
            {
                Grid.SetRow(g, 0);
                Grid.SetColumn(g, 1);
               
            }
            else
            {
                Grid.SetRow(g, 1);
                Grid.SetColumn(g, 0);
            }

           
            
            var row = 0;
            var col = 0;
            foreach (var kvp in dict)
            {
                if (col >= g.ColumnDefinitions.Count)
                {
                    col = 0; //set the column
                    row++;
                }
                var button = new SelectButton()
                {
                  HorizontalOptions = LayoutOptions.FillAndExpand,
                  Text = kvp.Value,
                  Style = _itemStyle,
                   
                };

                Grid.SetColumn(button, col++);
                Grid.SetRow(button, row);

                
                g.Children.Add(button);
            }


            var c = Content as Grid;
            c.Children.Add(g);

          
        }

        protected override View CreateControl(string bindingName, Type fieldType)
        {
            return null;
        }

        public override View GetControlContainer()
        {
            return this;
        }

    }
}
