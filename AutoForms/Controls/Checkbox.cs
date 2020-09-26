using AutoForms.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class Checkbox : ContentView
    {
        public enum LayoutOrientation
        {
            Default,
            Reversed
        }

        public static readonly BindableProperty CheckedProperty = BindableProperty.Create(nameof(Checked), typeof(bool), typeof(Checkbox), false, BindingMode.TwoWay,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ctrl = (Checkbox)bindable;

                ctrl._checkbox.Text = (bool)(newValue ?? false) ? AutoFormsConstants.Checkbox_Checked : AutoFormsConstants.Checkbox_Empty;

            });

        public bool Checked
        {
            get { return (bool)(GetValue(CheckedProperty) ?? false); }
            set { SetValue(CheckedProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Checkbox), null, BindingMode.Default,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ctrl = (Checkbox)bindable;
                ctrl._label.Text = (string)newValue;

                if (ctrl._grid.Children.Count < 2)
                    ctrl.UpdateLayout();
            });

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(nameof(LabelStyle), typeof(Style), typeof(Checkbox), null, BindingMode.Default,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var ctrl = (Checkbox)bindable;
                ctrl._label.Style = (Style)newValue;
            });

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        public static readonly BindableProperty CheckStyleProperty = BindableProperty.Create(nameof(CheckStyle), typeof(Style), typeof(Checkbox), null, BindingMode.Default,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var ctrl = (Checkbox)bindable;
                ctrl._checkbox.Style = (Style)newValue;
            });

        public Style CheckStyle
        {
            get { return (Style)GetValue(CheckStyleProperty); }
            set { SetValue(CheckStyleProperty, value); }
        }

        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(LayoutOrientation), typeof(Checkbox), LayoutOrientation.Default, BindingMode.Default,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var ctrl = (Checkbox)bindable;
                ctrl.UpdateLayout();
            });


        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Checkbox), null, BindingMode.Default, null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        public LayoutOrientation Orientation
        {
            get { return (LayoutOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        protected const double size = 40;

        

        protected Label _checkbox;
        protected Label _label;
        protected Grid _grid => Content as Grid;

        /// <summary>
        /// Default properties that can be overriden in derived styles
        /// </summary>
        public readonly static Style DefaultCheckStyle = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter{Property = Label.WidthRequestProperty, Value=size},
                new Setter{Property = Label.HeightRequestProperty, Value=size},
                new Setter{Property = Label.FontSizeProperty, Value=24}
            }
        };

        /// <summary>
        /// Default properties that can be overriden in derived styles
        /// </summary>
        public readonly static Style DefaultLabelStyle = new Style(typeof(Label))
        {
        };

        public Checkbox()
        {
            Content = new Grid
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                ColumnSpacing = 0,
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = GridLength.Auto},
                    new ColumnDefinition {Width = GridLength.Auto},
                }
            };

            //Application.Current.Resources.

            _checkbox = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Text = AutoFormsConstants.Checkbox_Empty,
                FontAttributes = FontAttributes.None,
                FontFamily = AutoFormsConstants.FontFamilyDefault,
                GestureRecognizers =
                {
                    new TapGestureRecognizer
                    {
                        Command = new Command(Toggle)
                    }
                },
                Style = DefaultCheckStyle
            };

            _label = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                GestureRecognizers =
                {
                    new TapGestureRecognizer
                    {
                        Command = new Command(Toggle)
                    }
                },
                Style = DefaultLabelStyle
            };

            UpdateLayout();
        }

        public void Toggle()
        {
            Checked = !Checked;

            if (Command != null && Command.CanExecute(null))
                Command.Execute(null);
        }

        protected void UpdateLayout()
        {
            _grid.Children.Clear();

            if (string.IsNullOrEmpty(_label.Text) == true)
            {
                Grid.SetColumn(_checkbox, 0);
            }
            else if (Orientation == LayoutOrientation.Default)
            {
                Grid.SetColumn(_checkbox, 0);
                Grid.SetColumn(_label, 1);
            }
            else
            {
                Grid.SetColumn(_checkbox, 1);
                Grid.SetColumn(_label, 0);
            }

            _grid.Children.Add(_checkbox);

            if (string.IsNullOrEmpty(_label.Text) == false)
            {
                _grid.Children.Add(_label);
                _grid.ColumnSpacing = 4;
            }
            else
            {
                _grid.ColumnSpacing = 0;
            }

            //_grid.DebugGrid(Color.Red);
        }




    }
}
