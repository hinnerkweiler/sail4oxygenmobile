using sail4oxygen.Models;

namespace sail4oxygen.Views;

public partial class CoordinateCalculatorModalPage : ContentPage
{
    private readonly Action<double, double> applyCoordinates;

    public CoordinateCalculatorModalPage(Location currentLocation, Action<double, double> applyCoordinates)
    {
        InitializeComponent();
        this.applyCoordinates = applyCoordinates;

        var latitude = currentLocation?.Latitude ?? 0;
        var longitude = currentLocation?.Longitude ?? 0;
        CalculatorView.InitializeCoordinates(latitude, longitude);

        CalculatorView.CoordinatesSet += OnCoordinatesSet;
        CalculatorView.Canceled += OnCanceled;
    }

    private async void OnCoordinatesSet(object sender, CoordinateCalculatorResult result)
    {
        applyCoordinates?.Invoke(result.Latitude, result.Longitude);
        await Navigation.PopModalAsync();
    }

    private async void OnCanceled(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    protected override void OnDisappearing()
    {
        CalculatorView.CoordinatesSet -= OnCoordinatesSet;
        CalculatorView.Canceled -= OnCanceled;
        base.OnDisappearing();
    }
}

