namespace sail4oxygen.Views;

public partial class Onboarding : ContentPage
{
	public double TitleHeight = 80d;

    public Onboarding()
	{
		InitializeComponent();
	}

    async void faq_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("FaqPage");
    }

    async void register_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://sail4oxygen.org#Form");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }

    async void authorize_Clicked(System.Object sender, System.EventArgs e)
    {
        // Save E-Mail and Registration to secure storage
        // Call API to Confirm App in Use
        await DisplayAlert("Registration", "Backend is not ready jet but for now let us pretend we did some server side magic and... Great, You are good to go.", "OK");
        await Shell.Current.GoToAsync("..");
    }

    async void exit_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
