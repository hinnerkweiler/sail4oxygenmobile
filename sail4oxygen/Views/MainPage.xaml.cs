using static Android.Graphics.Paint;

namespace sail4oxygen.Views;



public partial class MainPage : ContentPage
{
    public static ViewModels.MainPageVM MainPageVM = new();

    public MainPage()
	{
		BindingContext = MainPageVM;
		InitializeComponent();
        Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(FaqPage), typeof(FaqPage));
        Routing.RegisterRoute(nameof(Onboarding), typeof(Onboarding));

        if (VersionTracking.Default.IsFirstLaunchEver)
        {
            Shell.Current.GoToAsync("Onboarding");
        }

    }

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		MainPageVM.MyLocation = await MainPageVM.GetLocation();

        MainPageVM.TheFileToSend = await MainPageVM.SelectFile(null);

        if ((MainPageVM.TheFileToSend != null) && (MainPageVM.TheFileToSend.FullPath != ""))
        {
            MainPageVM.SendEMail();
        }
        else
        {
            await DisplayAlert("Alert", "No file was selected", "OK");
        }

        
    }



    async void About_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("AboutPage");
    }



    async void Faq_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("FaqPage");
    }



    async void Register_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }



    async void ReserveButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await DisplayAlert("Wouldn't it be good to...", "Backend is in the making. It will allow to enter a date and the App will list where sondes will be available that day (using previous reservation / return data). If anyone has a better approach, let me know.", "OK");
    }

    async void SondeSwitch_Toggled(System.Object sender, Microsoft.Maui.Controls.ToggledEventArgs e)
    {
        if (SondeSwitch.IsToggled)
            await DisplayAlert("To good to be ready yet...", "Backend is in the making. Select the Sonde you have on board (or maybe scan a QR code). So we know the sonde is in use ... ", "OK");
        else
            await DisplayAlert("I know... but it is comming!", "Once deactivated we just ask from a list, where the sonde has been dropped ashore.", "OK");
    }
}


