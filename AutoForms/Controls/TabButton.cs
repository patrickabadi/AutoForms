using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class TabButton:Button
    {
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TabButton), false, BindingMode.Default);

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public TabButton()
        {
            Clicked += TabButton_Clicked;
        }


        public void TabButton_Clicked(object sender, EventArgs e)
        {
            if (this.Parent == null || IsSelected == true)
                return;

            this.IsSelected = true;

            if(this.Parent is Grid grid)
            {
                foreach(var ele in grid.Children)
                {
                    if (ele == this)
                        continue;

                    if(ele is TabButton tab)
                    {
                        tab.IsSelected = false;
                    }
                }
            }
            else if(this.Parent is StackLayout stack)
            {
                foreach (var ele in stack.Children)
                {
                    if (ele == this)
                        continue;

                    if (ele is TabButton tab)
                    {
                        tab.IsSelected = false;
                    }
                }
            }

        }
    }
}
