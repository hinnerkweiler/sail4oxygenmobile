using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Sailing4oxygenApi.Controllers
{

    /// <summary>
    /// Calling the Overpass API to get marinas within the specified bounding box (Middelfahrt/Travemünde/Warnemünde/Beltbrücke)
    /// Returns a CSV file with the marinas GeoReverseCoded to get the town, country, state and municipality
    /// Store the marinas in the Appwrite Database for later reuse
    /// Do not call this endpoint too often! Better use  
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class MarinasController : Controller
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        
        [HttpGet("fromOverpass")]
        public async Task<IActionResult> GetFromOverpass()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                return Unauthorized("Authorization header is missing");
            }

            if (authorizationHeader != $"Bearer {Environment.GetEnvironmentVariable("LocalApiToken")}")
            {
                return Unauthorized("Authorization header is invalid");
            }

            var allMarinas = await Helpers.Overpass.GetMarinas();
            
            var csv = new StringBuilder();
            csv.AppendLine("\"Name\", \"Latitude\", \"Longitude\", \"Town\", \"Country\", \"State\", \"Municipality\", \"Email\", \"Phone\", \"Website\"");
            foreach (var marina in allMarinas)
            {
                var newLine =
                    $"\"{marina.Tags.Name}\", \"{marina.Latitude.ToString(CultureInfo.InvariantCulture)}\", \"{marina.Longitude.ToString(CultureInfo.InvariantCulture)}\", \"{marina.Town}\", \"{marina.Country}\", \"{marina.State}\", \"{marina.Municipality}\", \"{marina.Tags.email}\", \"{marina.Tags.phone}\", \"{marina.Tags.website}\"";
                csv.AppendLine(newLine);
            }
            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "marinas.csv");
        }

        [HttpGet("csv")]
        public async Task<IActionResult> GetFromAppwrite()
        {
            if (_cache.TryGetValue("marinas", out var marinasCsv))
            {
                return File(Encoding.UTF8.GetBytes(marinasCsv.ToString()), "text/csv", "marinas.csv");
            }
            var marinas = await Helpers.AppwriteDatabase.GetMarinas();
            var csv = new StringBuilder();
            csv.AppendLine(
                "\"Name\", \"Latitude\", \"Longitude\", \"Town\", \"Country\", \"State\", \"Municipality\", \"Email\", \"Phone\", \"Website\"");
            foreach (var marina in marinas)
            {
                var newLine =
                    $"\"{marina.Name}\", \"{marina.Latitude.ToString(CultureInfo.InvariantCulture)}\", \"{marina.Longitude.ToString(CultureInfo.InvariantCulture)}\", \"{marina.Town}\", \"{marina.Country}\", \"{marina.State}\", \"{marina.Municipality}\", \"{marina.Email}\", \"{marina.Phone}\", \"{marina.Website}\"";
                csv.AppendLine(newLine);
            }
            _cache.Set("marinas", csv, TimeSpan.FromHours(24));
            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "marinas.csv");
        }
    }
    
}