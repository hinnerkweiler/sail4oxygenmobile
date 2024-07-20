using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Maps;
using CommunityToolkit.Mvvm.Messaging;
using sail4oxygen.Models;
using sail4oxygen.Services;

namespace sail4oxygen.ViewModels
{
	public partial class MapPageVM : ObservableObject
	{
		public double Latitude
		{
			get => LocationService.Instance.MyLocation.Latitude;
			set
			{
				if (LocationService.Instance.MyLocation.Latitude != value)
				{
					LocationService.Instance.MyLocation.Latitude = value;
					OnPropertyChanged(nameof(Latitude));
					OnPropertyChanged(nameof(CurrentLocation));
					WeakReferenceMessenger.Default.Send(new LocationPropertyChangedMessage(nameof(Latitude), value));
				}
			}
		}
		
		public double Longitude
		{
			get => LocationService.Instance.MyLocation.Longitude;
			set
			{
				if (LocationService.Instance.MyLocation.Longitude != value)
				{
					LocationService.Instance.MyLocation.Longitude = value;
					OnPropertyChanged(nameof(Longitude));
					OnPropertyChanged(nameof(CurrentLocation));
					WeakReferenceMessenger.Default.Send(new LocationPropertyChangedMessage(nameof(Longitude), value));
				}
			}
		}
		
		public Syncfusion.Maui.Maps.MapLatLng CurrentLocation
		{
			get => new Syncfusion.Maui.Maps.MapLatLng(Latitude, Longitude);
		}
		
        
		//The user's location when changed manually
		//[ObservableProperty]
		//[NotifyCanExecuteChangedFor(nameof(FireLocationChangedCommand))]
		private Coordinate _userLat = new() { Degrees = 54, Minutes = 30.5, Direction = 'N'};
		
        public Coordinate UserLat
        {
            get => _userLat;
            set
            {
                if (_userLat != value)
                {
                    _userLat = value;
                    OnPropertyChanged(nameof(UserLat));
                    UpdateLatitudeFromCoordinate();
                }
            }
        }
        
        private Coordinate _userLng = new() { Degrees = 9, Minutes = 30.5, Direction = 'E'};

        public Coordinate UserLng
        {
            get => _userLng;
            set
            {
	            if (_userLng != value)
	            {
		            _userLng = value;
		            OnPropertyChanged(nameof(UserLng));
		            UpdateLongitudeFromCoordinate();
	            }
            }
        }
    
        private void UpdateLatitudeFromCoordinate()
        {
            Latitude = _userLat.Degrees + (_userLat.Minutes / 60);
        }
	
        private void UpdateLongitudeFromCoordinate()
        {
            Longitude = _userLat.Degrees + (_userLat.Minutes / 60);
        }
		
		//Read and write BoatName directly to the Preferences
		public string MyBoatName
		{
			get => PreferencesHelper.BoatName;
			set => PreferencesHelper.BoatName = value;
		}

		[ObservableProperty]
		private ObservableCollection<MapMarker> _portList = new();

		public string SaveButtonText
		{
			get
			{
				if (LocationService.Instance.ManualLocation)
				{
					return Resources.Languages.Lang.UpdateCoordinates;
				}
				else
				{
					return Resources.Languages.Lang.SetCoordinates;
				}
			}
		}

        
		public MapPageVM()
		{
			
			
			var location = LocationService.Instance.MyLocation;
			if (location != null)
			{
				Latitude = location.Latitude;
				Longitude = location.Longitude;
			}
			//Set UserLat and UserLong to the current location
			UserLat.Degrees = (int)Latitude;
			UserLat.Minutes = (Latitude - UserLat.Degrees) * 60;
			UserLng.Degrees = (int)Longitude;
			UserLng.Minutes = (Longitude - UserLng.Degrees) * 60;
			
			_ = Init();
		}
		
		public void SaveLocation()
		{
			//Build the Latitude from the Degrees and Minutes in the UserLat Coordinates
			Latitude = UserLat.Degrees + UserLat.Minutes / 60;
			Longitude = UserLng.Degrees + UserLng.Minutes / 60;
			
			var newLocation = new Location(Latitude, Longitude);
			LocationService.Instance.MyLocation = newLocation;
			LocationService.Instance.ManualLocation = true;
		}
		
		private async Task Init()
		{
			PortList = await Models.MapHelper.GetMapMarkersFromPortList();
		}
	}
}

