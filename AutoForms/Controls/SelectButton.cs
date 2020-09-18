using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class SelectButton : Button
    {

        public static readonly BindableProperty SelectedProperty = BindableProperty.Create(nameof(Selected), typeof(bool), typeof(SelectButton), false, BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var ctrl = (SelectButton)bindable;
                ctrl.UpdateButtonText();
            });

        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        /// <summary>
        /// Text that is displayed when button isn't selected
        /// </summary>
        public static readonly BindableProperty UnselectedTextProperty = BindableProperty.Create(nameof(UnselectedText), typeof(string), typeof(SelectButton), string.Empty, BindingMode.Default,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var ctrl = (SelectButton)bindable;
                ctrl.UpdateButtonText();
            });

        public string UnselectedText
        {
            get { return (string)GetValue(UnselectedTextProperty); }
            set { SetValue(UnselectedTextProperty, value); }
        }

        /// <summary>
        /// Text that is displayed when button is selected
        /// </summary>
        public static readonly BindableProperty SelectedTextProperty = BindableProperty.Create(nameof(SelectedText), typeof(string), typeof(SelectButton), string.Empty, BindingMode.Default,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var ctrl = (SelectButton)bindable;
                ctrl.UpdateButtonText();
            });
        public string SelectedText
        {
            get { return (string)GetValue(SelectedTextProperty); }
            set { SetValue(SelectedTextProperty, value); }
        }

        public SelectButton()
        {
            Clicked += SelectButton_Clicked;
        }

        public void SelectButton_Clicked(object sender, EventArgs e)
        {
            if (this.Parent == null || Selected == true)
                return;

            this.Selected = true;

            int selectedIndex = 0;
            int indexCounter = 0;

            if (this.Parent is Grid grid)
            {
                selectedIndex = 0;
                indexCounter = 0;
                foreach (var ele in grid.Children)
                {
                    if (ele == this)
                    {
                        selectedIndex = indexCounter;
                        continue;
                    }

                    if (ele is SelectButton btn)
                    {
                        btn.Selected = false;
                        indexCounter++;
                    }
                }
            }
            else if (this.Parent is StackLayout stack)
            {
                selectedIndex = 0;
                indexCounter = 0;
                foreach (var ele in stack.Children)
                {
                    if (ele == this)
                    {
                        selectedIndex = indexCounter;
                        continue;
                    }

                    if (ele is SelectButton btn)
                    {
                        btn.Selected = false;
                        indexCounter++;
                    }
                }
            }

            if (this.Parent is SelectButtonGroup group)
            {
                group.SelectedIndex = selectedIndex;
            }
            else if (this.Parent?.Parent is SelectButtonGroup grp)
            {
                grp.SelectedIndex = selectedIndex;
            }
            else if (this.Parent is SelectButtonGroupDynamic groupd)
            {
                groupd.SelectedIndex = selectedIndex;
            }
            else if (this.Parent?.Parent?.Parent is SelectButtonGroupDynamic grpd)
            {
                grpd.SelectedIndex = selectedIndex;
            }

        }

        protected void UpdateButtonText()
        {
            if (Selected && !string.IsNullOrEmpty(SelectedText))
            {
                Text = SelectedText;
            }
            else if (!Selected && !string.IsNullOrEmpty(UnselectedText))
            {
                Text = UnselectedText;
            }
        }
    }
}
