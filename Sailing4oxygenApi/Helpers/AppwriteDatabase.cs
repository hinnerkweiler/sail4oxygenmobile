using Appwrite;
using Appwrite.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sailing4oxygenApi.Models;

namespace Sailing4oxygenApi.Helpers;


public class AppwriteDatabase
{
    private static Appwrite.Client client = new Client()
        .SetEndpoint(Environment.GetEnvironmentVariable("APPWRITE_ENDPOINT_URL"))
        .SetProject(Environment.GetEnvironmentVariable("APPWRITE_PROJECT_ID"))
        .SetKey(Environment.GetEnvironmentVariable("APPWRITE_API_KEY"));

    private static Databases databases = new Databases(client);

    public static async Task StoreMarina(Models.MarinaElement marina)
    {
        //Stupid Appwrite-SDK translates the first letter of the keys to lowercase!
        var payload = new Dictionary<string, object>
        {
            { "Name", marina.Name.ToString() },
            { "Latitude", marina.Latitude },
            { "Longitude", marina.Longitude },
            { "Town", marina.Town.ToString() },
            { "Country", marina.Country.ToString() },
            { "State", marina.State.ToString() },
            { "Municipality", marina.Municipality.ToString() },
            { "Email", marina.Email.ToString() },
            { "Phone", marina.Phone.ToString() },
            { "Website", marina.Website.ToString() }
        };

        try
        {
            var response = await databases.CreateDocument(
                databaseId: Environment.GetEnvironmentVariable("APPWRITE_DATABASE_ID"),
                collectionId: Environment.GetEnvironmentVariable("APPWRITE_MARINAS_COLLECTION_ID"),
                documentId: ID.Unique(),
                data: payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
public static async Task<List<Models.MarinaElement>> GetMarinas()
    {
        //Get all marinas from the Appwrite database
        var queryString = Models.MarinaElement.QueryAll();

        var documentList = await databases.ListDocuments(
            databaseId: Environment.GetEnvironmentVariable("APPWRITE_DATABASE_ID"),
            collectionId: Environment.GetEnvironmentVariable("APPWRITE_MARINAS_COLLECTION_ID"),
            queries: queryString);

        var marinas = new List<Models.MarinaElement>();

        foreach (var item in documentList.Documents)
        {
            if (item.Data == null) continue;
            if(item.Data["name"] == null) continue;
            
            var marina = new Models.MarinaElement
            {
                Name = (string)item.Data["name"],
                Latitude = (double)item.Data["latitude"],
                Longitude = (double)item.Data["longitude"],
                Town = (string)item.Data["town"],
                Country = (string)item.Data["country"],
                State = (string)item.Data["state"],
                Municipality = (string)item.Data["municipality"],
                Email = (string)item.Data["email"],
                Phone = (string)item.Data["phone"],
                Website = (string)item.Data["website"]
            };
            marinas.Add(marina);
        }

        return marinas;
    }
    
    
}