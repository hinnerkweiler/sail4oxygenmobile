using sail4oxygen.Models;
using sail4oxygen.ViewModels;

namespace sail4oxygen.Views.Components;

public partial class Calculator : ContentView
{
    public CoordinateCalculatorVM ViewModel { get; }

    public event EventHandler<CoordinateCalculatorResult> CoordinatesSet;
    public event EventHandler Canceled;

    public Calculator()
    {
        InitializeComponent();

        ViewModel = new CoordinateCalculatorVM();
        BindingContext = ViewModel;

        ViewModel.CoordinatesSet += OnCoordinatesSet;
        ViewModel.Canceled += OnCanceled;
    }

    public void InitializeCoordinates(double latitude, double longitude)
    {
        ViewModel.SetFromDecimalCoordinates(latitude, longitude);
    }

    private void OnCoordinatesSet(object sender, CoordinateCalculatorResult result)
    {
        CoordinatesSet?.Invoke(this, result);
    }

    private void OnCanceled(object sender, EventArgs e)
    {
        Canceled?.Invoke(this, e);
    }
}