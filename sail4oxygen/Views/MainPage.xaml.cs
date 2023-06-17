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
    }

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		MainPageVM.MyLocation = await MainPageVM.GetLocation();

        MainPageVM.TheFileToSend = await MainPageVM.SelectFile(null);

        if ((MainPageVM.TheFileToSend == null) || (MainPageVM.TheFileToSend.FullPath == ""))
        {
            await DisplayAlert("Alert", "No file was selected", "OK");
            return;
        }

        MainPageVM.SendEMail();
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

    
}


