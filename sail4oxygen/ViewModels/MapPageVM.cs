using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace sail4oxygen.ViewModels
{
	public partial class MapPageVM : ObservableObject
	{
		[ObservableProperty]
		ObservableCollection<Models.Port> portList = new();

		public MapPageVM()
		{
			_= Init();
		}

		public async Task Init()
		{
			PortList = await Models.MapHelper.GetPortsFromFile();
#if DEBUG
			Console.WriteLine("Portlist updated:");
			foreach (var port in PortList)
			{
				Console.WriteLine(port.latitude + " " + port.longitude);
			}
#endif
		}
	}
}

