using AutoForms.Test.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoForms.Test.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Validation : ContentPage
    {
        public ValidationModel Model { get; set; }
        public ICommand CheckFormCommand { get; set; }

        public Validation()
        {
            CheckFormCommand = new Command(OnCheckForm);
            Model = new ValidationModel();

            InitializeComponent();

            BindingContext = this;
        }

        protected async void OnCheckForm()
        {
            var result = Model.AutoFormsValidation?.CheckValidation() ?? false;

            string response = "Your form is " + (result ? "valid" : "invalid");
            await DisplayAlert("Form Check", response, "OK");
        }
    }
}