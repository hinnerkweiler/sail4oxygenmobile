using System;

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
        Routing.RegisterRoute(nameof(Onboarding), typeof(Onboarding));
        Routing.RegisterRoute(nameof(ManualPage), typeof(ManualPage));
        Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));

        Models.FaqHelper.Init();

        if (VersionTracking.Default.IsFirstLaunchEver)
        {
            Shell.Current.GoToAsync("Onboarding");
        }

        if (VersionTracking.Default.IsFirstLaunchForCurrentVersion)
        {
            _ = Models.FaqHelper.CopyReleaseItemToAppFolder(Models.FaqHelper.PdfManualFileName);
        }
    }



	private async void OnCounterClicked(object sender, EventArgs e)
	{
        if (Models.DsgvoHandler.IsDsgvoAccepted)
        {
            if (await Application.Current.MainPage.DisplayAlert(sail4oxygen.Resources.Languages.lang.SendingNoteTitle, sail4oxygen.Resources.Languages.lang.SendingNote, sail4oxygen.Resources.Languages.lang.ContinueSending,  sail4oxygen.Resources.Languages.lang.cancel))
            _ = await MainPageVM.SelectFile(null);
        }
        else
        {
            if (await Application.Current.MainPage.DisplayAlert(sail4oxygen.Resources.Languages.lang.PrivacyAlertTitle, sail4oxygen.Resources.Languages.lang.PrivacyAlertText, sail4oxygen.Resources.Languages.lang.PrivacyAlertChangesetting, sail4oxygen.Resources.Languages.lang.cancel))
            {
                await Shell.Current.GoToAsync("Onboarding");
            }
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



    async void OnGPSReload_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("MapPage");
        //MainPageVM.MyLocation = await MainPageVM.GetLocation();
    }

    
    async void EditCoordinatesButton_Clicked(System.Object sender, System.EventArgs e)
    {
        MainPageVM.IsCoordinateViewVisible = false;
    }

    async void SaveCoordinatesButton_Clicked(System.Object sender, System.EventArgs e)
    {
        MainPageVM.IsCoordinateViewVisible = true;
    }

    async void OnFileRemove_Clicked(System.Object sender, System.EventArgs e)
    {
        MainPageVM.Cleanup();
    }

    public void NewsListView_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
    {
        MainPageVM.ItemTapped(e.Item as Models.NewsItem);
    }

    void NewsListView_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        Models.NewsItem item = e.SelectedItem as Models.NewsItem;
    }

    async void GetPdfManualButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("ManualPage");
    }

}


