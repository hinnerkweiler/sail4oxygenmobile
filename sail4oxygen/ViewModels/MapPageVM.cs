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
		private Location userLocation = new();

		public double UserLat {
			get => UserLocation.Latitude;
			set
			{
				UserLocation.Latitude = value;
			}
		}
		public double UserLon {
			get => UserLocation.Longitude;
            set
            {
                UserLocation.Longitude = value;
            }
        }

		[ObservableProperty]
		ObservableCollection<MapMarker> portList = new();

		public MapPageVM()
		{
			_= Init();
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

