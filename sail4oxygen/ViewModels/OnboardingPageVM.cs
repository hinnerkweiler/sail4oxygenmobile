using System;
using CommunityToolkit.Mvvm.ComponentModel;


namespace sail4oxygen.ViewModels
{


    public partial class OnboardingPageVM : ObservableObject
    {
        public bool BoatNameNotHidden
        {
            get => !BoatNameHidden;
        }


        public string BoatName
        {
            get
            {
                return Preferences.Get("BoatName", "Anonym");
            }
            set
            {
                //ToDo: Sanitizing??
                Preferences.Set("BoatName", value);
                OnPropertyChanged(nameof(BoatName));
            }
        }


        public bool BoatNameHidden
        {
            get
            {
                return Preferences.Get("BoatNameHidden", false);
            }
            set
            {
                Preferences.Set("BoatNameHidden", value);
                OnPropertyChanged(nameof(BoatNameHidden));
                OnPropertyChanged(nameof(BoatName));
            }
        }



        public string ManualDate
        {
            get
            {
                DateTime value = Preferences.Get("ManualDownloaded", DateTime.MinValue);
                return "Manual " + value.ToShortDateString();
            }
        }

        public string VersionInfo
        {
            get
            {
                return "Version " + VersionTracking.CurrentVersion;
            }
        }


        public bool DsgvoBoxChecked
        {
            get => Models.DsgvoHandler.IsDsgvoAccepted;
            set
            {
                Models.DsgvoHandler.IsDsgvoAccepted = value;
            }
        }

        public OnboardingPageVM()
        {

        }
    }
}

