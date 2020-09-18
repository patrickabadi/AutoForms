using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForms
{
    public class AutoFormsCustomAttribute:AutoFormsAttribute 
    {
        public string CustomControlType { get; private set; }

        public AutoFormsCustomAttribute(
            string customControlType,
            string label = null,            
            AutoFormsOrientation orientation = AutoFormsOrientation.Default,
            AutoFormsLayoutOptions horizontalLabelOptions = AutoFormsLayoutOptions.Default,
            AutoFormsLayoutOptions horizontalControlOptions = AutoFormsLayoutOptions.Default,
            double controlWidthRequest = -1,
            string itemStyle = null,
            string labelStyleOverride = null,
            string placeholder = null,
            double heightRequest = -1,
            string isVisible = null,
            string isEnabled = null,
            string isFocused = null,
            int filter = 0,
            double paddingLeft = 0,
            double paddingRight = 0,
            double paddingTop = 0,
            double paddingBottom = 0,
            double layoutHorizontalPercentageOverride = -1,
            string[] grouped = null
            ):
            base(label, AutoFormsType.Custom, orientation, horizontalLabelOptions, horizontalControlOptions, controlWidthRequest, itemStyle, labelStyleOverride, placeholder, heightRequest, isVisible, isEnabled, isFocused, filter,
                paddingLeft, paddingRight, paddingTop, paddingBottom, layoutHorizontalPercentageOverride, grouped)
        {
            CustomControlType = customControlType;
        }
    }
}
