using System;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;


namespace sail4oxygen.ViewModels
{ 
    public partial class OnboardingPageVM : ObservableObject
    {
        public string NameRegex = @"^[\wøæåØÆÅäöüÄÖÜ0-9\s\-+]*$";


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BoatnameValidationMessage))]
        bool nameIsValid;

        public string BoatnameValidationMessage
        {
            get
            {
                if (NameIsValid)
                {
                    return Resources.Languages.Lang.ok;
                }
                else
                {
                    return Resources.Languages.Lang.BoatNameInvalidMessage;
                }
            }
        }


        public bool BoatNameNotHidden
        {
            get => !Models.PreferencesHelper.BoatNameHidden;
        }

        private string boatNameCache;
        public string BoatName
        {
            get
            {
                return Models.PreferencesHelper.BoatName;
            }
                
            set
            {
            if (Regex.IsMatch(value, NameRegex))
                Models.PreferencesHelper.BoatName = value;
            }
        }

       
        public bool BoatNameHidden
        {
            get
            {
                return Models.PreferencesHelper.BoatNameHidden;
            }
            set
            {
                Models.PreferencesHelper.BoatNameHidden = value;
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

