using System;
using Appwrite;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Sailing4oxygenApi.Models
{
	public class Port
	{
		public enum PortStatus
		{
			Enabled, Disabled, Limited, Deleted
		}

        public string Name { get; set; } = "";

        public double Latitude { get; set; } = 0;

        public double Longitude { get; set; } = 0;

        public PortStatus Status { get; set; } = PortStatus.Disabled;

        [JsonProperty("Location")]
        public string LocationDescription { get; set; } = "";

        public string City { get; set; } = "";

        [JsonProperty("Country")]
        public string CountryCode { get; set; } = "";

        public string Country {
			get
			{
				switch (CountryCode)
				{
					case "de" : return "Germany";
					case "dk" : return "Denmark";
					case "se" : return "Sweden";
					default: return "";
					
				}
			}
				}

        public Port()
        {

        }

        public Port(string name = "", double Latitude = 0, double Longitude = 0, PortStatus status = PortStatus.Disabled, string locationDescription = "", string city = "", string country = "")
        {
            this.Name = name;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.Status = status;
            this.LocationDescription = locationDescription;
            this.City = city;
            this.CountryCode = country;
        }


        public static List<string> QueryActive()
		{
			return BuildQuery();
		}

        public static List<string> QueryAll()
        {
            return new List<string>
            {
                Query.OrderAsc("Name")
            };
        }


        /// <summary>
        /// create a querylist for Ports
        /// </summary>
        /// <param name="deleted"></param>
        /// <param name="disabled"></param>
        /// <param name="limited"></param>
        /// <param name="enabled"></param>
        /// <param name="name"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        private static List<string> BuildQuery(bool deleted = false,bool disabled = false,bool limited = true,bool enabled = true,string name = "",string city = "")
		{
			var queryList = new List<string>
			{
				Query.OrderAsc("Name")
			};

			if (name != "")
            {
				queryList.Add(Query.StartsWith("Name", name)); 
            }

            if (city != "")
            {
                queryList.Add(Query.StartsWith("City", city));
            }


            if (!deleted) { queryList.Add(Query.NotEqual("Status", PortStatus.Deleted)); }
            if (!disabled) { queryList.Add(Query.NotEqual("Status", PortStatus.Disabled)); }
            if (!limited) { queryList.Add(Query.NotEqual("Status", PortStatus.Limited)); }
            if (enabled) { queryList.Add(Query.Equal("Status", PortStatus.Enabled)); }


			return queryList;
        }
    }

}

