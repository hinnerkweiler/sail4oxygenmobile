using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Maps;
using CommunityToolkit.Mvvm.Messaging;
using sail4oxygen.Models;

namespace sail4oxygen.ViewModels
{
	public partial class MapPageVM : ObservableObject
	{
		//The user's location when changed manually
		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(FireLocationChangedCommand))]
		private Coordinate _userLat = new() { Degrees = 54, Minutes = 30.5, Direction = 'N'};
		
		
		[ObservableProperty] 
		[NotifyCanExecuteChangedFor(nameof(FireLocationChangedCommand))]
		private Coordinate _userLong = new() { Degrees = 9, Minutes = 30.5, Direction = 'E'};

		//Read and write BoatName directly to the Preferences
		public string MyBoatName
		{
			get => PreferencesHelper.BoatName;
			set => PreferencesHelper.BoatName = value;
		}

		[ObservableProperty]
		private ObservableCollection<MapMarker> _portList = new();

		public MapPageVM()
		{
			_ = Init();
		}

		//Release a Message whenever the user manually alters the location.
		partial void OnUserLongChanged(Coordinate value)
		{
			OnUserLatChanged(value);
		}
		
		partial void OnUserLatChanged(Coordinate value)
		{
			double latitude = UserLat.ToDouble();
			double longitude = UserLong.ToDouble();
			WeakReferenceMessenger.Default.Send(new UserLocationChangedMessage(latitude, longitude));
		}

		[RelayCommand]
		private void FireLocationChanged()
		{
			OnUserLatChanged(UserLat);
		}

		private async Task Init()
		{
			PortList = await Models.MapHelper.GetMapMarkersFromPortList();
		}
	}
}

