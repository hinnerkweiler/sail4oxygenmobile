namespace sail4oxygen.Models;

public sealed class CoordinateCalculatorResult : EventArgs
{
    public double Latitude { get; }
    public double Longitude { get; }

    public CoordinateCalculatorResult(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}

