
namespace sail4oxygen.Models
{
    public static class Mail
    {
        private static string[] _recipients = new[] { PrivatData.RecipientList };

        private static async Task<EmailAttachment> CreateLocationAttachment(Location location)
	{
        var targetFileName = "location" + location.Timestamp.Ticks.ToString()+".txt";
        try
        {
            var targetFile = System.IO.Path.Combine(FileSystem.Current.CacheDirectory, targetFileName);
            using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
            using StreamWriter streamWriter = new StreamWriter(outputStream);

            await streamWriter.WriteAsync(location.Latitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + location.Longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + location.Timestamp.ToString("u"));

            return new EmailAttachment(targetFile);
        }
        catch (Exception ex)
        {
            SharedData.LastError = ex.Message;
            Console.WriteLine(ex.Message);
            return null;
        }
    }



        public static async Task<EmailMessage> Send(Location location, string fileToSendPath)
        {
            string subject = "Sailing for Oxygen";
            string body = "Hello Friends, \n here are our latest measurements from \n\n " +
                                "Lat:  " + location.Latitude +
                                "\nLong:  " + location.Longitude +
                                "\nUTC  " + location.Timestamp.ToString("u");

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = [.._recipients]
            };

            message.Attachments.Add(new EmailAttachment(fileToSendPath));
            message.Attachments.Add(await CreateLocationAttachment(location));

            return message;
        }
    }
}

