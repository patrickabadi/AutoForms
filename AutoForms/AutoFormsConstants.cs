using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;



namespace AutoForms
{
    public static class AutoFormsConstants
    {
        // Icon Fonts
        public static string FontFamilyDefault = "MaterialIcons";

        // Icons related to Icon Fonts
        public static string Checkbox_Checked = "\xe834";
        public static string Checkbox_Empty = "\xe835";
        public static string Radio_Empty = "\xe836";
        public static string Radio_Checked = "\xe837";
        public static string Button_Edit = "\xe150";
        public static string Button_View = "\xe417";
        public static string Button_Delete = "\xe872";

        // Colors
        public static Color SelectedColor = Color.FromRgb(20, 143, 206);

        // UI styles
        public static string ActionButtonStyle = "AutoFormsActionButtonStyle";
        public static string ListHeaderStyle = "AutoFormsListHeaderStyle";
        public static string AutoFormsListHeaderLabelStyle = "AutoFormsListHeaderLabelStyle";

        // String resource location
        public static string ApplicationPath = "";
        public static string StringResourcePath = "";
        public static CultureInfo CultureOverride = null;

        // String sentences
        public static string ValidationMaxLength = "\"{0}\" is over maximum length of {1} ";
        public static string ValidationMinLength = "\"{0}\" must have at least {1} characters";
        public static string ValidationNumeric = "\"{0}\" can only contain numeric values";
        public static string ValidationEmail = "\"{0}\" must be a valid email address";
        public static string ValidationRequired = "\"{0}\" is required";
        public static string ComboSelect = "Please Select";
    }
}
