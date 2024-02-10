using System;
using Appwrite;
using Appwrite.Services;

namespace Sailing4oxygenApi.Helpers
{
    public static class Envs
    {
        public static List<string> Messages = new();

        public static string Endpoint = Environment.GetEnvironmentVariable("APPWRITE_ENDPOINT_URL") ?? "undefined";
        public static string Project = Environment.GetEnvironmentVariable("APPWRITE_PROJECT_ID") ?? "undefined";
        public static string ApiKey = Environment.GetEnvironmentVariable("APPWRITE_API_KEY") ?? "undefined";
        public static string DatabaseId = Environment.GetEnvironmentVariable("APPWRITE_DATABASE_ID") ?? "undefined";
        public static string PortCollectionId = Environment.GetEnvironmentVariable("APPWRITE_PORT_COLLECTION_ID") ?? "undefined";
        public static string ReservationCollectionId = Environment.GetEnvironmentVariable("APPWRITE_RESERVATION_COLLECTION_ID") ?? "undefined";
        public static string UserCollectionId = Environment.GetEnvironmentVariable("APPWRITE_USERPROFILE_COLLECTION_ID") ?? "undefined";
        public static string NotesCollectionId = Environment.GetEnvironmentVariable("APPWRITE_NOTES_COLLECTION_ID") ?? "undefined";
        public static string SondesCollectionId = Environment.GetEnvironmentVariable("APPWRITE_SONDES_COLLECTION_ID") ?? "undefined";

        public static Dictionary<string,string> GetAll()
        {
            return new Dictionary<string, string>
            {
                { "Endpoint", Endpoint },
                { "Project", Project },
                { "ApiKey", ApiKey },
                { "DatabaseId",DatabaseId },
                { "PortCollectionId", PortCollectionId }
            };
        }

        public static void MessagesDump()
        {
            foreach (var item in Messages)
            {
                Console.WriteLine(item);
            }
        }
    }
}

