using System.Linq;
using Xamarin.Forms;

namespace AutoForms.Behaviors
{
    public class NumericInputBehavior<T> : Behavior<T> where T : View
    {
        bool _allowDecimalPoint;

        public NumericInputBehavior(bool allowDecimalPoint = false)
        {
            _allowDecimalPoint = allowDecimalPoint;
        }

        protected override void OnAttachedTo(T view)
        {
            if(typeof(T) == typeof(Entry))
            {
                (view as Entry).TextChanged += OnEntryTextChanged;
            }
            else if (typeof(T) == typeof(Editor))
            {
                (view as Editor).TextChanged += OnEntryTextChanged;
            }

            base.OnAttachedTo(view);
        }

        protected override void OnDetachingFrom(T view)
        {
            if (typeof(T) == typeof(Entry))
            {
                (view as Entry).TextChanged -= OnEntryTextChanged;
            }
            else if (typeof(T) == typeof(Editor))
            {
                (view as Editor).TextChanged -= OnEntryTextChanged;
            }

            base.OnDetachingFrom(view);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {

            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                bool isValid =
                    args.NewTextValue.ToCharArray().All(x => char.IsDigit(x) || _allowDecimalPoint && x=='.'); //Make sure all characters are numbers

                if (sender is Entry entry)
                {
                    entry.Text =
                    isValid ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
                }
                else if (sender is Editor editor)
                {
                    editor.Text =
                    isValid ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
                }

            }
        }
    }
}