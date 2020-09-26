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
                new MenuItem("Hello AutoForms", "First look at AutoForms", typeof(HelloAutoForms)),
                new MenuItem("Horizontal Controls", "Controls grouped horizontally", typeof(HorizontalControls)),
                new MenuItem("More Controls", "List of different types of controls", typeof(MoreControls)),
                new MenuItem("Lists", "Dynamically add/remove items in a list", typeof(Lists)),
                new MenuItem("Validation", "Forms Validation", typeof(Validation)),
                new MenuItem("Filtering", "Dynamic filtering the same model", typeof(Filtering)),
                new MenuItem("Custom", "Create your own controls and add to AutoForms", typeof(CustomControls)),
            };

            InitializeComponent();
            BindingContext = this;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var t = (e.SelectedItem as MenuItem)?.PageType;
            if (t == null)
                return;                

            var page = Activator.CreateInstance(t) as ContentPage;
            if (page == null)
                return;

            await Navigation.PushAsync(page);

            ((ListView)sender).SelectedItem = null;
        }

        public class MenuItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public Type PageType { get; set; }

            public MenuItem(string title, string description, Type pageType)
            {
                Title = title;
                Description = description;
                PageType = pageType;
            }
        }
    }


}
