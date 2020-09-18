using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoForms
{
    public class AutoFormsButtonAttribute : Attribute
    {
        public string Text { get; private set; }

        public string ButtonStyle { get; private set; }

        public string Command { get; private set; }

        public AutoFormsButtonAttribute(
            string text,
            string buttonStyle = null,
            string command = null)
        {
            Text = text;
            ButtonStyle = buttonStyle;
            Command = command;
        }
    }
}
