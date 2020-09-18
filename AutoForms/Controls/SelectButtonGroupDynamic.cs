using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class SelectButtonGroupDynamic : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(SelectButtonGroupDynamic), propertyChanged: OnItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(SelectButtonGroupDynamic));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(SelectButtonGroupDynamic), -1, BindingMode.Default,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (SelectButtonGroupDynamic)bindable;

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

        public StackOrientation Orientation { get; set; } = StackOrientation.Horizontal;

        public Style SelectButtonStyle { get; set; } = null;

        protected StackLayout StackLayout { get; set; }

        public SelectButtonGroupDynamic()
        {
            Content = new ScrollView
            {
                Content = StackLayout = new StackLayout
                {
                    Margin = new Thickness(2),
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Orientation = Orientation,
                }
            };
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldVal, object newVal)
        {
            IEnumerable newValue = newVal as IEnumerable;
            var control = (SelectButtonGroupDynamic)bindable;

            var existingCollection = oldVal as INotifyCollectionChanged;
            if (existingCollection != null)
            {
                existingCollection.CollectionChanged -= control.OnItemsSourceCollectionChanged;
            }

            var observableCollection = newValue as INotifyCollectionChanged;
            if (observableCollection != null)
            {
                observableCollection.CollectionChanged += control.OnItemsSourceCollectionChanged;
            }

            control.StackLayout.Children.Clear();
            if (newValue != null)
            {
                foreach (var item in newValue)
                {
                    control.StackLayout.Children.Add(control.CreateChildView(item));
                }
            }
        }

        void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                StackLayout.Children.Clear();
            }

            if (e.OldItems != null && Children.Count > e.OldStartingIndex)
            {
                StackLayout.Children.RemoveAt(e.OldStartingIndex);
            }

            if (e.NewItems != null)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    var item = e.NewItems[i];
                    var view = CreateChildView(item);
                    StackLayout.Children.Insert(e.NewStartingIndex + i, view);
                }
            }
        }

        View CreateChildView(object binding)
        {
            if (ItemTemplate is DataTemplateSelector)
            {
                var dts = ItemTemplate as DataTemplateSelector;
                var itemTemplate = dts.SelectTemplate(binding, null);
                itemTemplate.SetValue(BindableObject.BindingContextProperty, binding);
                return (View)itemTemplate.CreateContent();
            }
            else
            {
                ItemTemplate.SetValue(BindableObject.BindingContextProperty, binding);
                return (View)ItemTemplate.CreateContent();
            }
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
