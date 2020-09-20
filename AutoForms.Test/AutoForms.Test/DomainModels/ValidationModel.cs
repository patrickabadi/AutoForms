using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Test.DomainModels
{
    public class ValidationModel
    {
        public AutoFormsValidation AutoFormsValidation { get; set; }

        [AutoForms("Name")]
        [AutoFormsRequired]
        [AutoFormsMaxLength(10)]
        public string Name { get; set; }

        [AutoForms("Email")]
        [AutoFormsRequired]
        [AutoFormsMaxLength(10)]
        [AutoFormsEmail]
        public string Email { get; set; }

        [AutoFormsHorizontalGroup(1, GridUnitType.Star)]
        [AutoForms(" ", AutoFormsType.Entry, placeholder: "CC", grouped: new string[] { nameof(PhoneNumber), nameof(Extension) })]
        [AutoFormsMaxLength(5)]
        [AutoFormsRequired]
        public string CountryCode { get; set; }

        [AutoFormsHorizontalGroup(3, GridUnitType.Star)]
        [AutoForms(" ", AutoFormsType.Entry, placeholder:"Number")]
        [AutoFormsMaxLength(10)]
        [AutoFormsMinLength(5)]
        [AutoFormsRequired]
        public string PhoneNumber { get; set; }

        [AutoFormsHorizontalGroup(1, GridUnitType.Star)]
        [AutoForms(" ", AutoFormsType.Entry, placeholder:"Ext")]
        [AutoFormsMaxLength(4)]        
        [AutoFormsRequired]
        public string Extension { get; set; }
    }
}
