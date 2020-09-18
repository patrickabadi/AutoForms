using AutoForms.Test.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoForms.Test
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<MenuItem> Items { get; set; }
        public MainPage()
        {
            Items = new ObservableCollection<MenuItem>
            {
                new MenuItem("Hello AutoForms", "First look at AutoForms", new HelloAutoForms()),
            };

            InitializeComponent();
            BindingContext = this;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var page = (e.SelectedItem as MenuItem)?.Page;
            if (page == null)
                return;

            await Navigation.PushAsync(new NavigationPage(page));
        }

        public class MenuItem
        {
            public string Title { get; set; }
            public string Description { get; set; }

            public ContentPage Page { get; set; }

            public MenuItem(string title, string description, ContentPage page)
            {
                Title = title;
                Description = description;
                Page = page;
            }
        }
    }


}
