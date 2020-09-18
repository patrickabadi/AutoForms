using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForms
{
    public class AutoFormsFilteredAttribute : Attribute
    {
        public int Filter { get; private set; }

        public AutoFormsFilteredAttribute(int filter = 0)
        {
            Filter = filter;
        }
    }
}
