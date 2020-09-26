using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AutoForms.Test.DomainModels
{
    public class MoreModel
    {     
        [AutoForms("A label", itemStyle: "DefaultLabelStyle")]
        public object ButtonLabel { get; set; }

        [AutoForms("", AutoFormsType.Label)]
        public string ChangableLabel { get; set; } = "A dynamic label (you can change it at runtime)";

        [AutoForms("Click Me", itemStyle:"DefaultButtonStyle")]
        public ICommand MyButton { get; set; }

        [AutoForms("Checkbox 1")]
        public bool Check1 { get; set; }

        [AutoForms("Checkbox 2")]
        public bool Check2 { get; set; }

        [AutoForms("Checkbox 3")]
        public bool Check3 { get; set; }

        [AutoForms("Combo box from list", AutoFormsType.Combo, grouped:new string[] { nameof(SelectedComboBoxIndex) })]
        public List<string> PickerStrings { get; set; } = new List<string> { "First", "Second", "Third" };

        [AutoForms()]
        public int SelectedComboBoxIndex { get; set; } = 2;

        public enum EnumType
        {
            First,
            Second,
            Third,
        }

        [AutoForms("Combo box from enum")]
        public EnumType? MyEnum { get; set; }

        [AutoForms("Pick a date")]
        public DateTime? MyDate { get; set; } = DateTime.UtcNow;

        public enum EnumRadioType
        {
            One,
            Two
        }

        [AutoFormsRadioButton("Radio buttons", AutoFormsOrientation.Horizontal)]
        public EnumRadioType? MyRadioEnum { get; set; }

        [AutoForms("Select Buttons", AutoFormsType.SelectButton, itemStyle: "DefaultSelectButtonStyle")]
        public EnumType? MySelectButton { get; set; }
    }
}
