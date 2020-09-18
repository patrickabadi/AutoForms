using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoForms
{
    public class AutoFormsListItemAttribute : AutoFormsAttribute
    {
        public double Value { get; private set; }
        public GridUnitType GridType { get; private set; }
        public TextAlignment HorizontalHeaderAlignment { get; private set; }
        public TextAlignment HorizontalItemAlignment { get; private set; }
        public object SortValue { get; private set; }

        public AutoFormsListItemAttribute(
            string label = null,
            double value = 1,
            GridUnitType gridType = GridUnitType.Star,
            string itemStyle = null,
            string labelStyleOverride = null,
            TextAlignment horizontalHeaderAlignment = TextAlignment.Start,
            TextAlignment horizontalItemAlignment = TextAlignment.Start,
            AutoFormsType type = AutoFormsType.Auto,
            AutoFormsOrientation orientation = AutoFormsOrientation.Horizontal,
            AutoFormsLayoutOptions horizontalLabelOptions = AutoFormsLayoutOptions.Default,
            AutoFormsLayoutOptions horizontalControlOptions = AutoFormsLayoutOptions.Default,
            int controlWidthRequest = -1,
            string placeholder = null,
            int heightRequest = -1,
            string isVisible = null,
            string isEnabled = null,
            string isFocused = null,
            int filter = 0,
            string[] grouped = null,
            object sortValue = null) :base(
                label, type, orientation, horizontalLabelOptions, horizontalControlOptions, controlWidthRequest, itemStyle, labelStyleOverride, placeholder, heightRequest, isVisible, isEnabled, isFocused, filter, grouped: grouped)
        {
            Value = value;
            GridType = gridType;
            HorizontalHeaderAlignment = horizontalHeaderAlignment;
            HorizontalItemAlignment = horizontalItemAlignment;
            SortValue = sortValue;
        }
    }
}
