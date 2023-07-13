using System;
namespace sail4oxygen.Models
{
	public static class SharedData
	{
        private static Uri fileUri;
        public static Uri FileUri
        {
            get => fileUri;

            set
            {
                Console.WriteLine("*****************Set Path"+value.ToString());
                fileUri = value;
            }
        }


        public static void FileUriFromString(string uriString)
        {
            var uri = new Uri(uriString);
            FileUri = uri;
        }
    }
    
}

