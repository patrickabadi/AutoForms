using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class RadioGroup : Grid
    {
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(RadioGroup), -1, BindingMode.Default,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (RadioGroup)bindable;

                    if(newValue == null)
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
            foreach(var ele in Children)
            {
                if (!(ele is RadioButton btn))
                    continue;

                if(selectedIndex == currentIndex)
                {
                    if (btn.Selected)
                        break;

                    btn.Selected = true;
                    break;
                }

                currentIndex++;
            }
        }
    }
}
