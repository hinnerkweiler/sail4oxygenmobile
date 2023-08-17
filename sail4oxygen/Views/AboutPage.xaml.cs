namespace sail4oxygen.Views;

public partial class AboutPage : ContentPage
{
    public ViewModels.AboutPageVM AboutPageVM = new();
   

    public AboutPage()
    {
        
        BindingContext = AboutPageVM;
        InitializeComponent();

    }

    async void Github_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://www.cdc.hinnerk-weiler.com/s4oapp");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    async void s4oweb_Clicked(System.Object sender, System.EventArgs e)
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

    async void settings_Clicked(System.Object sender, System.EventArgs e)
    {
        _ = Models.FaqHelper.UpdateAssets();
        await Shell.Current.GoToAsync("Onboarding");
    }
}
