using AutoForms.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ControlCheckbox : ControlBase
    {
        protected Checkbox _checkbox;

        public ControlCheckbox(ControlConfig config) : base(config)
        {

        }

        protected override View CreateControl(string bindingName, Type fieldType)
        {
            if (fieldType != typeof(bool) && fieldType != typeof(bool?))
            {
                Debug.WriteLine($"field:{bindingName} error. Wrong type {fieldType.ToString()} should be bool");
                return null;
            }

            _checkbox = new Checkbox
            {

            };

            _checkbox.SetBinding(Checkbox.CheckedProperty, new Binding(bindingName, BindingMode.TwoWay, new NullableConverter(), fieldType));

            return _checkbox;
        }

        protected override void OnSelectedText()
        {
            _checkbox.Toggle();
        }
    }
}
