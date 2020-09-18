using AutoForms.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace AutoForms.Controls
{
    public class ControlHorizontalGroup : ControlBase
    {
        Grid _grid;

        bool _foundStar;

        public ControlHorizontalGroup(ControlConfig config) : base(config)
        {
            _foundStar = false;
        }

        protected override View InitializeControl()
        {
            if (_config.Attribute.Orientation == AutoFormsOrientation.Horizontal)
                return base.InitializeControl();

            _grid = new Grid
            {
                ColumnSpacing = 4,
                VerticalOptions = LayoutOptions.StartAndExpand,
                RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto},
                    },
            };

            AddControl(_config);

            return _grid;


        }

        protected override View CreateControl(string bindingName, Type fieldType)
        {
            if (_config.Attribute.Orientation == AutoFormsOrientation.Vertical)
                return null;

            _grid = new Grid
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                RowDefinitions =
                    {
                        new RowDefinition {Height = GridLength.Auto},
                    },
            };

            // creating a control but removing the label so we create the label grid with zero width percentage and then after remove the actual
            // label.
            var config = new ControlConfig(
                0,
                _config.Attribute,
                _config.LabelStyle,
                _config.EditorStyle,
                _config.SeparatorColor,
                _config.Property,
                _config.Filter);

            var item = CreateControl(config);
            if (item == null)
                return _grid;

            item.Initialize();
            item.RemoveLabelControl();

            var control = item;

            var horizontalAttribute = GetFilteredAttribute<AutoFormsHorizontalGroupAttribute>(_config.Filter, _config.Property);

            if (horizontalAttribute != null)
            {
                if (horizontalAttribute.GridType == GridUnitType.Star)
                    _foundStar = true;

                _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(horizontalAttribute.Value, horizontalAttribute.GridType) });
            }
            else
            {
                _foundStar = true;
                _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            Grid.SetColumn(control, _grid.ColumnDefinitions.Count - 1);
            _grid.Children.Add(control);

            //_grid.DebugGrid(Color.Red);

            return _grid;
        }

        protected void AddControl(ControlConfig config)
        {
            var item = CreateControl(config);
            if (item == null)
            {
                Debug.WriteLine($"ControlHorizontalGroup.AddControl - unable to create control of name: {config.Property.Name}");
                return;
            }

            Groups.Add(item);

            item.Initialize();

            item.LayoutRoot.Padding = new Thickness(0, 0, 0, 0);

            View control = item;

            if (string.IsNullOrEmpty(config.Label) && item.LayoutRoot.Children.Count >= 1)
            {
                control = item.LayoutRoot.Children[item.LayoutRoot.Children.Count - 1];
                item.LayoutRoot.Children.Clear();
            }

            var horizontalAttribute = GetFilteredAttribute<AutoFormsHorizontalGroupAttribute>(config.Filter, config.Property);

            if (horizontalAttribute != null)
            {
                if (horizontalAttribute.GridType == GridUnitType.Star)
                    _foundStar = true;

                _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(horizontalAttribute.Value, horizontalAttribute.GridType) });
            }
            else
            {
                _foundStar = true;

                _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            Grid.SetColumn(control, _grid.ColumnDefinitions.Count - 1);
            control.VerticalOptions = LayoutOptions.End;

            _grid.Children.Add(control);

            // we do this if all the grid items only have exact widths or auto, so we make a new one at the end to take up the rest of the space
            if (_foundStar == false && _grid.Children.Count - 1 == _config.Attribute.Grouped?.Length)
            {
                _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            //_grid.DebugGrid(Color.Red);

        }

        public override bool HasValidation()
        {
            if (base.HasValidation())
                return true;

            foreach(var i in _grid.Children)
            {
                var control = i as ControlBase;
                if (control == null)
                    continue;

                if (control.HasValidation())
                    return true;
            }

            return false;
        }

        public override void SetGroupedProperty(PropertyInfo property)
        {
            var attributes = property.GetAttributes<AutoFormsAttribute>();

            var attribute = GetFilteredAttribute(_config.Filter, attributes);
            if (attribute == null)
            {
                Debug.WriteLine($"ControlHorizontalGroup.SetGroupedProperty - unable to create control of name: {property.Name}");
                return;
            }

            var config = new ControlConfig(
                0.5,
                attribute,
                _config.LabelStyle,
                _config.EditorStyle,
                _config.SeparatorColor,
                property,
                _config.Filter);

            AddControl(config);

        }
    }
}
