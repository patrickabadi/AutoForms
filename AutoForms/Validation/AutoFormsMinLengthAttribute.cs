using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForms
{
    public class AutoFormsMinLengthAttribute : AutoFormsValidationAttribute
    {
        public int Length { get; private set; }

        public AutoFormsMinLengthAttribute(int length):base(ValidationType.MinLength)
        {
            Length = length;
        }

        public override bool IsValid(object obj)
        {
            var s = obj as string;
            if (s == null || s.Length >= Length)
                return true;

            return false;
        }
    }
}
