
using Appwrite;
using Newtonsoft.Json;

namespace Sailing4oxygenApi.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Marina
    {
        [JsonProperty("elements")] public List<MarinaElement> Elements { get; set; }
    }

    public class MarinaElement
    {
        [JsonProperty("tags")] public Tags Tags { get; set; }

        [JsonProperty("lat")] public double? _latitude { get; set; }

        [JsonProperty("lon")] public double? _longitude { get; set; }
        
        [JsonProperty("center")] public Coordinate? Center { get; set; }
        
        public string? Town { get; set; }
        public string? Municipality { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        
        public double Latitude
        {
            get
            {
                return _latitude ?? Center?.Latitude ?? 0;
            }
            set
            {
                this._latitude = this._latitude ?? new double();
                _latitude = value;
            }
        }
        
        public double Longitude
        {
            get
            {
                return _longitude ?? Center?.Longitude ?? 0;
            }
            set
            {
                this._longitude = this._longitude ?? new double();
                _longitude = value;
            }
        }

        public string Phone
        {
            get
            {
                return (Tags.email ?? Tags.email2 ?? "").ToString();
            }
            set
            {
                this.Tags = this.Tags ?? new Tags();
                Tags.phone = value;
            }
        }
        
        public string Email
        {
            get
            {
                return (Tags.phone ?? Tags.phone2 ?? "").ToString();
            }
            set
            {
                this.Tags = this.Tags ?? new Tags();
                Tags.email = value ?? "";
            }
        }
        
        public string Website
        {
            get
            {
                return (Tags.website ?? "").ToString();
            }
            set
            {
                this.Tags = this.Tags ?? new Tags();
                Tags.website = value ?? "";
            }
        }
        
        public string Name
        {
            get
            {
                return (Tags.Name ?? Town ?? Municipality ?? "**").ToString();
            }
            set
            {
                this.Tags = this.Tags ?? new Tags();
                Tags.Name = value.ToString() ?? "";
            }
        }
        
        public static List<string> QueryAll()
        {
            return new List<string>
            {
                Query.Limit(9999),
                Query.OrderAsc("country"),
                Query.OrderAsc("town"),
                Query.OrderAsc("name")
            };
        }
    }

    public class Tags
    {
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("contact:email")] public string? email { get; set; }
        [JsonProperty("contact:phone")] public string? phone { get; set; }
        [JsonProperty("website")] public string? website { get; set; }
        [JsonProperty("email")] public string? email2 { get; set; }
        [JsonProperty("phone")] public string? phone2 { get; set; }
    }
    
    public class Coordinate
    {
        [JsonProperty("lat")] public double Latitude { get; set; }
        [JsonProperty("lon")] public double Longitude { get; set; }
    }
}