using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Test.DomainModels
{
    public class HorizontalModel
    {
        public enum PhoneType
        {
            Mobile,
            Home,
            Business,
        }

        public enum ContactRelation
        {
            [Description("Self")]
            Self,
            [Description("Parent")]
            Parent,
            [Description("Spouse")]
            Spouse,
            [Description("Child")]
            Child,
        }

        [AutoFormsHorizontalGroup(1, GridUnitType.Star)]
        [AutoForms("Phone Number", AutoFormsType.Entry, placeholder: "CC", grouped: new string[] { nameof(PhoneNumber), nameof(Extension) })]
        public string CountryCode { get; set; }

        [AutoFormsHorizontalGroup(3, GridUnitType.Star)]
        [AutoForms(" ", AutoFormsType.Entry, placeholder: "Number")]
        public string PhoneNumber { get; set; }

        [AutoFormsHorizontalGroup(1, GridUnitType.Star)]
        [AutoForms(" ", AutoFormsType.Entry, placeholder: "Ext")]
        public string Extension { get; set; }

        [AutoFormsHorizontalGroup(3, GridUnitType.Star)]
        [AutoForms("Phone Number", AutoFormsType.Entry, grouped: new string[] { nameof(Phone) })]
        public string PhoneNumber2 { get; set; }

        [AutoFormsHorizontalGroup(1, GridUnitType.Star)]
        [AutoForms(" ")]
        public PhoneType Phone { get; set; }

        [AutoForms("Relation to client", AutoFormsType.SelectButton, itemStyle: "DefaultSelectButtonStyle")]
        public ContactRelation? Relation { get; set; }
    }
}
