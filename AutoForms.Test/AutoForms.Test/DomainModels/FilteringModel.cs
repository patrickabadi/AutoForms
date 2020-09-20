using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AutoForms.Test.DomainModels
{
    [AddINotifyPropertyChangedInterface]
    public class FilteringModel
    {
        public enum FilterType
        {
            Login = 1 << 0,
            Create = 1 << 1,
        }

        public string FilteringType => typeof(FilterType).FullName;
        public string Filter { get; set; }

        [AutoForms("Login", 
            horizontalLabelOptions: AutoFormsLayoutOptions.Center, 
            paddingTop: 40,
            filter: (int)(FilterType.Login))]
        public object LabelLogin { get; set; }

        [AutoForms("Create", 
            horizontalLabelOptions: AutoFormsLayoutOptions.Center,
            paddingTop: 40,
            filter: (int)(FilterType.Create))]
        public object LabelCreate { get; set; }

        [AutoFormsHorizontalGroup(1, GridUnitType.Star, filter: (int)(FilterType.Create))]
        [AutoForms("Name", placeholder: "First", filter: (int)(FilterType.Create), grouped: new string[] { nameof(LastName) })]
        public string FirstName { get; set; }

        [AutoFormsHorizontalGroup(1, GridUnitType.Star, filter: (int)(FilterType.Create))]
        [AutoForms(" ", placeholder: "Last", filter: (int)(FilterType.Create))]
        public string LastName { get; set; }

        [AutoForms(" ", placeholder:"Email", filter:(int)(FilterType.Create | FilterType.Login))]
        public string Email { get; set; }

        [AutoForms(" ", placeholder: "Password", filter: (int)(FilterType.Create | FilterType.Login))]
        public string Password { get; set; }

        [AutoForms("Login", itemStyle: "DefaultButtonStyle", filter: (int)(FilterType.Login))]
        public ICommand LoginCommand { get; set; }

        [AutoForms("Create", itemStyle: "DefaultButtonStyle", filter: (int)(FilterType.Create))]
        public ICommand CreateCommand { get; set; }

    }
}
