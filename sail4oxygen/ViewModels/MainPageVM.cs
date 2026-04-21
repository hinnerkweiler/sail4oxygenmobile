using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using System.Threading.Tasks;



namespace sail4oxygen.ViewModels
{
	public partial class MainPageVM : ObservableObject
    {
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(LocationText))]
        private Location myLocation = new Location();

        private CancellationTokenSource gpsAutoUpdateCancellationTokenSource;
        private bool hasReceivedAutomaticGpsFix;
        private bool coordinatesEditedManually;
        
        public bool CoordinatesValid => LatitudeIsValid && LongitudeIsValid ? true : false;
        
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
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BoatnameValidationMessage))]
        bool nameIsValid;

        public string BoatnameValidationMessage =>
            NameIsValid
                ? Resources.Languages.lang.ok
                : Resources.Languages.lang.BoatNameInvalidMessage;

        private const string BoatNameRegex = @"^[\wøæåØÆÅäöüÄÖÜ0-9\s\-+]*$";

        public string BoatName
        {
            get => Models.PreferencesHelper.BoatName;
            set
            {
                if (value != null && System.Text.RegularExpressions.Regex.IsMatch(value, BoatNameRegex))
                    Models.PreferencesHelper.BoatName = value;
                OnPropertyChanged();
            }
        }

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



        public string LocationText =>
            MyLocation == null
                ? "–"
                : LatitudeString + " | " + LongitudeString +
                  (MyLocation.Timestamp != default ? "\n" + MyLocation.Timestamp.ToString("u") : "");




        PickOptions filePickOptions = new();



        public string LatitudeString =>
            MyLocation == null ? "–"
                : MyLocation.Latitude >= 0
                    ? Math.Abs(MyLocation.Latitude).ToString("00.0##° N")
                    : Math.Abs(MyLocation.Latitude).ToString("00.0##° S");



        public string LongitudeString =>
            MyLocation == null ? "–"
                : MyLocation.Longitude >= 0
                    ? Math.Abs(MyLocation.Longitude).ToString("00.0##° E")
                    : Math.Abs(MyLocation.Longitude).ToString("00.0##° W");


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


        public async void HandleCsvFileShared(object sender, string filePath)
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



        public void MarkCoordinatesEditedManually()
        {
            coordinatesEditedManually = true;
            StopAutoGpsRefresh();
        }

        public void StartAutoGpsRefresh()
        {
            if (coordinatesEditedManually || hasReceivedAutomaticGpsFix || gpsAutoUpdateCancellationTokenSource != null)
                return;

            gpsAutoUpdateCancellationTokenSource = new CancellationTokenSource();
            _ = AutoUpdateLocationUntilValidFixAsync(gpsAutoUpdateCancellationTokenSource.Token);
        }

        public void StopAutoGpsRefresh()
        {
            gpsAutoUpdateCancellationTokenSource?.Cancel();
            gpsAutoUpdateCancellationTokenSource?.Dispose();
            gpsAutoUpdateCancellationTokenSource = null;
        }

        public void ApplyGpsLocation(Location location, bool isAutomatic = false)
        {
            if (location == null || (isAutomatic && coordinatesEditedManually))
                return;

            MyLocation = location;
            LatitudeIsValid = true;
            LongitudeIsValid = true;

            if (isAutomatic)
            {
                hasReceivedAutomaticGpsFix = true;
                StopAutoGpsRefresh();
            }
        }

        private async Task AutoUpdateLocationUntilValidFixAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested && !coordinatesEditedManually && !hasReceivedAutomaticGpsFix)
                {
                    var location = await GetLocation();
                    if (IsValidAutomaticGpsFix(location))
                    {
                        ApplyGpsLocation(location!, isAutomatic: true);
                        return;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // expected when leaving the page or after manual edits
            }
            finally
            {
                gpsAutoUpdateCancellationTokenSource?.Dispose();
                gpsAutoUpdateCancellationTokenSource = null;
            }
        }

        private static bool IsValidAutomaticGpsFix(Location location)
        {
            if (location == null)
                return false;

            return location.Latitude != 0 || location.Longitude != 0;
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
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                Location location = await Geolocation.Default.GetLocationAsync(request);

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
                        await Application.Current.MainPage.DisplayAlert(
                            Resources.Languages.lang.NoFileAlertTitle,  
                            Resources.Languages.lang.NoFileAlertText + " " + ex.Message, 
                            Resources.Languages.lang.ok);
                    }
                }
                
                if (await Models.CSVHelper.AddLocation(CsvFileToSend.FullPath, MyLocation))
                {
                    await Email.Default.ComposeAsync(await Models.Mail.Send(MyLocation, CsvFileToSend.FullPath));

                    await Application.Current.MainPage.DisplayAlert(
                        Resources.Languages.lang.ThankYou, 
                        Resources.Languages.lang.SendMessageAlertText, 
                        Resources.Languages.lang.ok);

                    Cleanup();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Resources.Languages.lang.NoFileSent, 
                        Resources.Languages.lang.NoFileSentMessage + " " + Models.SharedData.LastError, 
                        Resources.Languages.lang.ok);
                }
                return true;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Nothing sent!", 
                    Resources.Languages.lang.LocationInvalidMessage, 
                    Resources.Languages.lang.ok);
            }
            return false;
        }
        
        public void RefreshBoatName()
        {
            OnPropertyChanged(nameof(BoatName));
        }
        
        public void Cleanup()
        {
            this.CsvFileToSend = null;
            Models.SharedData.FileUri = null;
            Models.SharedData.StartFromShare = false;
        }
    }
}

