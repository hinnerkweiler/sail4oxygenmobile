namespace sail4oxygen.Models;

public class MinutesBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(entry);
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnEntryTextChanged;
        base.OnDetachingFrom(entry);
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
        var entry = sender as Entry;
        if (entry == null) return;

        // Allow the user to clear the entry
        if (string.IsNullOrEmpty(args.NewTextValue))
        {
            return;
        }

        // Allow incomplete numeric input (e.g., "5.", "0.", or ".")
        if (args.NewTextValue == "." || args.NewTextValue.EndsWith("."))
        {
            return;
        }

        if (double.TryParse(args.NewTextValue, out double newValue))
        {
            // Check if the new value is within the valid range
            if (newValue < 0 || newValue >= 60)
            {
                entry.Text = args.OldTextValue;
            }
        }
        else
        {
            // If the new text is not a valid double, revert to the old value
            entry.Text = args.OldTextValue;
        }
    }
}