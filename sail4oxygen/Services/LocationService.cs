using CommunityToolkit.Mvvm.ComponentModel;

namespace sail4oxygen.Services;


public partial class LocationService : ObservableObject
{
    [ObservableProperty]
    private bool manualLocation = false;
    
    private static readonly Lazy<LocationService> lazy = new Lazy<LocationService>(() => new LocationService());

    public static LocationService Instance { get { return lazy.Value; } }

    private LocationService() { }
    
    private Location _myLocation;
    public Location MyLocation
    {
        get => _myLocation;
        set
        {
            _myLocation = value;
            OnLocationChanged?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("Location changed:" + _myLocation.Latitude + " " + _myLocation.Longitude);
        }
    }
    
    public void UpdateLocation(Location location)
    {
        MyLocation = location;
    }
    
    public async Task<Location> GetLocation()
    {
        if (ManualLocation)
        {
            return MyLocation;
        }
        
        try
        {
            Location location = await Geolocation.Default.GetLocationAsync();

            if (location != null)
            {
                MyLocation = location;
                return location;
            }
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            Console.WriteLine(fnsEx);
        }
        catch (FeatureNotEnabledException fneEx)
        {
            Console.WriteLine(fneEx);
        }
        catch (PermissionException pEx)
        {
            Console.WriteLine(pEx);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        //toDo: Advise to use UI to enter coordinates 
        return null;
    }

    public event EventHandler OnLocationChanged;
}
