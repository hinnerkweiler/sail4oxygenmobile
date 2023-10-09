using System;
using Appwrite;
using Appwrite.Models;
using Appwrite.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sailing4oxygenApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController
	{

        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ILogger<LocationsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetPorts")]
        public async Task<List<Models.Port>> GetPorts()
        {
            List<string> errors = new();
            try
            {

                var client = new Client()
                    .SetEndpoint(Environment.GetEnvironmentVariable("APPWRITE_ENDPOINT_URL"))
                    .SetProject(Environment.GetEnvironmentVariable("APPWRITE_PROJECT_ID"))
                    .SetKey(Environment.GetEnvironmentVariable("APPWRITE_API_KEY"));


                var databases = new Databases(client);

                var databaseId = Environment.GetEnvironmentVariable("APPWRITE_DATABASE_ID");

                var collectionId = Environment.GetEnvironmentVariable("APPWRITE_PORT_COLLECTION_ID");

                var queryString = Models.Port.QueryAll();

                var documentList = await databases.ListDocuments(
                    databaseId: databaseId,
                    collectionId: collectionId,
                    queries: queryString);

                errors.Add("Ports: " + documentList.Documents.Count);

                var portList = new List<Models.Port>();

                foreach (var item in documentList.Documents)
                {
                    errors.Add("adding Port:" + item.Data["Name"].ToString());

                    var portItem = new Models.Port
                    {
                        Name = (string)item.Data["Name"],
                        City = (string)item.Data["City"],
                        CountryCode = (string)item.Data["Country"],
                        Latitude = (double)item.Data["Latitude"],
                        Longitude = (double)item.Data["Longitude"],
                        LocationDescription = (string)item.Data["Location"],
                    };

                    switch (item.Data["Status"])
                    {
                        case "Enabled": portItem.Status = Models.Port.PortStatus.Enabled; break;
                        case "Limited": portItem.Status = Models.Port.PortStatus.Limited; break;
                        case "Deleted": portItem.Status = Models.Port.PortStatus.Deleted; break;
                        default : portItem.Status = Models.Port.PortStatus.Disabled; break;
                    }

                    portList.Add(portItem);
                }

                return portList;
            }
            catch (Exception ex)
            {
                errors.Add("Something went terrible wrong: LocationList could not be fetched from database => ");
                errors.Add(ex.Message);
            }
            foreach (var item in errors)
            {
                Console.WriteLine("Error:" + item);
            }
            return null;
        }

    }
}

