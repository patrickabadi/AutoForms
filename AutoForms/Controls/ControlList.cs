using AutoForms.Common;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ControlList : ControlBase 
    {
        protected Grid _labelsButtons { get; private set; }
        protected Grid _header { get; private set; }
        protected ListView _listView { get; private set; }

        public ListView ListView => _listView;

        public bool HasActions => !string.IsNullOrEmpty(_attribList.OnDeleteCommand) || !string.IsNullOrEmpty(_attribList.OnViewCommand) || !string.IsNullOrEmpty(_attribList.OnEditCommand);

        protected AutoFormsListAttribute _attribList => _attribute as AutoFormsListAttribute;

        public const double ActionButtonWidth = 160;

        private Color headerBackgroundColor = Color.FromHex("#888888");

        public string EditCommand => _attribList.OnEditCommand;
        public string ViewCommand => _attribList.OnViewCommand;
        public string DeleteCommand => _attribList.OnDeleteCommand;

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ControlList), propertyChanged: OnItemsSourceChanged);

        protected Grid _gridList;

        protected Label _emptyListMessage;

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public ControlList(ControlConfig config) : base(config)
        {
        }

        protected override View InitializeControl()
        {
            
            var g = new Grid
            {
                
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.StartAndExpand,
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }, // label and buttons
                    new RowDefinition { Height = GridLength.Auto }, // header
                    new RowDefinition { Height = GridLength.Auto }, // listview
                    new RowDefinition {Height = new GridLength(1)} //dividing line
                },
                Children =
                {
                    CreateLabelsButtons(),
                    CreateHeader(),
                    CreateListView(),
                }
                
            };
            var v = CreateEmptyListLabel();
            if (v != null)
            {
                g.Children.Add(v);
            }

            if (_attribList.DisplaySeparator)
            {
                var div = new BoxView { HeightRequest = 1, Color = Color.FromRgb(238, 238, 238) };
                g.Children.Add(div, 0, 3);
            }

            return g;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext == null|| _labelsButtons == null)
                return;

            var props = BindingContext.GetPropertyAttributes<AutoFormsButtonAttribute>();
            if (props == null || props.Count == 0)
                return;

            var list = _attribList.Commands;
            if (list == null || list.Length == 0)
                return;

            while (_labelsButtons.ColumnDefinitions.Count > 1)
                _labelsButtons.ColumnDefinitions.RemoveAt(1);

            while (_labelsButtons.Children.Count > 1)
                _labelsButtons.Children.RemoveAt(1);

            foreach(var l in list)
            {
                foreach (var prop in props)
                {
                    var property = prop.Item1;
                    var attribute = prop.Item2[0];

                    if (property.Name != l)
                        continue;

                    Style style = null;
                    if (!Application.Current.Resources.TryGetValue(attribute.ButtonStyle, out object obj))
                    {

                    }
                    else
                    {
                        style = (Style)obj;
                    }

                    _labelsButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    var b = new Button
                    {
                        Style = style,
                        Text = attribute.Text
                    };
                    b.SetBinding(ImageButton.CommandProperty, new Binding(property.Name));

                    Grid.SetColumn(b, _labelsButtons.ColumnDefinitions.Count - 1);
                    _labelsButtons.Children.Add(b);

                }
            }

        }

        public override bool HasValidation()
        {
            return false;
        }

        protected override View CreateControl(string bindingName, Type fieldType)
        {
            return null;
        }

        protected View CreateLabelsButtons()
        {
            _labelsButtons = new Grid
            {
                Padding = new Thickness(0,0,0,27),
            };
            Grid.SetRow(_labelsButtons, 0);

            var type = GetListItemType();
            if (type != null)
            {
                _labelsButtons.Padding = new Thickness(25, 5, 25, 5);                
            }
            

            if (string.IsNullOrEmpty(Label) == false)
            {
                _labelsButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                var lbl = CreateLabel(Label, LabelStyle, _attribute.Orientation);
                _labelsButtons.Children.Add(lbl);
            }

            return _labelsButtons;
        }

        public Type GetListItemType()
        {
            if (_propertyType == null)
                return null;

            try
            {
                return _propertyType.GetGenericArguments()[0];
            }
            catch
            {
                return null;
            }            
        }

        public void ClearValidation()
        {
            if (_gridList == null)
                return;

            foreach (var v in _gridList.Children)
            {
                var item = v as ListViewItem;
                if (item == null)
                    continue;

                if (item.ControlValidation != null)
                {
                    item.ControlValidation.Clear();
                }
            }
        }

        public View CheckValidation()
        {
            if (_gridList == null)
                return null;

            View firstInvalidItem = null;
            foreach (var v in _gridList.Children)
            {
                var item = v as ListViewItem;
                if (item == null)
                    continue;

                if (item.ControlValidation != null && !item.ControlValidation.IsValid())
                {
                    if (firstInvalidItem == null)
                        firstInvalidItem = v;
                }
            }

            return firstInvalidItem;
        }

        protected View CreateHeader()
        {
            var type = GetListItemType();
            if (type == null)
            {
                _header.BackgroundColor = headerBackgroundColor;
                return _header;
            }

            _header = new Grid
            {
                ColumnSpacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            Grid.SetRow(_header, 1);

            _header.Padding = new Thickness(25,5,25,5);

            var headerStyle = AutoFormsConstants.ListHeaderStyle;

            if(string.IsNullOrEmpty(headerStyle) == false && 
                Application.Current.Resources.TryGetValue(headerStyle, out object headerObj))
            {
                _header.Style = (Style)headerObj;
            }
            else
            {
                _header.BackgroundColor = headerBackgroundColor;
            }

            var props = AttributeHelper.GetPropertyAttributes<AutoFormsListItemAttribute>(type);

            var style = LabelStyle;
            Style sortButtonStyle = null;

            var headerLabelStyle = AutoFormsConstants.AutoFormsListHeaderLabelStyle;
            string headerSortButtonStyle = null; // for now

            if (string.IsNullOrEmpty(headerLabelStyle) == false &&
                Application.Current.Resources.TryGetValue(headerLabelStyle, out object obj))
            {
                style = (Style)obj;
            }

            if (string.IsNullOrEmpty(headerSortButtonStyle) == false &&
                Application.Current.Resources.TryGetValue(headerSortButtonStyle, out object sortButtonStyleObj))
            {
                sortButtonStyle = (Style)sortButtonStyleObj;
            }

            string sortedStateProperty = null;
            if (_config.Attribute is AutoFormsListAttribute listAttribute)
                sortedStateProperty = listAttribute.SortedStateProperty;

            foreach (var p in props)
            {
                var property = p.Item1;
                var attribute = GetFilteredAttribute(Filter, p.Item2);

                if (property == null || attribute == null)
                    continue;

                _header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(attribute.Value, attribute.GridType) });

                View view;
                //if (attribute.SortValue != null && !string.IsNullOrWhiteSpace(sortedStateProperty))
                //{
                //    view = new SortButton
                //    {
                //        Text = attribute.Label,
                //        VerticalOptions = LayoutOptions.CenterAndExpand,
                //        HorizontalOptions = LayoutOptions.Fill,
                //        SortValue = attribute.SortValue
                //    };
                //    if (sortButtonStyle != null)
                //        view.Style = sortButtonStyle;
                //    view.SetBinding(SortButton.SortedStateProperty, sortedStateProperty);
                //}
                //else
                {
                    view = new Label
                    {
                        Style = style,
                        Text = attribute.Label,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.Fill,
                        HorizontalTextAlignment = attribute.HorizontalHeaderAlignment,
                        LineBreakMode = LineBreakMode.WordWrap,
                    };

                }

                Grid.SetColumn(view, _header.ColumnDefinitions.Count - 1);
                _header.Children.Add(view);
            }

            if (HasActions)
            {
                _header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(ActionButtonWidth, GridUnitType.Absolute) });
            }

            //_header.DebugGrid(Color.Blue);

            return _header;
        }

        protected virtual View CreateListView()
        {
            if(_attribList.HeightRequest > 0)
            {

                _listView = new ListView
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    //BackgroundColor = Color.Yellow,
                    HeightRequest = _attribute.HeightRequest < 0 ? 200 : _attribute.HeightRequest,
                    HasUnevenRows = true,
                    ItemTemplate = new DataTemplate(typeof(ListViewItemCell)),
                    Margin = new Thickness(0, 0, 0, 0),
                };
                Grid.SetRow(_listView, 2);

                this.SetBinding(ControlList.ItemsSourceProperty, new Binding(BindingName, BindingMode.OneWay));
                _listView.SetBinding(ListView.ItemsSourceProperty, new Binding(BindingName, BindingMode.OneWay));

                return _listView;                
            }            
            else
            {
                this.SetBinding(ControlList.ItemsSourceProperty, new Binding(BindingName, BindingMode.TwoWay));

                _gridList = new Grid
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                };

                Grid.SetRow(_gridList, 2);

                return _gridList;
            }
            
        }

        View CreateEmptyListLabel()
        {
            var listAttribute = _config.Attribute as AutoFormsListAttribute;

            if (listAttribute == null || string.IsNullOrEmpty(listAttribute.EmptyListMessage))
            {
                return null;
            }

            var fontSize = listAttribute.NestedListView ? 16 : 22;

            _emptyListMessage = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,                
                Text = listAttribute.EmptyListMessage,
                IsVisible = false,
                InputTransparent = true,
                Margin = 20,
                FontSize = fontSize
            };

            Grid.SetRow(_emptyListMessage, 2);
            return _emptyListMessage;
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldVal, object newVal)
        {
            IEnumerable newValue = newVal as IEnumerable;
            var layout = (ControlList)bindable;

            var observableCollection = newValue as INotifyCollectionChanged;
            if (observableCollection != null)
            {
                observableCollection.CollectionChanged += layout.OnItemsSourceCollectionChanged;
                layout.CheckEmptyList();
            }

            if (layout._gridList == null)
            {
                return;
            }

            var g = layout._gridList;

            
            g.RowDefinitions.Clear();
            g.Children.Clear();
            if (newValue != null)
            {
                foreach (var item in newValue)
                {
                    g.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    var v = layout.CreateChildView(item);
                    Grid.SetRow(v, g.RowDefinitions.Count - 1);
                    g.Children.Add(v);

                }
            }
        }

        ListViewItem CreateChildView(object item)
        {
            var v = new ListViewItem
            {
                BindingContext = item
            };
            return v;
        }

        void CheckEmptyList()
        {
            if (_emptyListMessage == null)
            {
                return;
            }

            int count = 0;
            if (ItemsSource != null)
            {
                foreach (var i in ItemsSource)
                {
                    count++;
                    break;
                }
            }
            _emptyListMessage.IsVisible = count > 0 ? false : true;
        }

        void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CheckEmptyList();
            
            if (_gridList == null)
            {
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                _gridList.Children.Clear();
            }

            if (e.OldItems != null)
            {

                _gridList.Children.RemoveAt(e.OldStartingIndex);
            }

            if (e.NewItems != null)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    var item = e.NewItems[i];
                    var v = CreateChildView(item);
                    _gridList.RowDefinitions.Insert(e.NewStartingIndex + i, new RowDefinition { Height = GridLength.Auto });
                    Grid.SetRow(v, e.NewStartingIndex + i);
                    _gridList.Children.Insert(e.NewStartingIndex + i, v);

                }
            }
        }
    }

    
}
