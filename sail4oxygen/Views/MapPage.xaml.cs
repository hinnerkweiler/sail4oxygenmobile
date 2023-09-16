namespace sail4oxygen.Views;

public partial class MapPage : ContentPage
{
    public static ViewModels.MapPageVM MapPageViewModel = new();

    public MapPage()
	{
		BindingContext = MapPageViewModel;
		InitializeComponent();
	}
}
