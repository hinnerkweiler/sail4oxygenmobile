using System;
namespace sail4oxygen.Models
{
    
    //ToDo: Get bases directly from the Web-App endpoint at https://app.sail4oxygen.org/api/bases/geojson 
    //We do not need this at all 
    public class Port
    {
        public string name = "";
        public string city = "";
        public string country = "";
        public string locationDescription = "";
        public int status = 3;
        public double latitude = 0d;
        public double longitude = 0d;

        public Port()
        {
        }

        //Allow all parameters to be strings
        public Port(string name, string city, string latitude, string longitude, string country, string location, int status)
        {
            this.name = name;
            this.city = city;
            this.latitude = Double.Parse(latitude);
            this.longitude = Double.Parse(latitude);
            this.country = country;
            this.locationDescription = location;
            this.status = status;
        }

        //Allow coordinates as double
        public Port(string name, string city, Double latitude, Double longitude, string country, string location, int status)
        {
            this.name = name;
            this.city = city;
            this.latitude = latitude;
            this.longitude = longitude;
            this.country = country;
            this.locationDescription = location;
            this.status = status;
        }
    }
}

