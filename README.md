**AutoForms** is a Xamarin.Forms control that can dynamically generate UI powered by the data model you give it.

To use **AutoForms** simply add the control in your xaml and then bind it to your data model.  On your model you add certain attributes to each property telling AutoForms how it should behave.

### Example
**xaml**
```xml
<AutoForms LabelStyle="{StaticResource DefaultLabelStyle}"/>
```

**C#**
```cs
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
    }
```

| UWP      | Android |
| ----------- | ----------- |
| <img src="./Screenshots/helloworld-uwp.png" width="400" />     | <img src="./Screenshots/helloworld-android.png" width="400" />       |
