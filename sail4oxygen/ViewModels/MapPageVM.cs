using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.Maps;

namespace sail4oxygen.ViewModels
{
	public partial class MapPageVM : ObservableObject
	{
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
	}
}

