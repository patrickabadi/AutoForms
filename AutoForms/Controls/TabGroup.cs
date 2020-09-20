using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class TabGroup : Grid
    {
        public enum LayoutOrientation
        {
            Vertical,
            Horizontal
        };

        #region Properties
        public static readonly BindableProperty SelectedStyleProperty = BindableProperty.Create(nameof(SelectedStyle), typeof(Style), typeof(TabGroup), null, BindingMode.Default, null, null);

        public Style SelectedStyle
        {
            get { return (Style)GetValue(SelectedStyleProperty); }
            set { SetValue(SelectedStyleProperty, value); }
        }

        public static readonly BindableProperty UnSelectedStyleProperty = BindableProperty.Create(nameof(UnSelectedStyle), typeof(Style), typeof(TabGroup), null, BindingMode.Default, null, null);

        public Style UnSelectedStyle
        {
            get { return (Style)GetValue(UnSelectedStyleProperty); }
            set { SetValue(UnSelectedStyleProperty, value); }
        }

        public static readonly BindableProperty ButtonHeightProperty = BindableProperty.Create(nameof(ButtonHeight), typeof(int), typeof(TabGroup), 0, BindingMode.Default,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (TabGroup)bindable;
                    ctrl.UpdateControls();
                });

        public int ButtonHeight
        {
            get { return (int)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        public static readonly BindableProperty ButtonWidthProperty = BindableProperty.Create(nameof(ButtonWidth), typeof(int), typeof(TabGroup), 0, BindingMode.Default,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (TabGroup)bindable;
                    ctrl.UpdateControls();
                });

        public int ButtonWidth
        {
            get { return (int)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(TabGroup), 0, BindingMode.Default,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (TabGroup)bindable;

                    ctrl.UpdateSelector(true, true);                   
                });

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(LayoutOrientation), typeof(TabGroup), LayoutOrientation.Horizontal, BindingMode.Default,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (TabGroup)bindable;
                    ctrl.UpdateControls();
                });

        public LayoutOrientation Orientation
        {
            get { return (LayoutOrientation) GetValue(OrientationProperty);}
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly BindableProperty SelectorColorProperty = BindableProperty.Create(nameof(SelectorColor), typeof(Color), typeof(TabGroup), Color.Blue, BindingMode.Default,
                propertyChanging: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (TabGroup)bindable;
                    if(ctrl._selector != null)
                        ctrl._selector.BackgroundColor = (Color)newValue;
                });

        public Color SelectorColor
        {
            get { return (Color)GetValue(SelectorColorProperty); }
            set { SetValue(SelectorColorProperty, value); }
        }

        public static readonly BindableProperty SelectorSizeProperty = BindableProperty.Create(nameof(SelectorSize), typeof(int), typeof(TabGroup), 4, BindingMode.Default,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ctrl = (TabGroup)bindable;
                ctrl.UpdateControls();
            });

        public int SelectorSize
        {
            get { return (int)GetValue(SelectorSizeProperty); }
            set { SetValue(SelectorSizeProperty, value); }
        }

        #endregion Properties

        private BoxView _selector;

        public TabGroup()
        {
            Orientation = LayoutOrientation.Horizontal;
            this.SizeChanged += TabGroup_SizeChanged;

            //Default the spacing when created, allowing consumers to override through bindings should they chose
            ColumnSpacing = 2;
            RowSpacing = 2;
        }

        private void TabGroup_SizeChanged(object sender, EventArgs e)
        {
            if (_selector == null)
            {
                InvokeSelector();
            }
            else
            {
                UpdateSelector(true, false);
            }
        }

        protected void UpdateControls()
        {
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();

            if (Children.Count == 0)
                return;

            GridLength gr;
            if(Orientation == LayoutOrientation.Horizontal)
            {
                gr = HorizontalOptions.Alignment == LayoutAlignment.Fill ? GridLength.Star : GridLength.Auto;

                RowDefinitions.Add(new RowDefinition { Height = ButtonHeight > 0 ? ButtonHeight : GridLength.Auto });
                RowDefinitions.Add(new RowDefinition { Height = SelectorSize });
            }
            else
            {
                gr = VerticalOptions.Alignment == LayoutAlignment.Fill ? GridLength.Star : GridLength.Auto;

                ColumnDefinitions.Add(new ColumnDefinition { Width = SelectorSize });
                ColumnDefinitions.Add(new ColumnDefinition { Width = ButtonWidth > 0 ? ButtonWidth : GridLength.Star });
            }

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i] as TabButton;
                if (child == null)
                    continue;

                child.HorizontalOptions = ButtonWidth > 0 ? LayoutOptions.Fill : HorizontalOptions.Alignment == LayoutAlignment.Fill ? LayoutOptions.Fill : LayoutOptions.CenterAndExpand;
                child.VerticalOptions = ButtonHeight > 0 ? LayoutOptions.Fill : VerticalOptions.Alignment == LayoutAlignment.Fill ? LayoutOptions.Fill : LayoutOptions.CenterAndExpand;

                if (Orientation == LayoutOrientation.Horizontal)
                {
                    ColumnDefinitions.Add(new ColumnDefinition { Width = ButtonWidth > 0 ? ButtonWidth : gr });

                    Grid.SetColumn(child, i);
                    Grid.SetRow(child, 0);
                }
                else
                {
                    RowDefinitions.Add(new RowDefinition { Height = ButtonHeight > 0 ? ButtonHeight : gr });

                    Grid.SetColumn(child, 1);
                    Grid.SetRow(child, i);
                }
                
            }

            // NOTE: we can't change selector children add/remove during collectionchanged event
            InvokeSelector();
        }


        protected void UpdateSelector(bool setTabButton = false, bool animate = true)
        {
            if (_selector == null)
                return;

            var selectedChild = Children[SelectedIndex] as TabButton;
            if (selectedChild == null)
                return;

            if(setTabButton)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    var child = Children[i] as TabButton;
                    if (child == null)
                        continue;

                    var isSelected = (i == SelectedIndex ? true : false);

                    if (child.IsSelected != isSelected)
                        child.IsSelected = isSelected;
                }
            }            

            if(animate)
            {

          
                uint aniTime = 150;
                Easing ease = Easing.Linear;

                if (Orientation == LayoutOrientation.Horizontal)
                {
                    _selector.TranslateTo(selectedChild.X, 0.0, aniTime, ease);
                }
                else
                {
                    _selector.TranslateTo(0.0, selectedChild.Y, aniTime, ease);
                }
            }
            else
            {
                if(Orientation == LayoutOrientation.Horizontal)
                {
                    _selector.TranslationX = selectedChild.X;
                }
                else
                {
                    _selector.TranslationY = selectedChild.Y;
                }
                
            }
            
        }

        protected void InvokeSelector()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (_selector != null)
                {
                    Children.Remove(_selector);
                    _selector = null;
                }

                // create the bottom selector
                if(Orientation == LayoutOrientation.Horizontal)
                {
                    _selector = new BoxView
                    {
                        BackgroundColor = SelectorColor,
                        HeightRequest = SelectorSize,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.End
                    };
                    Grid.SetRow(_selector, 1);
                    Grid.SetColumn(_selector, 0);
                    this.Children.Add(_selector);
                }
                else
                {
                    _selector = new BoxView
                    {
                        BackgroundColor = SelectorColor,
                        WidthRequest = SelectorSize,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Fill
                    };
                    Grid.SetRow(_selector, 0);
                    Grid.SetColumn(_selector, 0);
                    this.Children.Add(_selector);
                }

                UpdateSelector(true, false);

                //this.DebugGrid(Color.Red);
            });
        }

        protected void InitSelector(bool restart)
        {

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            UpdateControls();
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child is BoxView)
                return;

            if(child is Button btn)
            {
                btn.Clicked -= Btn_Clicked;
                btn.Clicked += Btn_Clicked;
            }

            UpdateControls();
        }

        protected override void OnChildRemoved(Element child, int oldLogicalIndex)
        {
            base.OnChildRemoved(child, oldLogicalIndex);

            if (child is BoxView)
                return;

            if (child is Button btn)
            {
                btn.Clicked -= Btn_Clicked;
            }

            UpdateControls();
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            int selectedIndex = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i] as TabButton;
                if (child == null)
                    continue;

                if(sender == child)
                {
                    selectedIndex = i;
                    break;
                }
            }

            if (SelectedIndex == selectedIndex)
                return;

            SelectedIndex = selectedIndex;

            UpdateSelector();
        }
    }
}
