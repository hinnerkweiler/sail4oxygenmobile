using System;
using CommunityToolkit.Mvvm.ComponentModel;


namespace sail4oxygen.ViewModels
{
	public partial class MainPageVM : ObservableObject
	{
        //Setter for MyLocation updates LocationText and LatitudeString/LongitudeString and triggers OnPropertyChanged for LatitudeEntry
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LocationText))]
        [NotifyPropertyChangedFor(nameof(Latitude))]
        [NotifyPropertyChangedFor(nameof(Longitude))]
        Location myLocation;
        
        public double Latitude
        {
            get
            {
                return MyLocation.Latitude;
            }
            set
            {
                MyLocation.Latitude = value;
                OnPropertyChanged(nameof(LatitudeString));
                OnPropertyChanged(nameof(LocationText));
            }
        }

        public double Longitude
        {
            get
            {
                return MyLocation.Longitude;
            }
            set
            {
                MyLocation.Longitude = value;
                OnPropertyChanged(nameof(LongitudeString));
                OnPropertyChanged(nameof(LocationText));
            }
        }


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
                return !isCoordinateViewVisible;
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
                if (Models.SharedData.FileUri != null && Models.SharedData.FileUri.AbsolutePath != "")
                {
                    return "Send to GEOMAR";
                }
                return "Select CSV File";
            }
        }




        public MainPageVM()
		{
            screen.X = DeviceDisplay.Current.MainDisplayInfo.Width;
            screen.Y = DeviceDisplay.Current.MainDisplayInfo.Height;
            
        }



        public FileResult HandleCsvFile(Uri fileUri)
        {
            Console.WriteLine("********Recived from Share in VM (uri): " + fileUri.AbsoluteUri);
            Console.WriteLine("********Recived from Share in VM (path): " + fileUri.AbsolutePath);

            //
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
            string body = "Hello Friends, \n here are our latest measurements from \n\n " + "Lat:  " + MyLocation.Latitude + "\nLong:  " + MyLocation.Longitude + "\nUTC  " + MyLocation.Timestamp.ToString("u");

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

            //Models.SharedData.Cleanup();

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

        public async Task<FileResult> SelectFile(PickOptions options)
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
                    Console.WriteLine("The user canceled or something went wrong: ", ex);
                }
            }
            else
            {
                result = HandleCsvFile(Models.SharedData.FileUri);
            }

            await AddLocationToCsv(Models.SharedData.FileUri);

            return result;
        }
        
        public async Task<bool> AddLocationToCsv(Uri file)
        {
            Console.WriteLine("******** IS FILE: "+ file.LocalPath);
            try
            {
                string csvText = await File.ReadAllTextAsync(file.LocalPath);
                string[] csvLines = csvText.Split('\n');
                string[] csvHeader = csvLines[0].Split(';');
                string[] csvValues = csvLines[1].Split(';');

                string newCsvText = csvLines[0] + ";Latitude;Longitude\n";

                for (int i = 1; i < csvLines.Length; i++)
                {
                    newCsvText += csvLines[i] + ";" + MyLocation.Latitude + ";" + MyLocation.Longitude + "\n";
                }

                await File.WriteAllTextAsync(file.LocalPath, newCsvText);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: ", ex);
            }

            return false;
        } 


    }
}

