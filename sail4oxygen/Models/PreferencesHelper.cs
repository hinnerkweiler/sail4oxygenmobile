using System;
using CommunityToolkit.Mvvm.ComponentModel;
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
                Preferences.Set("BoatNameLastChangedUtc", DateTimeOffset.UtcNow);
            }
        }
        
        public static bool ResetBoatNameIfExpired(int maxAgeDays = 4)
        {
            var lastChanged = Preferences.Get("BoatNameLastChangedUtc", DateTimeOffset.MinValue);
            if (lastChanged == DateTimeOffset.MinValue) return false;

            if (DateTimeOffset.UtcNow - lastChanged >= TimeSpan.FromDays(maxAgeDays))
            {
                Preferences.Remove("BoatName");
                Preferences.Remove("BoatNameLastChangedUtc");
                return true;
            }
            return false;
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

