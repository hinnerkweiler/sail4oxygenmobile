using System;
using CommunityToolkit.Mvvm.ComponentModel;


namespace sail4oxygen.ViewModels
{
	
	
	public partial class OnboardingPageVM : ObservableObject
	{
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

