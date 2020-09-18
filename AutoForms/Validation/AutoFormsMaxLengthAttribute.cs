using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForms
{
    public class AutoFormsMaxLengthAttribute : AutoFormsValidationAttribute
    {
        public int Length { get; private set; }

        public AutoFormsMaxLengthAttribute(int length):base(ValidationType.MaxLength)
        {
            Length = length;
        }

        public override bool IsValid(object obj) 
        {
            var s = obj as string;
            if(s == null || s.Length <= Length)
                return true;

            return false;        
        }
    }
}
