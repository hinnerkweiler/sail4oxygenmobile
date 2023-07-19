namespace sail4oxygen.Models
{
    public static class DsgvoHandler
    {

        public static bool IsDsgvoAccepted
        {
            get 
            {
                if (Preferences.Default.Get("dsgvo","declined") == "accepted")
                {
                    return true;
                    
                }
                else
                {
                    return false;
                }
            }

            set 
            {
                if (value == true)
                {
                    Preferences.Default.Set("dsgvo", "accepted");
                }
                else
                {
                    Preferences.Default.Set("dsgvo", "declined");
                }
            }
        }
    }
}
 