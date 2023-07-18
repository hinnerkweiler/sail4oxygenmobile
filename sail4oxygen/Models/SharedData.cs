using System;


namespace sail4oxygen.Models
{
	public static class SharedData
	{

        public static event EventHandler<string> SharedFileHandled;

        public static string LastError;
        private static bool startFromShare;
        public static bool StartFromShare
        {
            get => startFromShare;

            set 
            {
#if DEBUG
                Console.WriteLine("*****************Set StartFromShare:"+value.ToString());
#endif
            }
        }


        private static Uri fileUri;
        public static Uri FileUri
        {
            get => fileUri;

            set
            {
                if (value == null)
                {
                    fileUri = new Uri("content:");
#if DEBUG
                    Console.WriteLine("*****************File URI is 'null'");
#endif
                }
                else
                {
#if DEBUG
                    Console.WriteLine("*****************Set FileUri to " + value.AbsolutePath);
#endif
                    fileUri = value;
                    
                    Models.SharedData.SharedFileHandled?.Invoke(typeof(SharedData), value.AbsolutePath);


                }
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

