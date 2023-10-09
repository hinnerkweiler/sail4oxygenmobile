using System;
namespace sail4oxygen.Models
{
    public class Port
    {
        public string name = "";
        public string country = "";
        public string location = "";
        public double latitude = 0d;
        public double longitude = 0d;
        public bool disabled = false;

        public Port()
        {
        }

        //Allow all parameters to be strings
        public Port(string name, string latitude, string longitude, string country, string location, string disabled)
        {
            this.name = name;
            this.latitude = Double.Parse(latitude);
            this.longitude = Double.Parse(latitude);
            this.country = country;
            this.location = location;
            this.disabled = disabled == "true" ? true : false;
        }

        //Allow coordinates as double
        public Port(string name, Double latitude, Double longitude, string country, string location, string disabled)
        {
            this.name = name;
            this.latitude = latitude;
            this.longitude = latitude;
            this.country = country;
            this.location = location;
            this.disabled = disabled == "true" ? true : false;
        }
    }
}

