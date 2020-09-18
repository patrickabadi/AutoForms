using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForms
{
    public class AutoFormsNumericAttribute : AutoFormsValidationAttribute
    {
        public AutoFormsNumericAttribute():base(ValidationType.Numeric) 
        {
        }

        public override bool IsValid(object obj)
        {
            if (obj == null)
                return true;

            var s = obj as string;
            if (s != null && string.IsNullOrEmpty(s))
                return true;

            if (long.TryParse(s, out long l))
                return true;

            if (decimal.TryParse(s, out decimal d))
                return true;

            return false;
        }
    }

}
