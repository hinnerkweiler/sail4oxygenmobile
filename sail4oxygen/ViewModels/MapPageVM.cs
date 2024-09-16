using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
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
			get => LocationService.Instance.MyLocation?.Latitude ?? 54.322;
			set
			{
				if (Math.Abs(LocationService.Instance.MyLocation.Latitude - value) > 0.01f)
				{
					LocationService.Instance.MyLocation.Latitude = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(CurrentLocation));
					WeakReferenceMessenger.Default.Send(new LocationPropertyChangedMessage(nameof(Latitude), value));
				}
			}
		}
		
		public double Longitude
		{
			get => LocationService.Instance.MyLocation?.Longitude ?? 10.135;
			set
			{
				if (Math.Abs(LocationService.Instance.MyLocation.Longitude - value) > 0.01f)
				{
					LocationService.Instance.MyLocation.Longitude = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(CurrentLocation));
					WeakReferenceMessenger.Default.Send(new LocationPropertyChangedMessage(nameof(Longitude), value));
				}
			}
		}
		
		public MapLatLng CurrentLocation => new (Latitude, Longitude);


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
                    OnPropertyChanged();
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
		            OnPropertyChanged();
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
			try
			{
				var location = LocationService.Instance.MyLocation;
				if (location != null)
				{
					Latitude = location.Latitude;
					Longitude = location.Longitude;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("GPS not ready: "+e.Message);
				//The location is not jet available and we do not wait for it, set Map Center to Lighthouse Kiel
				Latitude = 54.322;
				Longitude = 10.135;
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
			
			LocationService.Instance.ManualLocation = true;
		}
		
		private async Task Init()
		{
			PortList = await MapHelper.GetMapMarkersFromPortList();
		}
	}
}

