using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.Maps;
using CommunityToolkit.Mvvm.Messaging;
using sail4oxygen.Models;

namespace sail4oxygen.ViewModels
{
	public partial class MapPageVM : ObservableObject
	{
		//The user's location when changed manually
		[ObservableProperty] private Coordinate _userLat = new();

		[ObservableProperty] private Coordinate _userLong = new();

		//Read and write BoatName directly to the Preferences
		public string MyBoatName
		{
			get => PreferencesHelper.BoatName;
			set => PreferencesHelper.BoatName = value;
		}

		[ObservableProperty] private ObservableCollection<MapMarker> _portList = new();

		public MapPageVM()
		{
			_ = Init();
			//We are in the northern hemisphere and east of Greenwich at all times!
			UserLat.Direction = 'N';
			UserLong.Direction = 'E';
		}

		//Release a Message whenever the user manually alters the location.
		partial void OnUserLongChanged(Coordinate newValue)
		{
			OnUserLatChanged(newValue);
		}

		partial void OnUserLatChanged(Coordinate newValue)
		{
			double latitude = newValue.ToDouble();
			double longitude = newValue.ToDouble();
			WeakReferenceMessenger.Default.Send(new UserLocationChangedMessage(latitude, longitude));
		}

		private async Task Init()
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
	}
}

