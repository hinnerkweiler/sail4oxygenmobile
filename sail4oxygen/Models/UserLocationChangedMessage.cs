namespace sail4oxygen.Models;

public class UserLocationChangedMessage
{
    // This is used when the user location changes in the MapPageVM
    
    public double Latitude { get; }
    public double Longitude { get; }

    public UserLocationChangedMessage(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}