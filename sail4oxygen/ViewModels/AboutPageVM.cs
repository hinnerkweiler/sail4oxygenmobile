using System;
using CommunityToolkit.Mvvm.ComponentModel;


namespace sail4oxygen.ViewModels
{
	public partial class AboutPageVM : ObservableObject
	{
		[ObservableProperty]
		public string developerText;

		[ObservableProperty]
		public string aboutText;

        [ObservableProperty]
        public string privacyText;

        public AboutPageVM()
		{
			DeveloperText = Resources.Languages.Lang.DeveloperNote;
			AboutText = Resources.Languages.Lang.AboutText;
			PrivacyText=Resources.Languages.Lang.PrivacyText;
        }
	}
}

