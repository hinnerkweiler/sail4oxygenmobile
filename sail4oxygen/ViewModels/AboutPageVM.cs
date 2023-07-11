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
			DeveloperText = "<p>Made 2023 with \u2665 for the oceans by Hinnerk Weiler.<br />&lt;service@hinnerk-weiler.com&gt;<br />This Software is provided as is with no warranties and released under MIT License. Source code and full license text are available on the documentation page.<p>";

			AboutText = "<h1>Be Part of the Research</h1>" +
				$"<p style=\"margin:5px;\"><em>Citizen Science: Sailing for Oxygen</em> is a joint research project between the international renown german research center GEOMAR and Trans-Ocean Sailing Club. This compagnion App aims to assist you in becomming part of the research and keep you updated about the project. </p>" +
				$"<p style=\"margin:5px;\">For more information and contact details you can visit the project Website at <strong>sail4oxygen.org</strong>. Technical support regarding this App is available using the contact details towards the end of this screen.</p>";

			PrivacyText=$"<pstyle=\"margin:5px;\"><strong>Privacy Information:</strong> Transmitting a measurement reveals your location at the time of sample and the E-Mail-Address configured on your Phone to GEOMAR servers located in Germany. Registering as a participant, making a sonde reservation or confirming a sonde transmits information required to handle the given purpose. Only Information provided by you will be transmitted.";
        }
	}
}

