using System;


namespace sail4oxygen.Models
{
	public static class LocationMail
	{
		public static async Task<EmailAttachment> FromLocation(Location location)
		{
            var targetFileName = "location" + location.Timestamp.Ticks.ToString()+".txt";
            string targetFile = System.IO.Path.Combine(FileSystem.Current.CacheDirectory, targetFileName);


            using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
            using StreamWriter streamWriter = new StreamWriter(outputStream);

            await streamWriter.WriteAsync(location.Latitude.ToString()+","+location.Latitude.ToString()+","+location.Timestamp.ToString("u"));

            return new EmailAttachment(targetFile);
            
        }
    }
}

