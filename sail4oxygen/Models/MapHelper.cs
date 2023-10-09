using System;

using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Maps;

namespace sail4oxygen.Models
{
	
	public static class MapHelper
	{

        public static bool PortListUpdateDue
        {
            get
            {
                DateTime lastUpdate = Preferences.Get("PortListLastUpdated", DateTime.MinValue);
                TimeSpan span = DateTime.Now - lastUpdate;
                if (span.TotalDays > 14)    //update portlist after 14 days
                {
                    return true;
                }
                return false;
            }
        }

		public async static Task<ObservableCollection<Port>> GetPorts(Uri uri)
		{
            string jsonResult = "";
            ObservableCollection<Port> portList = new();

            try
            {
                //Download json from api and serialize into portList using HTTPClient 
                using (var client = new HttpClient())
                {
                    jsonResult = await client.GetStringAsync(uri);
                    portList = JsonConvert.DeserializeObject<ObservableCollection<Port>>(jsonResult);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //save portlist to appspace for later use in a local file

            return portList;
        }

        public async static Task<FileResult> UpdatePortListFromServer(Uri url, string name="portlist.json")
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, name);

            try
            {
                using (var client = new HttpClient
                        {
                            Timeout = TimeSpan.FromSeconds(5)
                        })
                {
                    using (var stream = await client.GetStreamAsync(url).ConfigureAwait(false))
                    {
                        using (var fileStream = File.Create(filePath))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }   
                }

                Preferences.Set("PortListLastUpdated", DateTime.Now);
#if DEBUG
                Console.WriteLine("********Download PortList from Server complete");
#endif
                return new FileResult(filePath, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not Download PortList: " + ex.Message);
            }
            return null;
        }

        public async static Task<ObservableCollection<Port>> GetPortsFromFile(string name="portlist.json")
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, name);
            if (!File.Exists(filePath))
            {
                //if file does not exist, or is older than n days, download new one from server
                FileResult fileResult = await UpdatePortListFromServer(new Uri("https://s4oapiserver.funkschiff.com/Locations"), name);
                if (fileResult == null)
                {
                    return null;
                }
            }
            
            ObservableCollection<Port> portList = new();

            try
            {
                using (var stream = File.OpenRead(filePath))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string json = await reader.ReadToEndAsync();
                        portList = JsonConvert.DeserializeObject<ObservableCollection<Port>>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not read PortList: " + ex.Message);
            }
            return portList;
        }

        //return a Marker List for syncfusion maps made from the portList returned from GetPortsFromFile()
        public async static Task<ObservableCollection<MapMarker>> GetMapMarkersFromPortList(string name="portlist.json")
        {
            ObservableCollection<MapMarker> markerList = new();
            ObservableCollection<Port> portList = await GetPortsFromFile(name);

            foreach (Port port in portList)
            {

                var markerItem = (new MapMarker()
                {
                    Latitude = port.latitude,
                    Longitude = port.longitude,
                    IconType = MapIconType.Circle
                });
                switch (port.status)
                {
                    case 0: markerItem.IconFill = new SolidColorBrush(Color.FromRgba(0, 128, 0, 0.9)); break;
                    case 2: markerItem.IconFill = new SolidColorBrush(Color.FromRgba(255, 165, 0, 0.9)); break;
                    default: markerItem.IconFill = new SolidColorBrush(Color.FromRgba(255, 0, 0, 0.9)); break;
                }

                markerList.Add(markerItem);
            }
            return markerList;
        }

          
	}
}

