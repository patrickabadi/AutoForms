using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForms
{
    public class AutoFormsListUIAttribute : Attribute
    {
        public string ColumnHeaderGridStyle { get; private set; }
        public string ColumnHeaderLabelStyle { get; private set; }
        public string ActionButtonStyle { get; private set; }
        public string ColumnHeaderSortButtonStyle { get; private set; }
        public string AlternateListItemStyle { get; private set; }
        public double ListItemPaddingLeft { get; private set; }
        public double ListItemPaddingRight { get; private set; }
        public double ListItemPaddingTop { get; private set; }
        public double ListItemPaddingBottom { get; private set; }
        public double LabelButtonsPaddingTop { get; private set; }
        public double LabelButtonsPaddingBottom { get; private set; }
        public double ListHeaderPaddingTop { get; private set; }
        public double ListHeaderPaddingBottom { get; private set; }

        public double ColumnSpacing { get; private set; }

        public AutoFormsListUIAttribute(
            string columnHeaderGridStyle = null,
            string columnHeaderLabelStyle = null,
            string actionButtonStyle = null,
            string columnHeaderSortButtonStyle = null,
            string alternateListItemStyle = null,
            double listItemPaddingLeft = 25,
            double listItemPaddingRight = 25,
            double listItemPaddingTop = 5,
            double listItemPaddingBottom = 5,
            double labelButtonsPaddingTop = 0,
            double labelButtonsPaddingBottom = 27,
            double listHeaderPaddingTop = 10,
            double listHeaderPaddingBottom = 10,
            double columnSpacing = 0)
        {
            ColumnHeaderGridStyle = columnHeaderGridStyle;
            ColumnHeaderLabelStyle = columnHeaderLabelStyle;
            ActionButtonStyle = actionButtonStyle;
            ColumnHeaderSortButtonStyle = columnHeaderSortButtonStyle;
            AlternateListItemStyle = alternateListItemStyle;
            ListItemPaddingLeft = listItemPaddingLeft;
            ListItemPaddingRight = listItemPaddingRight;
            ListItemPaddingTop = listItemPaddingTop;
            ListItemPaddingBottom = listItemPaddingBottom;
            LabelButtonsPaddingTop = labelButtonsPaddingTop;
            LabelButtonsPaddingBottom = labelButtonsPaddingBottom;
            ListHeaderPaddingTop = listHeaderPaddingTop;
            ListHeaderPaddingBottom = listHeaderPaddingBottom;
            ColumnSpacing = columnSpacing;
        }
    }
}