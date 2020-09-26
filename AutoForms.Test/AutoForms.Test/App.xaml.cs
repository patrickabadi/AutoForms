using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("MaterialIcons-Regular.ttf", Alias = "MaterialIcons")]

namespace AutoForms.Test
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Icon Fonts
            AutoFormsConstants.FontFamilyDefault = "MaterialIcons";

            // Icons related to Icon Fonts
            AutoFormsConstants.Checkbox_Checked = "\xe834";
            AutoFormsConstants.Checkbox_Empty = "\xe835";
            AutoFormsConstants.Radio_Empty = "\xe836";
            AutoFormsConstants.Radio_Checked = "\xe837";
            AutoFormsConstants.Button_Edit = "\xe150";
            AutoFormsConstants.Button_View = "\xe417";
            AutoFormsConstants.Button_Delete = "\xe872";

            // Colors
            AutoFormsConstants.SelectedColor = Color.FromRgb(20, 143, 206);

            // UI styles
            AutoFormsConstants.ActionButtonStyle = "AutoFormsActionButtonStyle";
            AutoFormsConstants.ListHeaderStyle = "AutoFormsListHeaderStyle";
            AutoFormsConstants.AutoFormsListHeaderLabelStyle = "AutoFormsListHeaderLabelStyle";

            // Resource string
            AutoFormsConstants.ApplicationPath = "AutoForms.Test";
            AutoFormsConstants.StringResourcePath = "AutoForms.Test.Resources.Resource";

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
