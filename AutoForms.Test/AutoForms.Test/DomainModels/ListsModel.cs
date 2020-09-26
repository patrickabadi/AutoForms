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
            commands: new string[] { nameof(AddCommand) },
            onDeleteCommand: nameof(DeleteCommand),
            onEditCommand: nameof(EditCommand),
            onViewCommand: nameof(ViewCommand),
            emptyListMessage: "No phone numbers have been entered yet. Add one above.",
            nestedListView: true)]
        public ObservableCollection<PhoneNumberModel> PhoneNumbers { get; set; }

        [AutoFormsButton("Add", "DefaultButtonStyle")]
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand ViewCommand { get; set; }


        public ListsModel()
        {
            PhoneNumbers = new ObservableCollection<PhoneNumberModel>();
            AddCommand = new Command(OnAdd);
            DeleteCommand = new Command(OnDelete);
            EditCommand = new Command(OnEdit);
            ViewCommand = new Command(OnView);
        }

        public void OnAdd()
        {
            PhoneNumbers.Add(new PhoneNumberModel());
        }

        public void OnDelete(object obj)
        {
            PhoneNumbers.Remove(obj as PhoneNumberModel);
        }

        public void OnEdit(object obj)
        {

        }

        public void OnView(object obj)
        {

        }
    }

    [AddINotifyPropertyChangedInterface]
    public class PhoneNumberModel
    {
        [AutoFormsListItem("Number", 3, GridUnitType.Star, type: AutoFormsType.Entry)]
        public string Number { get; set; }
        [AutoFormsListItem("Ext", 1, GridUnitType.Star, type: AutoFormsType.Entry)]
        public string Extension { get; set; }
    }
}
