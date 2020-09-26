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
    public class ControlRadio : ControlBase
    {
        protected RadioButton _radioButton;

        protected bool _hideHeader;

        public ControlRadio(bool hideHeader, ControlConfig config) : base(config)
        {
            _hideHeader = hideHeader;
        }

        protected override View InitializeControl()
        {
            //_attribute.Orientation = AutoFormsOrientation.Horizontal;

            var propertyType = _propertyType;
            var t = Nullable.GetUnderlyingType(propertyType);
            if (t != null)
                propertyType = t;

            if (!propertyType.IsEnum)
            {
                Debug.WriteLine($"field:{BindingName} error. Wrong type {propertyType.ToString()} should be an Enum");
                return null;
            }

            double layoutPercentage = _attribute.LayoutHorizontalPercentageOverride == -1
                                          ? _config.LayoutHorizontalPercentage
                                          : _attribute.LayoutHorizontalPercentageOverride;

            var g = new Grid
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                ColumnDefinitions =
                    {
                        new ColumnDefinition {Width = new GridLength(layoutPercentage, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(1-layoutPercentage, GridUnitType.Star)},
                    },
                RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto},
                        new RowDefinition {Height = GridLength.Auto},
                    },

            };

            if(string.IsNullOrEmpty(Label) == false)
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

            //g.DebugGrid(Color.Blue);

            AddControlContainers(BindingName, propertyType, _attribute.Orientation);

            return g;
        }

        private void AddControlContainers(string bindingName, Type propertyType, AutoFormsOrientation orientation)
        {
            var dict = EnumHelper.ToDictionary(propertyType);

            var inline = false;
            if (_attribute is AutoFormsRadioButtonAttribute rba)
                inline = rba.Inline;

            var g = new RadioGroup
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.StartAndExpand,
            };
            g.SetBinding(
                RadioGroup.SelectedIndexProperty,
                new Binding(bindingName, BindingMode.TwoWay, new EnumConverter(), propertyType));

            var h = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.StartAndExpand,
            };

            if (orientation == AutoFormsOrientation.Horizontal)
            {
                Grid.SetRow(g, 1);
                Grid.SetColumn(g, 1);
                Grid.SetRow(h, 0);
                Grid.SetColumn(h, 1);
            }
            else
            {
                Grid.SetRow(g, 1);
                Grid.SetColumn(g, 1);
                Grid.SetRow(h, 1);
                Grid.SetColumn(h, 0);
            }


            foreach (var kvp in dict)
            {

                var radio = new RadioButton
                {
                    HorizontalOptions = inline ? LayoutOptions.End : LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };

                var lbl = new Label
                {
                    Style = LabelStyle,
                    Text = kvp.Value,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Fill,
                    HorizontalTextAlignment = inline ? TextAlignment.Start : TextAlignment.Center,
                    LineBreakMode = LineBreakMode.WordWrap,
                    GestureRecognizers =
                    {
                        new TapGestureRecognizer
                        {
                            Command = new Command(()=>radio.SelectItem())
                        }
                    }
                };

                if (_attribute.Orientation == AutoFormsOrientation.Horizontal)
                {
                    if (!inline)
                    {
                        g.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        h.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        Grid.SetColumn(radio, g.ColumnDefinitions.Count - 1);
                        Grid.SetColumn(lbl, g.ColumnDefinitions.Count - 1);
                    }
                    else
                    {
                        g.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        Grid.SetColumn(radio, g.ColumnDefinitions.Count - 1);
                        g.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        Grid.SetColumn(lbl, g.ColumnDefinitions.Count - 1);
                    }

                }
                else
                {
                    // have to force them the same height
                    h.RowDefinitions.Add(new RowDefinition { Height = 40 });
                    g.RowDefinitions.Add(new RowDefinition { Height = 40 });

                    Grid.SetRow(radio, g.RowDefinitions.Count - 1);
                    Grid.SetRow(lbl, g.RowDefinitions.Count - 1);

                    if (!inline)
                    {
                        lbl.HorizontalTextAlignment = TextAlignment.End;
                        radio.HorizontalOptions = LayoutOptions.StartAndExpand;

                    }
                    else
                    {
                        if (g.ColumnDefinitions.Count < 2)
                            g.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        if (g.ColumnDefinitions.Count < 2)
                            g.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                        Grid.SetColumn(radio, 0);
                        Grid.SetColumn(lbl, 1);
                    }
                }

                if (!inline)
                    h.Children.Add(lbl);
                else
                    g.Children.Add(lbl);
                g.Children.Add(radio);
            }

            if (_attribute.Orientation == AutoFormsOrientation.Horizontal)
            {
                SetHorizontalLayoutOptions(g, _attribute.HorizontalControlOptions);
                SetHorizontalLayoutOptions(h, _attribute.HorizontalControlOptions);
            }

            if (_attribute.ControlWidthRequest > 0)
            {
                g.WidthRequest = _attribute.ControlWidthRequest;
                h.WidthRequest = _attribute.ControlWidthRequest;
            }


            var c = Content as Grid;
            c.Children.Add(g);

            if (!_hideHeader && !inline)
                c.Children.Add(h);
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
