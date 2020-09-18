using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class SelectButtonGroup : Grid
    {
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(SelectButtonGroup), -1, BindingMode.Default,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (SelectButtonGroup)bindable;

                    if (newValue == null)
                    {
                        ctrl.UpdateSelector(-1);
                    }
                    else
                    {
                        ctrl.UpdateSelector((int)newValue);
                    }

                });

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private void UpdateSelector(int selectedIndex)
        {
            if (Children == null || Children.Count <= selectedIndex)
                return;

            int currentIndex = 0;
            foreach (var ele in Children)
            {
                if (!(ele is SelectButton btn))
                    continue;

                btn.Selected = selectedIndex == currentIndex;

                currentIndex++;
            }
        }
    }
}
