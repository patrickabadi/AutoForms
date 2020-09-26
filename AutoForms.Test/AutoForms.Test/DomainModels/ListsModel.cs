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
        [AutoFormsList("Policy Numbers",
            commands: new string[] { nameof(AddPolicyCommand) },
            emptyListMessage: "No policy numbers have been entered yet. Add one above.",
            nestedListView: true)]
        public ObservableCollection<PolicyNumber> PolicyNumbers { get; set; }

        [AutoFormsButton("Add", "AddListButtonStyle")]
        public ICommand AddPolicyCommand { get; set; }

        [AutoFormsList("List with Actions",
            commands: new string[] { nameof(AddCommand) },
            onDeleteCommand: nameof(DeleteCommand),
            onEditCommand: nameof(EditCommand),
            onViewCommand: nameof(ViewCommand),
            emptyListMessage: "No phone numbers have been entered yet. Add one above.",
            nestedListView: true)]
        public ObservableCollection<PhoneNumberModel> PhoneNumbers { get; set; }

        [AutoFormsButton("Add", "AddListButtonStyle")]
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand ViewCommand { get; set; }


        public ListsModel()
        {
            PolicyNumbers = new ObservableCollection<PolicyNumber>();
            PhoneNumbers = new ObservableCollection<PhoneNumberModel>();

            AddCommand = new Command(()=> PhoneNumbers.Add(new PhoneNumberModel()));
            DeleteCommand = new Command((obj)=> PhoneNumbers.Remove(obj as PhoneNumberModel));
            EditCommand = new Command(()=> { });
            ViewCommand = new Command(()=> { });
            AddPolicyCommand = new Command(() => PolicyNumbers.Add(new PolicyNumber()));
        }

    }

    [AddINotifyPropertyChangedInterface]
    public class PolicyNumber
    {
        [AutoFormsListItem("Number", 1, GridUnitType.Star, type: AutoFormsType.Entry)]
        public int Number { get; set; }
        [AutoFormsListItem("Description", 3, GridUnitType.Star, type: AutoFormsType.Entry)]
        public string Extension { get; set; }
        public enum PolicyType { Insurance, Government, Other  }
        [AutoFormsListItem("Type", 2, GridUnitType.Star, type: AutoFormsType.Combo)]
        public PolicyType Type { get; set; }
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
