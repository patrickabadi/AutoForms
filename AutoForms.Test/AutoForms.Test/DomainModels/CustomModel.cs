using AutoForms.Test.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForms.Test.DomainModels
{
    public class CustomModel
    {
        [AutoFormsCustom("AutoForms.Test.Controls.DateAndTime", "This is a custom Control")]
        public DateTime Timer { get; set; }
    }
}
