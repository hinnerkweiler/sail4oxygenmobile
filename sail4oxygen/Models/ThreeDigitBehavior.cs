namespace sail4oxygen.Models;

public class TwoDigitBehavior : Behavior<Entry>
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

        if (args.NewTextValue.Length > 2)
        {
            // Revert to old value if more than two digits are entered
            entry.Text = args.OldTextValue;
        }
        else if (!string.IsNullOrEmpty(args.NewTextValue) && !int.TryParse(args.NewTextValue, out _))
        {
            // Revert to old value if the new value is not a number
            entry.Text = args.OldTextValue;
        }
        // Allow one or two digit numbers without adding a leading zero
    }
}