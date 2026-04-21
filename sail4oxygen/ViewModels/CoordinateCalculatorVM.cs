using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace sail4oxygen.ViewModels;

public partial class CoordinateCalculatorVM : ObservableObject
{
    [ObservableProperty]
    private string latitudeDegrees = "0";

    [ObservableProperty]
    private string latitudeMinutes = "0";

    [ObservableProperty]
    private int selectedLatitudeHemisphereIndex;

    [ObservableProperty]
    private string longitudeDegrees = "0";

    [ObservableProperty]
    private string longitudeMinutes = "0";

    [ObservableProperty]
    private int selectedLongitudeHemisphereIndex;

    [ObservableProperty]
    private string validationMessage = string.Empty;

    [ObservableProperty]
    private bool isInputValid;

    public IRelayCommand SetCoordinatesCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public event EventHandler<Models.CoordinateCalculatorResult> CoordinatesSet;
    public event EventHandler Canceled;

    public CoordinateCalculatorVM()
    {
        SetCoordinatesCommand = new RelayCommand(OnSetCoordinates, () => IsInputValid);
        CancelCommand = new RelayCommand(() => Canceled?.Invoke(this, EventArgs.Empty));
        ValidateInput();
    }

    public void SetFromDecimalCoordinates(double latitude, double longitude)
    {
        SelectedLatitudeHemisphereIndex = latitude < 0 ? 1 : 0;
        SelectedLongitudeHemisphereIndex = longitude < 0 ? 1 : 0;

        var absoluteLatitude = Math.Abs(latitude);
        var absoluteLongitude = Math.Abs(longitude);

        var latitudeDegreeValue = (int)Math.Floor(absoluteLatitude);
        var longitudeDegreeValue = (int)Math.Floor(absoluteLongitude);

        var latitudeMinuteValue = (absoluteLatitude - latitudeDegreeValue) * 60d;
        var longitudeMinuteValue = (absoluteLongitude - longitudeDegreeValue) * 60d;

        LatitudeDegrees = latitudeDegreeValue.ToString(CultureInfo.InvariantCulture);
        LatitudeMinutes = latitudeMinuteValue.ToString("00.000", CultureInfo.InvariantCulture);
        LongitudeDegrees = longitudeDegreeValue.ToString(CultureInfo.InvariantCulture);
        LongitudeMinutes = longitudeMinuteValue.ToString("00.000", CultureInfo.InvariantCulture);

        ValidateInput();
    }

    partial void OnLatitudeDegreesChanged(string value) => ValidateInput();
    partial void OnLatitudeMinutesChanged(string value) => ValidateInput();
    partial void OnSelectedLatitudeHemisphereIndexChanged(int value) => ValidateInput();
    partial void OnLongitudeDegreesChanged(string value) => ValidateInput();
    partial void OnLongitudeMinutesChanged(string value) => ValidateInput();
    partial void OnSelectedLongitudeHemisphereIndexChanged(int value) => ValidateInput();

    private void OnSetCoordinates()
    {
        if (!TryBuildCoordinate(out var latitude, out var longitude, out var errorMessage))
        {
            ValidationMessage = errorMessage;
            IsInputValid = false;
            SetCoordinatesCommand.NotifyCanExecuteChanged();
            return;
        }

        ValidationMessage = string.Empty;
        CoordinatesSet?.Invoke(this, new Models.CoordinateCalculatorResult(latitude, longitude));
    }

    private void ValidateInput()
    {
        IsInputValid = TryBuildCoordinate(out _, out _, out var errorMessage);
        ValidationMessage = IsInputValid ? string.Empty : errorMessage;
        SetCoordinatesCommand.NotifyCanExecuteChanged();
    }

    private bool TryBuildCoordinate(out double latitude, out double longitude, out string errorMessage)
    {
        latitude = 0;
        longitude = 0;

        if (!TryParseCoordinate(LatitudeDegrees, LatitudeMinutes, 90, out var parsedLatitude, out errorMessage))
            return false;

        if (!TryParseCoordinate(LongitudeDegrees, LongitudeMinutes, 180, out var parsedLongitude, out errorMessage))
            return false;

        latitude = SelectedLatitudeHemisphereIndex == 1 ? -parsedLatitude : parsedLatitude;
        longitude = SelectedLongitudeHemisphereIndex == 1 ? -parsedLongitude : parsedLongitude;

        return true;
    }

    private static bool TryParseCoordinate(
        string degreesText,
        string minutesText,
        int maxDegrees,
        out double coordinate,
        out string errorMessage)
    {
        coordinate = 0;
        errorMessage = "Please enter valid degree and minute values.";

        var normalizedDegreesText = NormalizeInput(degreesText);
        var normalizedMinutesText = NormalizeInput(minutesText);

        if (!double.TryParse(normalizedDegreesText, NumberStyles.Float, CultureInfo.InvariantCulture, out var degrees))
            return false;

        if (!double.TryParse(normalizedMinutesText, NumberStyles.Float, CultureInfo.InvariantCulture, out var minutes))
            return false;

        if (degrees < 0 || degrees > maxDegrees)
        {
            errorMessage = $"Degrees must be between 0 and {maxDegrees}.";
            return false;
        }

        if (minutes < 0 || minutes >= 60)
        {
            errorMessage = "Minutes must be between 0 and less than 60.";
            return false;
        }

        if (Math.Abs(degrees - maxDegrees) < 0.0000001 && minutes > 0)
        {
            errorMessage = $"Minutes must be 0 when degrees are {maxDegrees}.";
            return false;
        }

        coordinate = degrees + minutes / 60d;
        return true;
    }

    private static string NormalizeInput(string value)
    {
        return (value ?? string.Empty).Trim().Replace(',', '.');
    }
}

