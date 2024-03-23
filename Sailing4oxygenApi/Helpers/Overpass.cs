using System.Globalization;
using Newtonsoft.Json;

namespace Sailing4oxygenApi.Helpers
{
    public static class Overpass
    {
        private const double North = 55.552;
        private const double South = 53.873;
        private const double East = 12.149;
        private const double West = 9.417;

        private static readonly List<string> Tags = new List<string>
        {
            "leisure\"=\"marina",
            "landuse\"=\"harbour"
        };


        private static List<Models.MarinaElement> SortMarinas(List<Models.MarinaElement> marinas)
        {
            marinas = marinas.GroupBy(x => x.Tags.Name).Select(x => x.First()).ToList();
            return marinas.OrderBy(x => x.Tags.Name).ToList();
        }



        public static async Task<List<Models.MarinaElement>> GetMarinas()
        {
            var marinas = new List<Models.MarinaElement>();
            foreach (var tag in Tags)
            {
                var marina = await GetThingsByTag(South, West, North, East, tag);
                marinas.AddRange(marina);
            }

            marinas = SortMarinas(marinas);
            marinas = await AddCitiesToMarinas(marinas);

            return marinas;
        }


        private static async Task<List<Models.MarinaElement>> AddCitiesToMarinas(List<Models.MarinaElement> marinas)
        {
            foreach (var marina in marinas)
            {
                if (marina.Latitude == 0 || marina.Longitude == 0) continue;
                //throttle to not hammer the  API! max 1 requests per second and do not look suspicious
                int stopper = new Random().Next(1200, 4700);
                await Task.Delay(stopper);
                Console.Write($"Geocoding {marina.Latitude}, {marina.Longitude}");

                var geocodeList = await ReverseGeocodeCoordinates(marina.Latitude, marina.Longitude);
                marina.Town = geocodeList["town"];
                marina.Country = geocodeList["country"];
                marina.State = geocodeList["state"];
                marina.Municipality = geocodeList["municipality"];
                Console.Write($" - {marina.Town}, {marina.Country} =>");
                await AppwriteDatabase.StoreMarina(marina);
            }

            return marinas;
        }

        private static async Task<List<Models.MarinaElement>> GetThingsByTag(double minLat, double minLon,
            double maxLat, double maxLon, string tag)
        {
            try
            {
                var query =
                    $"[out:json];\n(\n  node[\"{tag}\"]({minLat.ToString(CultureInfo.InvariantCulture)},{minLon.ToString(CultureInfo.InvariantCulture)},{maxLat.ToString(CultureInfo.InvariantCulture)},{maxLon.ToString(CultureInfo.InvariantCulture)});\nway[\"{tag}\"]({minLat.ToString(CultureInfo.InvariantCulture)},{minLon.ToString(CultureInfo.InvariantCulture)},{maxLat.ToString(CultureInfo.InvariantCulture)},{maxLon.ToString(CultureInfo.InvariantCulture)});\nrelation[\"{tag}\"]({minLat.ToString(CultureInfo.InvariantCulture)},{minLon.ToString(CultureInfo.InvariantCulture)},{maxLat.ToString(CultureInfo.InvariantCulture)},{maxLon.ToString(CultureInfo.InvariantCulture)});\n);\nout center;";
                Console.WriteLine(query);
                var client = new HttpClient();
                var response =
                    await client.GetAsync(
                        $"https://overpass-api.de/api/interpreter?data={System.Net.WebUtility.UrlEncode(query)}");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();

                var rootObject = JsonConvert.DeserializeObject<Models.Marina>(jsonString);

                return rootObject.Elements ?? new List<Models.MarinaElement>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async Task<Dictionary<string, string>> ReverseGeocodeCoordinates(double lat, double lon)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("ToolBox");
                var response =
                    await client.GetAsync(
                        $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat.ToString(CultureInfo.InvariantCulture)}&lon={lon.ToString(CultureInfo.InvariantCulture)}&zoom=18&addressdetails=1");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                dynamic rootObject = JsonConvert.DeserializeObject(jsonString);
                var data = new Dictionary<string, string>();
                data.TryAdd("town",
                    (rootObject.address.city ?? rootObject.address.town ?? rootObject.address.village ??
                        rootObject.address.hamlet ?? rootObject.address.suburb ?? "_").ToString());
                data.TryAdd("country", (rootObject.address.country ?? "").ToString());
                data.TryAdd("state", (rootObject.address.state ?? "").ToString());
                data.TryAdd("municipality", (rootObject.address.municipality ?? "").ToString());
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}