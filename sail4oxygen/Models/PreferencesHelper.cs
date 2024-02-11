using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace sail4oxygen.Models
{
	public static partial class PreferencesHelper  
	{
        public static bool BoatNameNotHidden 
        {
            get => !BoatNameHidden;
        }


        public static string BoatName
        {
            get
            {
                if (BoatNameNotHidden)
                {
                    return Preferences.Get("BoatName", "Anonym");
                }
                return "Anonym";
            }

            set
            {
                Preferences.Set("BoatName", value);
                WeakReferenceMessenger.Default.Send<Models.BoatNameChangeMessage>(new Models.BoatNameChangeMessage(value));
            }
        }


        public static bool BoatNameHidden
        {
            get
            {
                return Preferences.Get("BoatNameHidden", false);
            }
            set
            {
                Preferences.Set("BoatNameHidden", value);
            }
        }
    }
}

