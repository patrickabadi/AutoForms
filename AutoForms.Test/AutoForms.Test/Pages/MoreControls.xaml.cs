using AutoForms.Test.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoForms.Test.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoreControls : ContentPage
    {
        public MoreControls()
        {
            InitializeComponent();
            var model = new MoreModel
            {
                MyButton = new Command(OnMyButton)
            };

            BindingContext = model;
        }

        private async void OnMyButton()
        {
            await DisplayAlert("", "Clicked", "OK");
        }
    }
}