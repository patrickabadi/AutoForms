using AutoForms.Common;
using AutoForms.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ListViewItemCell : ViewCell
    {
        Grid _grid => View as Grid;

        public ListViewItemCell()
        {
            View = new ListViewItem();
        }
    }

    public class ListViewItem : ContentView
    {
        public ControlValidation ControlValidation;
        Grid _grid => Content as Grid;

        public ListViewItem()
        {
            ControlValidation = null;

            Content = new Grid
            {
                ColumnSpacing = 0,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
            };           
        }

        //pulled this out into it's own method to access the actual ListView properties
        protected ControlList GetParentListView()
        {
            // NOTE: Look away nothing to see here!
            var listControl = this.Parent?.Parent?.Parent as ControlList;

            if (listControl == null)
            {
                // if we're using the real ListView control this will work
                listControl = this.Parent?.Parent?.Parent?.Parent as ControlList;
            }

            // can be null
            return listControl;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null && _grid.Children.Count == 0)
            {
                RecreatControl();
            }

            var listControl = GetParentListView();
            if (listControl == null)
                return;

            var domainModelType = listControl.GetListItemType();

            var attrib = domainModelType.GetAttribute<AutoFormsListUIAttribute>();
            if (attrib == null || attrib.AlternateListItemStyle == null)
            { 
                return;
            }
            Style style = null;

            if (Application.Current.Resources.TryGetValue(attrib.AlternateListItemStyle, out object obj))
            {
                style = (Style)obj;
            }
            else
                return;

            ICollection list = listControl.ItemsSource as ICollection;
            if (list == null)
                list = listControl.ListView?.ItemsSource as ICollection;


            if (list != null)
            {
                int index = 0;
                foreach (var item in list)
                {
                    if (item == BindingContext && index % 2 != 0)
                    { 
                        Content.Style = style;
                    }
                    index++;
                }
            }
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (this.Parent == null || _grid.Children.Count > 0)
                return;

            RecreatControl();
        }

        protected void RecreatControl()
        {
            _grid.Children.Clear();
            _grid.RowDefinitions.Clear();
            _grid.ColumnDefinitions.Clear();
            ControlValidation = null;

            var listControl = GetParentListView();
            if (listControl == null)
            {
                return;
            }
                        
            var domainModelType = listControl.GetListItemType();

            var attrib = domainModelType.GetAttribute<AutoFormsListUIAttribute>();
            if (attrib == null)
            { 
                return;
            }

            _grid.ColumnSpacing = attrib.ColumnSpacing;
            _grid.Padding = new Thickness(attrib.ListItemPaddingLeft, attrib.ListItemPaddingTop, attrib.ListItemPaddingRight, attrib.ListItemPaddingBottom);

            var props = AttributeHelper.GetPropertyAttributes<AutoFormsListItemAttribute>(domainModelType);

            ControlValidation controlValidation = new ControlValidation(()=> { _grid.ForceLayout(); });

            foreach (var p in props)
            {
                var property = p.Item1;
                var attribute = ControlBase.GetFilteredAttribute(listControl.Filter, p.Item2);

                if (property == null || attribute == null)
                    continue;

                _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(attribute.Value, attribute.GridType) });

                var v = CreateControlView(property, attribute, listControl);
                Grid.SetColumn(v, _grid.ColumnDefinitions.Count - 1);
                _grid.Children.Add(v);

                var validations = property.GetAttributes<AutoFormsValidationAttribute>();
                if(validations != null && validations.Length > 0)
                {
                    foreach(var validation in validations)
                    {
                        controlValidation.AddValidation(validation, v, string.IsNullOrWhiteSpace(attribute.Label) ? attribute.Placeholder : attribute.Label);
                    }
                }
            }

            CreateActionButtons(attrib, listControl);

            if(controlValidation.Size > 0)
            {
                _grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Grid.SetRow(controlValidation, 1);
                Grid.SetColumnSpan(controlValidation, _grid.ColumnDefinitions.Count);
                _grid.Children.Add(controlValidation);

                ControlValidation = controlValidation;
            }

            //_grid.DebugGrid(Color.Red);
        }

        void CreateActionButtons(AutoFormsListUIAttribute attribute, ControlList listControl)
        { 
            if (listControl == null || attribute == null || listControl.HasActions == false)
                return;

            Style buttonStyle = null;
            if (Application.Current.Resources.TryGetValue(attribute.ActionButtonStyle, out object obj))
            {
                buttonStyle = (Style)obj;
            }

            // action buttons
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(ControlList.ActionButtonWidth, GridUnitType.Absolute) });

            var stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Orientation = StackOrientation.Horizontal,
                Spacing = 15,
            };
            Grid.SetColumn(stack, _grid.ColumnDefinitions.Count - 1);
            _grid.Children.Add(stack);

            if (string.IsNullOrEmpty(listControl.EditCommand) == false)
            {
                var attrib = FindButtonAttribute(listControl.EditCommand, listControl);

                var style = buttonStyle;
                var text = "\uE70F";

                if(attrib != null)
                {
                    if (Application.Current.Resources.TryGetValue(attrib.ButtonStyle, out object obj1))
                        style = (Style)obj1;
                    text = attrib.Text;
                }

                var b = new Button
                {
                    Style = style,
                    Text = text,
                };

                b.SetBinding(ImageButton.CommandParameterProperty, new Binding("."));
                b.SetBinding(ImageButton.CommandProperty, new Binding("BindingContext." + listControl.EditCommand, source: listControl));
                stack.Children.Add(b);
            }
            if (string.IsNullOrEmpty(listControl.ViewCommand) == false)
            {
                var attrib = FindButtonAttribute(listControl.ViewCommand, listControl);

                var style = buttonStyle;
                var text = "\uE890";

                if (attrib != null)
                {
                    if (Application.Current.Resources.TryGetValue(attrib.ButtonStyle, out object obj2))
                        style = (Style)obj2;
                    text = attrib.Text;
                }

                var b = new Button
                {
                    Style = style,
                    Text = text,
                };
                b.SetBinding(ImageButton.CommandParameterProperty, new Binding("."));
                b.SetBinding(ImageButton.CommandProperty, new Binding("BindingContext." + listControl.ViewCommand, source: listControl));
                stack.Children.Add(b);
            }
            if (string.IsNullOrEmpty(listControl.DeleteCommand) == false)
            {
                var attrib = FindButtonAttribute(listControl.DeleteCommand, listControl);

                var style = buttonStyle;
                var text = "\uE74D";

                if (attrib != null)
                {
                    if (Application.Current.Resources.TryGetValue(attrib.ButtonStyle, out object obj3))
                        style = (Style)obj3;
                    text = attrib.Text;
                }

                var b = new Button
                {
                    Style = style,
                    Text = text,
                };
                b.SetBinding(ImageButton.CommandParameterProperty, new Binding("."));
                b.SetBinding(ImageButton.CommandProperty, new Binding("BindingContext." + listControl.DeleteCommand, source: listControl));
                stack.Children.Add(b);
            }


        }

        AutoFormsButtonAttribute FindButtonAttribute(string commandName, ControlList listControl)
        {
            var domainModelType = listControl.BindingContext.GetType();

            var props = AttributeHelper.GetPropertyAttributes<AutoFormsButtonAttribute>(domainModelType);

            foreach (var p in props)
            {
                if(p.Item1.Name != commandName || p.Item2 == null)
                {
                    continue;
                }

                return p.Item2[0];
            }

            return null;
        }

        View CreateControlView(
            PropertyInfo property,
            AutoFormsListItemAttribute attribute,
            ControlList listControl)
        {
            Style style = null;

            if (string.IsNullOrEmpty(attribute.ItemStyle) == false &&
                        Application.Current.Resources.TryGetValue(attribute.ItemStyle, out object obj))
            {
                style = (Style)obj;
            }

            View v = null;
            switch (attribute.Type)
            {
                case AutoFormsType.DateTime:
                    v = new DatePicker
                    {
                        Style = style
                    };
                    v.SetBinding(DatePicker.DateProperty, new Binding(property.Name, BindingMode.TwoWay, new DateTimeConverter(), property.PropertyType));
                    break;
                case AutoFormsType.Entry:

                    var maxLength = property.GetAttribute<AutoFormsMaxLengthAttribute>()?.Length ?? 0;

                    InputView iv;
                    if (attribute.HeightRequest > 0)
                    {
                        iv = new Editor
                        {
                            Style = style,
                            Placeholder = attribute.Placeholder,
                            HeightRequest = attribute.HeightRequest,
                        };
                        iv.SetBinding(Editor.TextProperty, new Binding(property.Name, BindingMode.TwoWay));
                    }
                    else
                    {
                        iv = new Entry
                        {
                            Style = style,
                            Placeholder = attribute.Placeholder,
                        };
                        iv.SetBinding(Entry.TextProperty, new Binding(property.Name, BindingMode.TwoWay));
                    }

                    if (maxLength > 0)
                        iv.MaxLength = maxLength;

                    v = iv;
                    break;
                case AutoFormsType.Checkbox:
                    //v = new Checkbox
                    //{
                    //    HorizontalOptions = LayoutOptions.Center,
                    //};
                    //v.SetBinding(Checkbox.CheckedProperty, new Binding(property.Name, BindingMode.TwoWay));
                    break;
                case AutoFormsType.Button:
                    var btnAttrib = property.GetAttribute<AutoFormsButtonAttribute>();

                    if (btnAttrib != null &&
                        string.IsNullOrEmpty(btnAttrib.ButtonStyle) == false &&
                        Application.Current.Resources.TryGetValue(btnAttrib.ButtonStyle, out object btnObj))
                    {
                        style = (Style)btnObj;
                    }

                    v = new Button
                    {
                        Style = style,
                        Text = btnAttrib?.Text,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                    };
                    v.SetBinding(Button.CommandParameterProperty, new Binding("."));
                    v.SetBinding(Button.CommandProperty, new Binding(property.Name));
                    break;
                case AutoFormsType.Combo:
                    v = new Picker();
                    var propertyType = property.PropertyType;
                    var t = Nullable.GetUnderlyingType(propertyType);

                    if (t != null && t.IsEnum)
                    {
                        propertyType = t;

                        var dict = EnumHelper.ToDictionary(propertyType);

                        var items = new List<ControlCombo.EnumItem>();
                        items.Add(new ControlCombo.EnumItem { Title = "Please Select", Value = -1 });

                        int index = 0;

                        foreach (var d in dict)
                        {
                            items.Add(new ControlCombo.EnumItem { Title = d.Value, Value = index++ });
                        }

                        var picker = v as Picker;
                        picker.ItemsSource = items;

                        picker.SetBinding(
                            Picker.SelectedIndexProperty,
                            new Binding(property.Name, BindingMode.TwoWay, new ControlCombo.EnumItemConverter(), propertyType));
                    } 
                    else if(property.PropertyType.IsEnum)
                    {
                        var picker = v as Picker;
                        var dict = EnumHelper.ToDictionary(property.PropertyType);
                        picker.ItemsSource = dict.Values.ToList();

                        picker.SetBinding(
                        Picker.SelectedIndexProperty,
                        new Binding(property.Name, BindingMode.TwoWay, new EnumConverter(), property.PropertyType));
                    }
                    break;
                default:
                    style = style ?? listControl?.LabelStyle ?? null;

                    v = new Label
                    {
                        Style = style,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.Fill,
                        HorizontalTextAlignment = attribute.HorizontalItemAlignment,
                        LineBreakMode = LineBreakMode.TailTruncation,
                        MaxLines = 1,
                    };
                    v.SetBinding(Label.TextProperty, new Binding(property.Name, converter: new DisplayConverter()));
                    break;
            }

            return v;

        }
    }
}
