namespace sail4oxygen.Views;

public partial class Onboarding : ContentPage
{
	public double TitleHeight = 80d;

    public Onboarding()
	{
		InitializeComponent();
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
        await DisplayAlert("Registration", "", "OK");
        await Shell.Current.GoToAsync("..");
    }



    async void exit_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }



    async void installkor_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://play.google.com/store/apps/details?id=com.xylem.kor");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
