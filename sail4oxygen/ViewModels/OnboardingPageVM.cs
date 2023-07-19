using System;
using CommunityToolkit.Mvvm.ComponentModel;


namespace sail4oxygen.ViewModels
{
	
	public partial class OnboardingPageVM : ObservableObject
	{
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

