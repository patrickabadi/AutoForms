using AutoForms.Common;
using AutoForms.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ControlConfig
    {
        public string Label => Attribute.Label;
        public Style LabelStyle { get; private set; }
        public Style EditorStyle { get; private set; }
        public Color SeparatorColor { get; private set; }
        public AutoFormsAttribute Attribute { get; private set; }
        public PropertyInfo Property { get; private set; }
        public string BindingPath => Property.Name;
        public Type FieldType => Property?.PropertyType;
        public double LayoutHorizontalPercentage { get; private set; }
        public int Filter { get; private set; }

        public ControlConfig(
            double layoutHorizontalPercentage,
            AutoFormsAttribute attribute,
            Style labelStyle,
            Style editorStyle,
            Color separatorColor,
            PropertyInfo property,
            int filter)
        {
            LayoutHorizontalPercentage = layoutHorizontalPercentage;
            Attribute = attribute;
            LabelStyle = labelStyle;
            EditorStyle = editorStyle;
            SeparatorColor = separatorColor;
            Property = property;
            Filter = filter;
        }
    }

    public abstract class ControlBase:ContentView
    {
        #region Properties
        public static readonly BindableProperty IsControlVisibleProperty = BindableProperty.Create(nameof(IsControlVisible), typeof(bool), typeof(ControlBase), true, BindingMode.Default,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var ctrl = (ControlBase)bindable;

                if (newValue == null || ctrl == null)
                    return;

                var t = Nullable.GetUnderlyingType(newValue.GetType());
                if(t != null && newValue is bool?)
                {
                    ctrl.RecurseVisibility(ctrl.Content, (bool)newValue);
                }
                else if(newValue is bool b)
                {
                    ctrl.RecurseVisibility(ctrl.Content, b);
                }

                //change spacing depending on if the hidden property is visible
                if ((bool)newValue && !(ctrl is ControlHorizontalGroup))
                {
                    if (ctrl.Content is Grid g)
                    {
                        g.Padding = new Thickness(ctrl._attribute.PaddingLeft, ctrl._attribute.PaddingTop, ctrl._attribute.PaddingRight, ctrl._attribute.PaddingBottom);
                    }
                    else
                    {
                        ctrl.Content.Margin = new Thickness(ctrl._attribute.PaddingLeft, ctrl._attribute.PaddingTop, ctrl._attribute.PaddingRight, ctrl._attribute.PaddingBottom);
                    }
                }

                else
                {
                    if (ctrl.Content is Grid g)
                    {
                        g.Padding = new Thickness(0);
                    }
                    else
                    {
                        ctrl.Content.Margin = new Thickness(0);
                    }
                }

            });

        public bool IsControlVisible
        {
            get { return (bool)GetValue(IsControlVisibleProperty); }
            set { SetValue(IsControlVisibleProperty, value); }
        }

        public static readonly BindableProperty IsControlEnabledProperty = BindableProperty.Create(nameof(IsControlEnabled), typeof(bool), typeof(ControlBase), true, BindingMode.Default,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var ctrl = (ControlBase)bindable;

                if (newValue == null || ctrl == null)
                    return;

                var t = Nullable.GetUnderlyingType(newValue.GetType());
                if (t != null && newValue is bool?)
                {
                    ctrl.RecurseEnabled(ctrl.Content, (bool)newValue);
                }
                else if (newValue is bool b)
                {
                    ctrl.RecurseEnabled(ctrl.Content, b);
                }

            });

        public bool IsControlEnabled
        {
            get { return (bool)GetValue(IsControlEnabledProperty); }
            set { SetValue(IsControlEnabledProperty, value); }
        }
        #endregion

        public List<ControlBase> Groups;
        public string Label => _config.Label;
        public AutoFormsAttribute Attribute => _config.Attribute;
        public string BindingName => _config.BindingPath;
        public Style LabelStyle => _config.LabelStyle;
        public int Filter => _config.Filter;

        public Grid LayoutRoot => Content as Grid;

        public View Control => _control;

        protected AutoFormsAttribute _attribute => _config.Attribute;
        protected PropertyInfo _property => _config.Property;
        protected Type _propertyType => _config.Property?.PropertyType;
        protected ControlConfig _config { get; set; }
        protected View _control { get; set; }
        protected View _labelControl;

        protected const int _gridSpacing = 2;

        protected Style _itemStyle
        {
            get
            {
                return GetStyle(_config.Attribute.ItemStyle);
            }
        }
        protected Style _labelStyleOverride
        {
            get
            {
                return GetStyle(_config.Attribute.LabelStyleOverride);
            }
        }

        public ControlBase(ControlConfig config)
        {
            Groups = new List<ControlBase>();
            _config = config;
        }

        public static ControlBase CreateControl(ControlConfig config)
        {
            var attribute = config.Attribute;
            var property = config.Property;

            ControlBase item = null;

            switch (attribute.Type)
            {
                case AutoFormsType.Group:
                    item = new ControlGroup(config);
                    break;
                case AutoFormsType.Custom:
                    var attribCustom = property.GetAttribute<AutoFormsCustomAttribute>();
                    if (attribCustom != null && string.IsNullOrEmpty(attribCustom.CustomControlType) == false)
                    {
                        item = CreateCustomControl(attribCustom.CustomControlType);

                        if (item != null && item is ControlCustom customControl)
                        {
                            customControl.InitializeCustom(config);
                        }
                    }
                    break;
                case AutoFormsType.ActionList:
                    var attribList = property.GetAttribute<AutoFormsListAttribute>();
                    if (attribList != null)
                    {
                        item = new ControlList(config);
                    }
                    break;
                case AutoFormsType.Entry:
                    item = new ControlEditor(config);
                    break;
                case AutoFormsType.Combo:
                    item = new ControlCombo(config);
                    break;
                case AutoFormsType.Checkbox:
                    item = new ControlCheckbox(config);
                    break;
                case AutoFormsType.Radio:
                    item = new ControlRadio(false, config);
                    break;
                case AutoFormsType.DateTime:
                    item = new ControlDateTime(config);
                    break;
                case AutoFormsType.Button:
                    item = new ControlButton(config);
                    break;
                case AutoFormsType.Label:
                    item = new ControlLabel(config);
                    break;
                case AutoFormsType.SelectButton:
                    item = new ControlSelectButton(config);
                    break;
                case AutoFormsType.Auto:

                    var p = property.PropertyType;

                    var t = Nullable.GetUnderlyingType(p);
                    if (t != null)
                        p = t;

                    switch (p)
                    {
                        case Type _ when p == typeof(int):
                        case Type _ when p == typeof(float):
                        case Type _ when p == typeof(double):
                        case Type _ when p == typeof(decimal):
                        case Type _ when p == typeof(string):
                            item = new ControlEditor(config);
                            break;
                        case Type _ when p == typeof(DateTime):
                        case Type _ when p == typeof(DateTimeOffset):
                            item = new ControlDateTime(config);
                            break;
                        case Type _ when p == typeof(ICommand):
                            item = new ControlButton(config);
                            break;
                        case Type _ when p == typeof(bool):
                             item = new ControlCheckbox(config);
                            break;
                        case Type _ when p.IsEnum:
                            item = new ControlCombo(config);
                            break;
                        case Type _ when p == typeof(object):
                            item = new ControlLabel(config);
                            break;
                    }
                    break;
            }

            return item;
        }

        public virtual bool HasValidation() => _property.GetAttributes<AutoFormsValidationAttribute>()?.Length > 0;

        public List<AutoFormsValidationAttribute> GetValidations() => _property.GetAttributes<AutoFormsValidationAttribute>()?.ToList();

        public static T GetFilteredAttribute<T>(int filter, PropertyInfo prop) where T : AutoFormsFilteredAttribute
        {
            if (prop == null)
                return null;

            return GetFilteredAttribute(filter, prop.GetAttributes<T>());            
        }

        public static T GetFilteredAttribute<T>(int filter, T[] attribs) where T : AutoFormsFilteredAttribute
        {
            if (attribs == null || attribs.Length == 0)
                return null;

            var list = attribs.ToList();

            if (filter > 0)
            {
                list = list.Where(x => (x.Filter & filter) == filter).ToList();
            }
            else
            {
                list = list.Where(x => x.Filter == 0).ToList();
            }

            if (list == null || list.Count == 0)
                return null;

            return list[0];
        }

        protected static ControlBase CreateCustomControl(string controlType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(controlType);
                if (type == null)
                    continue;

                var cl = Activator.CreateInstance(type);

                return cl as ControlBase;
            }
            return null;
        }

        public virtual void Initialize()
        {
            if (Content != null)
                return;

            var v = InitializeControl();
            if (v == null)
                return;

            if (v is Grid g)
            {
                g.Padding = new Thickness(_attribute.PaddingLeft, _attribute.PaddingTop, _attribute.PaddingRight, _attribute.PaddingBottom);
            }
            else
            {
                v.Margin = new Thickness(_attribute.PaddingLeft, _attribute.PaddingTop, _attribute.PaddingRight, _attribute.PaddingBottom);
            }

            if (string.IsNullOrEmpty(_attribute.IsVisible) == false)
            {
                SetBinding(IsControlVisibleProperty, new Binding(FullPath(_attribute.IsVisible), BindingMode.TwoWay, new NullableConverter(), typeof(bool)));
            }
            if (string.IsNullOrEmpty(_attribute.IsEnabled) == false)
            {
                SetBinding(IsControlEnabledProperty, new Binding(FullPath(_attribute.IsEnabled), BindingMode.TwoWay, new NullableConverter(), typeof(bool)));
            }
            if(string.IsNullOrEmpty(_attribute.IsFocused) == false && _control != null)
            {
                _control.SetBinding(IsFocusedProperty, new Binding(FullPath(_attribute.IsFocused)));
            }

            Content = v;

        }

        protected virtual View InitializeControl()
        {
            Grid g;
            if (_attribute.Orientation == AutoFormsOrientation.Horizontal || _attribute.Orientation == AutoFormsOrientation.HorizontalReversed)
            {

                double layoutPercentage = _attribute.LayoutHorizontalPercentageOverride == -1 
                                          ? _config.LayoutHorizontalPercentage 
                                          : _attribute.LayoutHorizontalPercentageOverride;
                
                g = new Grid
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition {Width = new GridLength(layoutPercentage, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength((1-layoutPercentage), GridUnitType.Star)},
                    },
                    RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto}
                    }
                };
            }
            else
            {
                g = new Grid
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition {Width = GridLength.Star}
                    },
                    RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto},
                        new RowDefinition {Height = GridLength.Auto},
                    },
                };
            }

            View v = null;
            v = _labelControl = CreateControlLabel();
            if (v != null)
            {
                g.Children.Add(v);
            }
                

            v = _control = CreateControlContainer(BindingName, _propertyType, _attribute.Orientation);
            SetHorizontalLayoutOptions(v, _attribute.HorizontalControlOptions);
            if(v != null)
                g.Children.Add(v);

            //g.DebugGrid(Color.Blue);

            return g;
        }

        protected static void SetHorizontalLayoutOptions(View v, AutoFormsLayoutOptions layoutOptions)
        {
            switch (layoutOptions)
            {
                case (AutoFormsLayoutOptions.Start):
                    v.HorizontalOptions = LayoutOptions.Start;
                    SetLabelTextAlignment(v, TextAlignment.Start);
                    break;
                case (AutoFormsLayoutOptions.Center):
                    v.HorizontalOptions = LayoutOptions.Center;
                    SetLabelTextAlignment(v, TextAlignment.Center);
                    break;
                case (AutoFormsLayoutOptions.End):
                    v.HorizontalOptions = LayoutOptions.End;
                    SetLabelTextAlignment(v, TextAlignment.End);
                    break;
                case (AutoFormsLayoutOptions.Fill):
                    v.HorizontalOptions = LayoutOptions.Fill;
                    break;
                case (AutoFormsLayoutOptions.Default):
                default:
                    // do not override control's defaults
                    break;
            }
        }

        private static void SetLabelTextAlignment(View v, TextAlignment a)
        {
            if (v is Label l)
                l.HorizontalTextAlignment = a;
        }

        public virtual View CreateControlLabel()
        {
            if (string.IsNullOrEmpty(Label))
                return null;

            int row = 0;
            int col = _attribute.Orientation == AutoFormsOrientation.HorizontalReversed ? 1 : 0;
            return CreateLabel(Label, LabelStyle, _attribute.Orientation, col, row);
        }

        protected string FullPath(string bindingName)
        {
            int index = BindingName.LastIndexOf('.');
            if (index == -1)
                return bindingName;

            var path = BindingName.Substring(0, index + 1) + bindingName;

            return path;
        }

        public virtual void SetGroupedProperty(PropertyInfo property) { }

        protected virtual View CreateLabel(string label, Style labelStyle, AutoFormsOrientation orientation, int col = 0, int row = 0)
        {

            var l = new Label
            {
                Style = _labelStyleOverride ?? labelStyle,
                Text = label,
                VerticalOptions =
                orientation == AutoFormsOrientation.Horizontal && _attribute.HeightRequest > 0
                    ? LayoutOptions.StartAndExpand
                    : LayoutOptions.CenterAndExpand, 
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Fill,
                HorizontalTextAlignment = orientation == AutoFormsOrientation.Horizontal
                    ? TextAlignment.End 
                    : TextAlignment.Start,
                LineBreakMode = LineBreakMode.WordWrap,
                Margin = new Thickness(0, 0, 0, 0),
                GestureRecognizers =
                {
                    new TapGestureRecognizer
                    {
                        Command = new Command(OnSelectedText)
                    }
                }
            };

            if (string.IsNullOrEmpty(_attribute.IsVisible) == false)
            {
                l.SetBinding(IsVisibleProperty, new Binding(_attribute.IsVisible, BindingMode.TwoWay));
            }

            //update the horizontal options if provided
            if (_attribute.Orientation == AutoFormsOrientation.Horizontal)
                SetHorizontalLayoutOptions(l, _attribute.HorizontalLabelOptions);

            Grid.SetColumn(l, col);
            Grid.SetRow(l, row);

            return l;
        }

        protected virtual void OnSelectedText() {}

        protected View CreateControlContainer(string bindingName, Type fieldType, AutoFormsOrientation orientation)
        {
            var c = CreateControl(bindingName, fieldType);
            if (c == null)
                return null;

            if (_attribute.ControlWidthRequest > 0)
            {
                c.WidthRequest = _attribute.ControlWidthRequest;
            }

            SetHorizontalLayoutOptions(c, _attribute.HorizontalControlOptions);

            if(orientation == AutoFormsOrientation.Horizontal)
            {
                Grid.SetColumn(c, 1);
            }
            else if (orientation == AutoFormsOrientation.HorizontalReversed)
            {
                Grid.SetColumn(c, 0);
            }
            else
            {
                Grid.SetRow(c, 1);
            }
            

            return c;
        }

        public virtual View GetControlContainer()
        {
            if (_control != null)
            {
                LayoutRoot.Children.Clear();

                return _control;
            }

            return this;
        }

        public void RemoveLabelControl()
        {
            if (LayoutRoot == null || _labelControl == null)
            {
                return;
            }

            if (_config.Attribute.Orientation == AutoFormsOrientation.Horizontal)
                LayoutRoot.ColumnSpacing = 0;

            LayoutRoot.Children.Remove(_labelControl);
        }

        protected abstract View CreateControl(string bindingName, Type fieldType);

        protected void RecurseVisibility(View control, bool val) => RecurseControl(control, val, (v, bval) => { v.IsVisible = bval; });

        protected void RecurseEnabled(View control, bool val) => RecurseControl(control, val, (v, bval) => { v.IsEnabled = bval; });

        protected void RecurseControl(View control, bool val, Action<View, bool> fnEdit)
        {
            if (control == null || fnEdit == null)
                return;

            fnEdit(control, val);

            if(control is Grid grid)
            {
                grid.ColumnSpacing = val ? _gridSpacing : 0;
                grid.RowSpacing = val ? _gridSpacing : 0;

                foreach (var g in grid.Children)
                {
                    RecurseControl(g, val, fnEdit);
                }
            }
        }

        private static Style GetStyle(string styleName)
        {
            Style style = null;

            if (string.IsNullOrEmpty(styleName) == false &&
                Application.Current.Resources.TryGetValue(styleName, out object labelStyleOverride))
            {
                style = (Style)labelStyleOverride;
            }

            return style;
        }

    }
}
