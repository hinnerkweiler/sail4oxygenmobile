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
		FileResult csvFileToSend;



        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCoordinateEditorVisible))]
        bool isCoordinateViewVisible = true;



        public bool IsCoordinateEditorVisible
        {
            //opposite of IsCoordinateViewVisible
            get
            {
                return !IsCoordinateViewVisible;
            }
        }



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

        public string SendButtonText
        {
            get
            {
                return "Send CSV File to Geomar";
            }
        }




        public MainPageVM()
		{

        }



        public FileResult HandleCsvFile(Uri fileUri)
        {
#if DEBUG
            Console.WriteLine("********Recived from Share in VM (uri): " + fileUri.AbsoluteUri);
            Console.WriteLine("********Recived from Share in VM (path): " + fileUri.AbsolutePath);
#endif
            return new FileResult(Models.SharedData.FileUri.AbsolutePath);
        }



        [CommunityToolkit.Mvvm.Input.RelayCommand]
        async void Appearing()
        {
            MyLocation = await GetLocation();
        }



        public async Task<bool> SendEMail()
        {
            string subject = "Sailing for Oxygen";
            string body = "Hello Friends, \n here are our latest measurements from \n\n " + 
                                "Lat:  " + MyLocation.Latitude + 
                                "\nLong:  " + MyLocation.Longitude + 
                                "\nUTC  " + MyLocation.Timestamp.ToString("u");

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = new List<string>(Models.LocationMail.Recipients)
            };

            message.Attachments.Add(new EmailAttachment(CsvFileToSend.FullPath));
            message.Attachments.Add(await Models.LocationMail.FromLocation(MyLocation));

            await Email.Default.ComposeAsync(message);
            
            return true;
        }



        public async Task<Location> GetLocation()
        {
            try
            {
                Location location = await Geolocation.Default.GetLocationAsync();

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



        public async Task<bool> SelectFile(PickOptions options)
        {
            if (ValidateLatitude(MyLocation.Latitude.ToString()) && ValidateLongitude(MyLocation.Longitude.ToString()))
            {
                FileResult result = null;

                if (Models.SharedData.FileUri == null || Models.SharedData.FileUri.AbsolutePath == "")
                {
                    try
                    {
                        Uri fileUri = null;
                        var file = await FilePicker.Default.PickAsync(filePickOptions);
                        if (file != null)
                        {
                            fileUri = new Uri(file.FullPath);
                            Models.SharedData.FileUri = fileUri;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The user canceled or something went wrong: ", ex.Message);
                        await Application.Current.MainPage.DisplayAlert("Nothing sent!", $"Bummer! Can not select a File: {ex.Message}", "OK");
                    }
                }
                else
                {
                    result = HandleCsvFile(Models.SharedData.FileUri);
                }

                if (await Models.CSVHelper.AddLocation(Models.SharedData.FileUri, MyLocation))
                {
                    await SendEMail();
                    await Application.Current.MainPage.DisplayAlert("Data sent!", $"Thank you for participating. Your measurement will be available for scientists in a few seconds.", "OK");

                    return true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Nothing sent!", $"Bummer! Something went wrong: {Models.SharedData.LastError}", "OK");
                }

                return true;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Nothing sent!", $"Please enter valid Coordinates for Kiel Bight", "OK");
            }
            return false;
        }



        static public bool ValidateLatitude(string value)
        {
            if (double.TryParse(value, out double result))
            {
                if (result >= 54 && result <= 56)
                {
                    return true;
                }
            }
            return false;
        }



        static public bool ValidateLongitude(string value)
        {
            if (double.TryParse(value, out double result))
            {
                if (result >= 9 && result <= 11)
                {
                    return true;
                }
            }
            return false;
        }

    }
}

