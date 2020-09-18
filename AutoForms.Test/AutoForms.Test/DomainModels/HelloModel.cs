using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AutoForms.Test.DomainModels
{
    public class HelloModel
    {
        public enum Relation
        {
            [Description("Parent")]
            Parent,
            [Description("Child")]
            Child,
            [Description("Grandparent")]
            Grandparent,
            [Description("No Relation")]
            NoRelation
        }

        [AutoForms("First Name")]
        public string Firstname { get; set; }

        [AutoForms("Last Name")]
        public string Lastname { get; set; }

        [AutoForms("Date of Birth")]
        public DateTime DOB { get; set; }

        [AutoForms("Relation")]
        public Relation RelationToClient { get; set; }

        [AutoForms("Notes", heightRequest: 100)]
        public string notes { get; set; }

    }
}
