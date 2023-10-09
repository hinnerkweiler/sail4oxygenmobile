namespace sail4oxygen.Views;

public partial class MapPage : ContentPage
{
    ViewModels.MapPageVM MapPageViewModel = new();

    public MapPage()
	{
		BindingContext = MapPageViewModel;
		InitializeComponent();
	}
}
