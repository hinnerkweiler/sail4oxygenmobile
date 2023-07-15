using System;
namespace sail4oxygen.Models
{
	public static class SharedData
	{
        public static string LastError;
        private static Uri fileUri;
        public static Uri FileUri
        {
            get => fileUri;

            set
            {
#if DEBUG
                Console.WriteLine("*****************Set Path"+value.ToString());
#endif
                fileUri = value;
            }
        }


        public static void FileUriFromString(string uriString)
        {
            try
            {
                var uri = new Uri(uriString);
                FileUri = uri;
            }
            catch (Exception e)
            {
                LastError = e.Message;
            }
        }
    }
    
}

