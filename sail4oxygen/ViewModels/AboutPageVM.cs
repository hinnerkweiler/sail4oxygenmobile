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

		public AboutPageVM()
		{
			DeveloperText = "<p>Made 2023 with \u2665 for the oceans by Hinnerk Weiler.<br/>This Software is provided as is and released under MIT License. Code and License text are available on GitHub.<p>";
			AboutText = "<h1>Vestibulum nec convallis magna</h1>" +
				"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam eget pharetra lacus, ac dapibus est. " +
				"Mauris maximus purus ac tortor consequat, in pharetra arcu elementum. </p>" +
				"<p>Sed malesuada, velit a auctor lacinia, erat eros luctus lectus, sed condimentum odio felis sit amet ipsum. Sed eu eros vel est congue auctor eu eu justo. </p>" +
				"<p>Fusce suscipit, augue sed volutpat tempor, justo purus ullamcorper neque, vitae feugiat urna diam at nulla. Nullam ut justo id dui luctus eleifend. </p>" +
                "<h3>Vestibulum nec convallis magna</h3>" + "<p>Nullam dictum dapibus lectus, id convallis risus commodo at.</p>" +
                "<p>Nullam ut justo id dui luctus eleifend. Fusce suscipit, augue sed volutpat tempor, justo purus ullamcorper neque, vitae feugiat urna diam at nulla. </p>" +
                "<p>Fusce suscipit, augue sed volutpat tempor, justo purus ullamcorper neque, vitae feugiat urna diam at nulla. Nullam ut justo id dui luctus eleifend. </p>" +
                "<h3>Vestibulum nec convallis magna</h3>" + "<p>Nullam dictum dapibus lectus, id convallis risus commodo at.</p>";
        }
	}
}

