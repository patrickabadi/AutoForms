using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForms.Test.DomainModels
{
    public class LocalizationModel
    {
        [AutoForms("IDS_DATE_Days")]
        public string Days { get; set; }

        [AutoForms("IDS_DATE_Week")]
        public string Week { get; set; }

        [AutoForms("IDS_DATE_Hours")]
        public string Hours { get; set; }

        [AutoForms("IDS_DATE_Min")]
        public string Min { get; set; }

        [AutoForms("IDS_DATE_Sec")]
        public string Sec { get; set; }
    }
}
