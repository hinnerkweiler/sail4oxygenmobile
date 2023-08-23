using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;



namespace sail4oxygen.ViewModels
{
	public partial class MainPageVM : ObservableObject
	{
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LocationText))]
        Location myLocation;



        public bool CoordinatesValid
        {
            get
            {
                if (LatitudeIsValid && LongitudeIsValid)
                {
                    return true;
                }
                return false;
            }
        }



        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CoordinatesValid))]
        bool latitudeIsValid;



        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CoordinatesValid))]
        bool longitudeIsValid;



        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FileName))]
        [NotifyPropertyChangedFor(nameof(SendButtonText))]
        [NotifyPropertyChangedFor(nameof(FileRemoveButtonVisible))]
        
		FileResult csvFileToSend = null;

        

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCoordinateEditorVisible))]
        bool isCoordinateViewVisible = true;

        public string LearnMoreHeaderText
        {
            get
            {
                if (IsLearnMoreExpanded)
                {
                    return "&#x2304;  " + Resources.Languages.lang.MainPageIntro1;
                }
                else
                {
                    return "&gt;  " + Resources.Languages.lang.MainPageIntro1;

                }
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LearnMoreHeaderText))]
        bool isLearnMoreExpanded = false;

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


        public bool FileRemoveButtonVisible
        {
            get
            {
                if (CsvFileToSend == null)
                    return false;
                return true;
            }
        }
        


        [ObservableProperty]
        private Models.ScreenInfo screen = new();

        public string SendButtonText
        {
            get
            {
                if (CsvFileToSend == null)
                    return Resources.Languages.lang.SendButtonTextSelectFile;
                else
                    return Resources.Languages.lang.SendButtonTextSendFile;
            }
        }



        public string FileName
        {
            get
            {
                if (CsvFileToSend == null)
                    return Resources.Languages.lang.NoCsvFile;
                else
                    return CsvFileToSend.FileName;
            }
        }


        [ObservableProperty]
        private Models.NewsItems news = new Models.NewsItems();


        public MainPageVM()
		{
            if (Models.SharedData.StartFromShare)
            {
                HandleCsvFileShared(null, Models.SharedData.FileUri?.AbsolutePath);
            }
            else
            {
                Models.SharedData.SharedFileHandled += HandleCsvFileShared;
            }
        }


        public async void HandleCsvFileShared(object? sender, string filePath)
        {
            Models.SharedData.SharedFileHandled -= HandleCsvFileShared;
            
#if DEBUG
            Console.WriteLine("********Startet from Share ");
            Console.WriteLine("********Recived from Share (path): " + filePath);
#endif
            try
            {
                CsvFileToSend = new FileResult(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("File Error", $"Bummer! Shared Data could not be read.", "OK");
                Cleanup();
            }
            OnPropertyChanged(nameof(FileName));
            OnPropertyChanged(nameof(FileRemoveButtonVisible));
            
#if DEBUG
            Console.WriteLine("********Filename: "+FileName);
#endif
            Models.SharedData.SharedFileHandled += HandleCsvFileShared;

        }



        [CommunityToolkit.Mvvm.Input.RelayCommand]
        async void Appearing()
        {
            MyLocation = await GetLocation();
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand]
        public async void ItemTapped(Models.NewsItem item)
        {
            await Browser.Default.OpenAsync(item.Url);
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
            //toDo: Advise to use UI to enter coordinates 
            return null;
        }



        public async Task<bool> SelectFile(PickOptions options)
        {
            if (CoordinatesValid)
            {
                if (CsvFileToSend == null || CsvFileToSend.FileName == "")
                {
                    try
                    {
                        var file = await FilePicker.Default.PickAsync(filePickOptions);
                        if (file != null)
                        {
                            CsvFileToSend = file;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The user canceled or something went wrong: ", ex.Message);
                        await Application.Current.MainPage.DisplayAlert(Resources.Languages.lang.NoFileAlertTitle,  Resources.Languages.lang.NoFileAlertText + " " + ex.Message, Resources.Languages.lang.ok);
                    }
                }
                
                if (await Models.CSVHelper.AddLocation(CsvFileToSend.FullPath, MyLocation))
                {
                    await Email.Default.ComposeAsync(await Models.Mail.Send(MyLocation, CsvFileToSend.FullPath));

                    await Application.Current.MainPage.DisplayAlert(Resources.Languages.lang.ThankYou, Resources.Languages.lang.SendMessageAlertText, Resources.Languages.lang.ok);

                    Cleanup();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.Languages.lang.NoFileSent, Resources.Languages.lang.NoFileSentMessage + " " + Models.SharedData.LastError, Resources.Languages.lang.ok);
                }
                return true;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Nothing sent!", Resources.Languages.lang.LocationInvalidMessage, Resources.Languages.lang.ok);
            }
            return false;
        }



        public void Cleanup()
        {
            this.CsvFileToSend = null;
            Models.SharedData.FileUri = null;
            Models.SharedData.StartFromShare = false;
        }
    }
}

