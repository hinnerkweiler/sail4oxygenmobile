using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Messaging;

namespace sail4oxygen.ViewModels
{
    public partial class MapViewVM : ObservableObject
    {
        
        private readonly MainPageVM pageVM;

        [ObservableProperty]
        Location userLocation;

        public MapViewVM()
        {
            
        }

        private void OnLocationChangeMessage(Location location)
        {
            UserLocation.Latitude = location.Latitude;
            UserLocation.Longitude = location.Longitude;
#if DEBUG
            Console.WriteLine("*************** Location Change Recived in MapViewVM");
#endif
        }

    }
}

