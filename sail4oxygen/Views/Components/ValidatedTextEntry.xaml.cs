namespace sail4oxygen.Views.Components;

public partial class ValidatedTextEntry : ContentView
{
    public static readonly BindableProperty LabelTextProperty =
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(ValidatedTextEntry), string.Empty);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ValidatedTextEntry), string.Empty);

    public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(ValidatedTextEntry), Colors.Gray);

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(ValidatedTextEntry), string.Empty, BindingMode.TwoWay);

    public static readonly BindableProperty IsValidProperty =
        BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ValidatedTextEntry), false, BindingMode.TwoWay);

    public static readonly BindableProperty ValidationMessageProperty =
        BindableProperty.Create(nameof(ValidationMessage), typeof(string), typeof(ValidatedTextEntry), string.Empty);

    public static readonly BindableProperty RegexPatternProperty =
        BindableProperty.Create(nameof(RegexPattern), typeof(string), typeof(ValidatedTextEntry), ".*");

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public Color PlaceholderColor
    {
        get => (Color)GetValue(PlaceholderColorProperty);
        set => SetValue(PlaceholderColorProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        set => SetValue(IsValidProperty, value);
    }

    public string ValidationMessage
    {
        get => (string)GetValue(ValidationMessageProperty);
        set => SetValue(ValidationMessageProperty, value);
    }

    public string RegexPattern
    {
        get => (string)GetValue(RegexPatternProperty);
        set => SetValue(RegexPatternProperty, value);
    }

    public ValidatedTextEntry()
    {
        InitializeComponent();
    }
}
