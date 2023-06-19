using System;
using CommunityToolkit.Mvvm.ComponentModel;


namespace sail4oxygen.ViewModels
{
	public partial class MainPageVM : ObservableObject
	{
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LocationText))]
        Location myLocation;

		[ObservableProperty]
		FileResult theFileToSend;

        public string LocationText
        {
            get
            {
                return LatitudeString +" | "+LongitudeString + "\n" + MyLocation.Timestamp.ToString("u");
            }
        }

        PickOptions filePickOptions = new();

        public string LatitudeString
        {
            get
            {
                if (MyLocation.Latitude > 0)
                    return Math.Abs(MyLocation.Latitude).ToString("00.0##° N");
                else
                    return Math.Abs(MyLocation.Latitude).ToString("00.0##° S");
            }
        }

        public string LongitudeString
        {
            get
            {
                if (MyLocation.Longitude > 0)
                    return Math.Abs(MyLocation.Longitude).ToString("00.0##° E");
                else
                    return Math.Abs(MyLocation.Longitude).ToString("00.0##° W");
            }
        }


        [ObservableProperty]
        private Models.ScreenInfo screen = new();


        public MainPageVM()
		{
            screen.X = DeviceDisplay.Current.MainDisplayInfo.Width;
            screen.Y = DeviceDisplay.Current.MainDisplayInfo.Height;

        }


        public async Task<bool> SendEMail()
        {
            string subject = "Sailing for Oxygen";
            string body = "Hello friends! \n Here are our latest measurements. \n\n " + "Lat:" + MyLocation.Latitude + "\nLong:" + MyLocation.Longitude + "\nUTC" + MyLocation.Timestamp.ToString("u");
            string[] recipients = new[] { "h.weiler@trans-ocean.org", "kojefrei@gamil.com" };

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = new List<string>(recipients)
            };

            message.Attachments.Add(new EmailAttachment(TheFileToSend.FullPath));
            message.Attachments.Add(await Models.LocationMail.FromLocation(MyLocation));

            await Email.Default.ComposeAsync(message);

            return true;
        }


        public async Task<Location> GetLocation()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                    return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine(fnsEx);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Console.WriteLine(fneEx);
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine(pEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        public async Task<FileResult> SelectFile(PickOptions options)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(filePickOptions);
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("The user canceled or something went wrong: ", ex);
            }

            return null;
        }
    }
}

