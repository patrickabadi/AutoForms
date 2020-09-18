using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoForms
{
    public class AutoFormsHorizontalGroupAttribute : AutoFormsFilteredAttribute
    {
        public double Value { get; private set; }

        public GridUnitType GridType { get; private set; }

        public AutoFormsHorizontalGroupAttribute(
            double value = 1,
            GridUnitType gridType = GridUnitType.Star,
            int filter = 0):base(filter)
        {
            Value = value;
            GridType = gridType;
        }
    }
}
