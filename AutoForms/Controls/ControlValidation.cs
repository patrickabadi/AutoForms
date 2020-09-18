using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ControlValidation : ContentView
    {
        public int Size => _validations.Count;

        private readonly Color _clrValidationBackground = Color.FromRgb(250, 215, 195);
        private readonly Color _clrValidationText = Color.FromRgb(237, 129, 68);

        private List<Validation> _validations = new List<Validation>();
        private StackLayout _stack;
        private Action _forceLayout;

        public ControlValidation(Action forceLayout)
        {
            _forceLayout = forceLayout;
            _stack = new StackLayout
            {
                Margin = new Thickness(3)
            };

            var grid = new Grid
            {
                RowSpacing = 0,
                IsVisible = false,
                IsEnabled = false,
                Children =
                {
                    new BoxView
                    {
                        CornerRadius = 3,
                        Color = _clrValidationBackground,
                    },
                    _stack
                }
            };

            Content = grid;
        }

        public ControlValidation(ControlBase control, Action forceLayout) :this(forceLayout)
        {
            BuildValidations(control);
        }

        public void Clear()
        {
            foreach(var validation in _validations)
            {
                validation.Label.IsVisible = validation.Label.IsEnabled = false;
            }

            Content.IsVisible = Content.IsEnabled = false;
        }

        public bool IsValid()
        {
            Clear();

            bool isValid = true;
            foreach (var validation in _validations)
            {
                var c = validation.Control as Entry;
                if (c != null )
                {
                    if (!validation.ValidationAttribute.IsValid(c.Text))
                    {
                        validation.Label.IsVisible = validation.Label.IsEnabled = true;
                        isValid = false;
                    }
                }
                else 
                {
                    var ed = validation.Control as Editor;
                    if (ed != null && !validation.ValidationAttribute.IsValid(ed.Text))
                    {
                        validation.Label.IsVisible = validation.Label.IsEnabled = true;
                        isValid = false;
                    }
                }                
            }

            Content.IsVisible = Content.IsEnabled = !isValid;

            return isValid;
        }

        private void BuildValidations(ControlBase control)
        {
            _validations.Clear();

            if(control.Groups.Any())
            {
                foreach(var item in control.Groups)
                {
                    var validations = item.GetValidations();
                    if (validations?.Count > 0)
                    {
                        validations.ForEach(x => AddValidation(x, item.Control, string.IsNullOrWhiteSpace(item.Label) ? item.Attribute.Placeholder : item.Label));
                    }
                }
            }
            else
            {
                var validations = control.GetValidations();
                if (validations?.Count > 0)
                {
                    validations.ForEach(x => AddValidation(x, control.Control, string.IsNullOrWhiteSpace(control.Label) ? control.Attribute.Placeholder : control.Label));
                }
            }

        }

        public void AddValidation(AutoFormsValidationAttribute validation, View control, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                title = "Item";

            //Debug.WriteLine($"AddValidation {validation.Type} for {title}");

            var label = new Label
            {
                IsVisible = false,
                IsEnabled = false,
                TextColor = _clrValidationText,
                Margin = new Thickness(3,0,0,3)
            };
            switch(validation)
            {
                case AutoFormsMaxLengthAttribute max when validation.Type == AutoFormsValidationAttribute.ValidationType.MaxLength:
                    label.Text = $"\"{title}\" is over maximum length of {max.Length} ";
                    MonitorEntryControl(true, validation, control, label);
                    break;
                case AutoFormsMinLengthAttribute min when validation.Type == AutoFormsValidationAttribute.ValidationType.MinLength:
                    label.Text = $"\"{title}\" must have at least {min.Length} characters";
                    MonitorEntryControl(true, validation, control, label);
                    break;
                case AutoFormsValidationAttribute _ when validation.Type == AutoFormsValidationAttribute.ValidationType.Numeric:
                    label.Text = $"\"{title}\" can only contain numeric values";
                    MonitorEntryControl(true, validation, control, label);
                    break;
                case AutoFormsValidationAttribute _ when validation.Type == AutoFormsValidationAttribute.ValidationType.Email:
                    label.Text = $"\"{title}\" must be a valid email address";
                    MonitorEntryControl(false, validation, control, label);
                    break;
                case AutoFormsValidationAttribute _ when validation.Type == AutoFormsValidationAttribute.ValidationType.Required:
                    label.Text = $"\"{title}\" is required";
                    MonitorEntryControl(false, validation, control, label);
                    break;
                default:
                    label = null;
                    break;
            }

            if (label == null)
                return;

            _stack.Children.Add(label);

            _validations.Add(new Validation(validation,control,label));
        }

        private void MonitorEntryControl(bool activeMonitoring, AutoFormsValidationAttribute attribute, View control, Label label)
        {
            var c = control as Entry;
            if(c != null)
            {
                if(activeMonitoring)
                {
                    c.TextChanged += (object sender, TextChangedEventArgs e) =>
                    {
                        var invalid = !attribute.IsValid(e.NewTextValue);
                        if (label.IsEnabled != invalid)
                            ToggleValidation(label);
                    };
                }

                c.Unfocused += (object sender, FocusEventArgs e) =>
                {
                    if(!e.IsFocused)
                    {
                        var invalid = !attribute.IsValid(c.Text);
                        if (label.IsEnabled != invalid)
                            ToggleValidation(label);
                    }
                };

                return;
            }

            var ed = control as Editor;
            if(ed != null)
            {
                if (activeMonitoring)
                {
                    ed.TextChanged += (object sender, TextChangedEventArgs e) =>
                    {
                        var invalid = !attribute.IsValid(e.NewTextValue);
                        if (label.IsEnabled != invalid)
                            ToggleValidation(label);
                    };
                }

                ed.Unfocused += (object sender, FocusEventArgs e) =>
                {
                    if (!e.IsFocused)
                    {
                        var invalid = !attribute.IsValid(ed.Text);
                        if (label.IsEnabled != invalid)
                            ToggleValidation(label);
                    }
                };
            }
        }

        private void ToggleValidation(Label label)
        {
            label.IsEnabled = label.IsVisible = !label.IsEnabled;

            var isEnabled = _validations.Any(x => x.Label.IsEnabled);

            Content.IsVisible = Content.IsEnabled = isEnabled;

            //Debug.WriteLine($"Toggle \"{label.Text}\" to {Content.IsVisible}");

            _forceLayout?.Invoke();
        }

        class Validation
        {
            public AutoFormsValidationAttribute ValidationAttribute;
            public View Control;
            public Label Label;

            public Validation (AutoFormsValidationAttribute validationAttribute, View control, Label label)
            {
                ValidationAttribute = validationAttribute;
                Control = control;
                Label = label;
            }
        }

    }
}
