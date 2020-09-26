using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AutoForms.Test.DomainModels
{
    [AddINotifyPropertyChangedInterface]
    public class ListsModel
    {
        
        [AutoFormsList("Phone Numbers",
            labelStyleOverride: "AutoFormsListViewLabelStyle",
            commands: new string[] { nameof(AddCommand) },
            onDeleteCommand: nameof(DeleteCommand),
            emptyListMessage: "No phone numbers have been entered yet. Add one above.",
            nestedListView: true)]
        public ObservableCollection<PhoneNumberModel> PhoneNumbers { get; set; }

        [AutoFormsButton("Add", "AutoFormsContactAddNewPhoneButtonStyle")]
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ListsModel()
        {
            PhoneNumbers = new ObservableCollection<PhoneNumberModel>();
            AddCommand = new Command(OnAdd);
            DeleteCommand = new Command(OnDelete);
        }

        public void OnAdd()
        {
            PhoneNumbers.Add(new PhoneNumberModel());
        }

        public void OnDelete(object obj)
        {
            PhoneNumbers.Remove(obj as PhoneNumberModel);
        }
    }

    [AutoFormsListUIAttribute(
        actionButtonStyle: "AutoFormsActionButtonStyle",
        columnHeaderGridStyle: "AutoFormsListHeaderStyle",
        columnHeaderLabelStyle: "AutoFormsListHeaderLabelStyle")]
    [AddINotifyPropertyChangedInterface]
    public class PhoneNumberModel
    {
        [AutoFormsListItem("Number", 3, GridUnitType.Star, type: AutoFormsType.Entry)]
        public string Number { get; set; }
        [AutoFormsListItem("Ext", 1, GridUnitType.Star, type: AutoFormsType.Entry)]
        public string Extension { get; set; }
    }
}
