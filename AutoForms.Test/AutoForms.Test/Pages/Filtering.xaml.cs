using AutoForms.Test.DomainModels;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoForms.Test.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [AddINotifyPropertyChangedInterface]
    public partial class Filtering : ContentPage
    {       
        public FilteringModel Model { get; set; }

        public int SelectedTabIndex { get; set; }

        public Filtering()
        {
            Model = new FilteringModel
            {
                Filter = FilteringModel.FilterType.Login.ToString()
            };

            InitializeComponent();
            BindingContext = this;
        }

        private void OnSelectedTabIndexChanged()
        {
            Model.Filter = SelectedTabIndex == 0 ? FilteringModel.FilterType.Login.ToString() : FilteringModel.FilterType.Create.ToString();
        }
    }
}