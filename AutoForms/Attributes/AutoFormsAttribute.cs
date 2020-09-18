using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForms
{
    public class AutoFormsAttribute : AutoFormsFilteredAttribute
    {
        /*The label for each control*/
        public string Label { get; private set; }
        /*Placeholder value on any edit control*/
        public string Placeholder { get; private set; }
        /*Specific Styling for this control*/
        public string ItemStyle { get; private set; }
        /*Label Style*/
        public string LabelStyleOverride { get; private set; }
        /*Type of control to create.  Auto will create the control by the type of variable it's bound to*/
        public AutoFormsType Type { get; private set; }
        /*get set visibility on this control*/
        public string IsVisible { get; set; }
        /*get set enabled on this control*/
        public string IsEnabled { get; set; }
        /*get focused on this control*/
        public string IsFocused { get; set; }
        /*Orientation flow of the label+control*/
        public AutoFormsOrientation Orientation { get; private set; }
        /*Horizontal positioning of the label*/
        public AutoFormsLayoutOptions HorizontalLabelOptions { get; private set; }
        /*Horizontal positioning of the control*/
        public AutoFormsLayoutOptions HorizontalControlOptions { get; private set; }
        /*Full width request of this control*/
        public double ControlWidthRequest { get; private set; }
        /*HeightRequest of the control*/
        public double HeightRequest { get; private set; }
        /*Padding*/
        public double PaddingLeft { get; private set; }
        /*Padding*/
        public double PaddingRight { get; private set; }
        /*Padding*/
        public double PaddingTop { get; private set; }
        /*Padding*/
        public double PaddingBottom { get; private set; }
        /*When Orientation is horizonal, this is the percentage space taken up by label/control*/
        public double LayoutHorizontalPercentageOverride { get; private set; }
        /*When you need to group several variables to one control/sub control. Eg. doing a horizontal control*/
        public string[] Grouped { get; private set; }

        public AutoFormsAttribute(
            string label = null,
            AutoFormsType type = AutoFormsType.Auto,
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
            double paddingLeft = 25,
            double paddingRight = 25,
            double paddingTop = 0,
            double paddingBottom = 20,
            double layoutHorizontalPercentageOverride = -1,
            string[] grouped = null) : base(filter)
        {
            Label = label;
            ItemStyle = itemStyle;
            LabelStyleOverride = labelStyleOverride;
            Placeholder = placeholder;
            Type = type;
            HeightRequest = heightRequest;
            IsVisible = isVisible;
            IsEnabled = isEnabled;
            IsFocused = isFocused;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;
            PaddingTop = paddingTop;
            PaddingBottom = paddingBottom;
            LayoutHorizontalPercentageOverride = layoutHorizontalPercentageOverride;
            Grouped = grouped;

            Orientation = orientation;
            HorizontalLabelOptions = horizontalLabelOptions;
            HorizontalControlOptions = horizontalControlOptions;
            ControlWidthRequest = controlWidthRequest;
        }
    }
}
