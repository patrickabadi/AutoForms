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
    public partial class CustomControls : ContentPage
    {
        public CustomControls()
        {
            InitializeComponent();
            BindingContext = new CustomModel();
        }
    }
}