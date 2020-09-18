using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AutoForms
{
    public class AutoFormsListAttribute : AutoFormsAttribute
    {
        public string [] Commands { get; private set; }
        public string ActionButtonsStyle { get; private set; }
        public string OnEditCommand { get; private set; }
        public string OnViewCommand { get; private set; }
        public string OnDeleteCommand { get; private set; }
        public string SortedStateProperty { get; private set; }
        public string EmptyListMessage { get; private set; }
        public bool DisplaySeparator { get; private set; }
        public bool NestedListView { get; private set; }

        public AutoFormsListAttribute(
            string label = null, 
            string actionButtonsStyle = null,
            string onEditCommand = null,
            string onViewCommand = null,
            string onDeleteCommand = null,
            string [] commands = null,
            string labelStyleOverride = null,
            int filter = 0,
            double heightRequest = -1,
            string isVisible = null,
            string isEnabled = null,
            string isFocused = null,
            double paddingLeft = 0,
            double paddingRight = 0,
            double paddingTop = 0,
            double paddingBottom = 0,
            string sortedStateProperty = null,
            string emptyListMessage = null,
            bool displaySeparator = false,
            bool nestedListView = false) 
            :base(
                 label, 
                 AutoFormsType.ActionList, 
                 AutoFormsOrientation.Vertical, 
                 heightRequest: heightRequest,
                 labelStyleOverride: labelStyleOverride,
                 filter: filter,
                 isVisible: isVisible,
                 isEnabled: isEnabled,
                 isFocused: isFocused,
                 paddingLeft: paddingLeft,
                 paddingRight: paddingRight,
                 paddingTop: paddingTop,
                 paddingBottom: paddingBottom)
        {
            ActionButtonsStyle = actionButtonsStyle;
            OnEditCommand = onEditCommand;
            OnViewCommand = onViewCommand;
            OnDeleteCommand = onDeleteCommand;
            Commands = commands;
            SortedStateProperty = sortedStateProperty;
            EmptyListMessage = emptyListMessage;
            DisplaySeparator = displaySeparator;
            NestedListView = nestedListView;
        }
    }
}
