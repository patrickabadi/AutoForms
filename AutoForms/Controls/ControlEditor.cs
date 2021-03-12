using AutoForms.Behaviors;
using AutoForms.Common;
using AutoForms.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ControlEditor : ControlBase
    {
        public Style EditorStyle => _config.EditorStyle;

        public ControlEditor(ControlConfig config) :base(config)
        {
        }

        protected override View CreateControl(string bindingName, Type fieldType)
        {           
            Keyboard kb = Keyboard.Default;

            bool isNumeric = false;
            bool isDecimal = false;

            if(IsNumericField(fieldType))
            {
                kb = Keyboard.Numeric;
                isNumeric = true;
                isDecimal = IsDecimalField(fieldType);
            }

            var maxLength = _property.GetAttribute<AutoFormsMaxLengthAttribute>()?.Length ?? 0;

            isNumeric |= _property.GetAttribute<AutoFormsNumericAttribute>() != null;

            InputView t;
            if(_attribute.HeightRequest <= 0)
            {
                t = new Entry
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Style = EditorStyle,
                    Placeholder = GetLocalizedString(_attribute.Placeholder),
                    Keyboard = kb,
                };
                t.SetBinding(Entry.TextProperty, new Binding(bindingName, BindingMode.TwoWay, new DisplayConverter()));

                if (isNumeric)
                {
                    t.Behaviors.Add(new NumericInputBehavior<Entry>(isDecimal));
                }
            }
            else
            {
                t = new Editor
                {
                    Style = EditorStyle,
                    Placeholder = GetLocalizedString(_attribute.Placeholder),
                    HeightRequest = _attribute.HeightRequest,
                    Keyboard = kb,
                };

                t.SetBinding(Editor.TextProperty, new Binding(bindingName, BindingMode.TwoWay, new DisplayConverter()));

                if (isNumeric)
                {
                    t.Behaviors.Add(new NumericInputBehavior<Editor> (isDecimal));
                }
            }

            // adding in max length safety here
            if (maxLength > 0)
                t.MaxLength = maxLength;

            

            return t;
        }

        protected bool IsDecimalField(Type fieldType)
        {
            var types = new List<Type>
            {
                typeof(double),
                typeof(double?),
                typeof(float),
                typeof(float?),
                typeof(decimal),
                typeof(decimal?)
            };

            return types.Any(x => x == fieldType);
        }

        protected bool IsNumericField(Type fieldType)
        {
            var types = new List<Type>
            {
                typeof(int),
                typeof(int?),
                typeof(double),
                typeof(double?),
                typeof(float),
                typeof(float?),
                typeof(decimal),
                typeof(decimal?)
            };

            return types.Any(x => x == fieldType);
        }
    }
}
