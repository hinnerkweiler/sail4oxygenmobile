using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.Maps;
using CommunityToolkit.Mvvm.Messaging;

namespace sail4oxygen.ViewModels
{
	public partial class MapPageVM : ObservableObject
	{
		[ObservableProperty]
		Models.Coordinate userLat =  new();
		
		[ObservableProperty]
		Models.Coordinate userLong = new();

		
		[ObservableProperty]
		private Location userLocation = new();

		
		
		public string MyBoatName
		{
			get => Models.PreferencesHelper.BoatName;
			set
			{
				Models.PreferencesHelper.BoatName = value; 
			}
		}

		[ObservableProperty]
		ObservableCollection<MapMarker> portList = new();

		public MapPageVM()
		{
			_= Init();
			UserLat.Direction = 'N';
			UserLong.Direction = 'E';
        }

		public async Task Init()
		{
			PortList = await Models.MapHelper.GetMapMarkersFromPortList();
#if DEBUG
			Console.WriteLine("Portlist updated:");
			foreach (var port in PortList)
			{
				Console.WriteLine(port.Latitude + " " + port.Longitude);
			}
#endif
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

