using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using sail4oxygen.Models;
using sail4oxygen.Services;

namespace sail4oxygen.ViewModels
{
    
	public partial class MainPageVM : ObservableObject
	{
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LocationText))]
        [NotifyPropertyChangedFor(nameof(LatitudeString))]
        [NotifyPropertyChangedFor(nameof(LongitudeString))]
        private Location _myLocation;
        
       /// <summary>
       /// Returns status of the location service to enable/Disable the Send Button
       /// </summary>
       private bool HaveLocation
        {
            get
            {
                if ((_myLocation != null) && (_myLocation.Latitude != 0) && (_myLocation.Longitude != 0))
                {
                    return true;
                }
                return false;
            }
        }

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

        public string MyBoatName
        {
            get
            {
                string name=PreferencesHelper.BoatName;
                //if name is longer than 20 characters, cut it and add …
                if (name.Length > 20)
                {
                    name = name.Substring(0, 20) + "…";
                }

                return name;
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CoordinatesValid))]
        bool latitudeIsValid =true;
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CoordinatesValid))]
        bool longitudeIsValid=true;
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FileName))]
        [NotifyPropertyChangedFor(nameof(SendButtonText))]
        [NotifyPropertyChangedFor(nameof(FileRemoveButtonVisible))]
        
		FileResult csvFileToSend;
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCoordinateEditorVisible))]
        bool isCoordinateViewVisible = true;

        public string LearnMoreHeaderText
        {
            get
            {
                if (IsLearnMoreExpanded)
                {
                    return "&#x2304;  " + Resources.Languages.Lang.MainPageIntro1;
                }
                else
                {
                    return "&gt;  " + Resources.Languages.Lang.MainPageIntro1;

                }
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LearnMoreHeaderText))]
        bool isLearnMoreExpanded;

        public bool IsCoordinateEditorVisible => !IsCoordinateViewVisible;
           
        public string LocationText => LatitudeString +" | "+LongitudeString + "\n" + MyLocation.Timestamp.ToString("u");
        
        PickOptions _filePickOptions = new();
        
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
                    return Resources.Languages.Lang.SendButtonTextSelectFile;
                else
                    return Resources.Languages.Lang.SendButtonTextSendFile;
            }
        }

        public string FileName
        {
            get
            {
                if (CsvFileToSend == null)
                    return Resources.Languages.Lang.NoCsvFile;
                else
                    return CsvFileToSend.FileName;
            }
        }
        
        [ObservableProperty]
        private Models.NewsItems news = new Models.NewsItems();

        
        
        public MainPageVM()
		{
            WeakReferenceMessenger.Default.Register<LocationPropertyChangedMessage>(this, (r, m) =>
            {
                    OnPropertyChanged(nameof(MyLocation));
            });
            
            WeakReferenceMessenger.Default.Register<BoatNameChangeMessage>(this, (r, m) =>
            {
                    OnPropertyChanged(nameof(MyBoatName));
            });

            
            LocationService.Instance.OnLocationChanged += (s, e) =>
            {
                MyLocation = LocationService.Instance.MyLocation;
            };

            MyLocation = LocationService.Instance.MyLocation;

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
        private async void Appearing()
        {
           LocationService.Instance.GetLocation();
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand]
        public async void ItemTapped(Models.NewsItem item)
        {
            await Browser.Default.OpenAsync(item.Url);
        }

        public async Task<bool> SelectFile(PickOptions options)
        {
            if (CoordinatesValid)
            {
                if (CsvFileToSend == null || CsvFileToSend.FileName == "")
                {
                    try
                    {
                        var file = await FilePicker.Default.PickAsync(_filePickOptions);
                        if (file != null)
                        {
                            CsvFileToSend = file;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The user canceled or something went wrong: ", ex.Message);
                        await Application.Current.MainPage.DisplayAlert(Resources.Languages.Lang.NoFileAlertTitle,  Resources.Languages.Lang.NoFileAlertText + " " + ex.Message, Resources.Languages.Lang.ok);
                    }
                }
                
                if (await Models.CSVHelper.AddLocation(CsvFileToSend.FullPath, MyLocation))
                {
                    await Email.Default.ComposeAsync(await Models.Mail.Send(MyLocation, CsvFileToSend.FullPath));

                    await Application.Current.MainPage.DisplayAlert(Resources.Languages.Lang.ThankYou, Resources.Languages.Lang.SendMessageAlertText, Resources.Languages.Lang.ok);

                    Cleanup();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.Languages.Lang.NoFileSent, Resources.Languages.Lang.NoFileSentMessage + " " + Models.SharedData.LastError, Resources.Languages.Lang.ok);
                }
                return true;
            }
            return false;
        }


        public void Cleanup()
        {
            this.CsvFileToSend = null!;
            Models.SharedData.FileUri = null;
            Models.SharedData.StartFromShare = false;
        }
    }
}

