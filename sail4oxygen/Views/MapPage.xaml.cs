using sail4oxygen.Services;
using sail4oxygen.ViewModels;

namespace sail4oxygen.Views;

public partial class MapPage : ContentPage
{
    ViewModels.MapPageVM mapPageViewModel = new();

    public MapPage()
	{
		BindingContext = mapPageViewModel;
		InitializeComponent();
	}
    
	async void OnSave_Clicked(System.Object sender, System.EventArgs e)
	{
		((MapPageVM)BindingContext).SaveLocation();
		//await Shell.Current.GoToAsync("..");
	}
	
	async void OnActivateGPS_Clicked(System.Object sender, System.EventArgs e)
	{
		LocationService.Instance.ManualLocation = false;
		_= LocationService.Instance.GetLocation();
		((MapPageVM)BindingContext).SaveLocation();
		await Shell.Current.GoToAsync("..");
	}
}
