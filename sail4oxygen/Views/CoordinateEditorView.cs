namespace sail4oxygen.Views;

public class CoordinateEditorView : ContentView
{
	public CoordinateEditorView()
	{
		//Display two input fields for coordinates and a button to save them. Than call a Function to save them.
		Content = new VerticalStackLayout
		{
			
			Children = {
				new HorizontalStackLayout
				{
					Children = {
						new Label { Text = "Latitude" },
						new Entry { Placeholder = "Latitude" }
					}
				},
				new HorizontalStackLayout
				{
					Children = {
						new Label { Text = "Longitude" },
						new Entry { Placeholder = "Longitude" }
					}
				},
				new Button
				{
					Text = "Save",
					Command = new Command(() => SaveCoordinates())
				}
			}
		};
	}
	//Update the coordinates in MainPage.MainPageVM
	private void SaveCoordinates()
	{
		//MainPage.MainPageVM.Latitude = LatitudeEntry.Text;
		//MainPage.MainPageVM.Longitude = LongitudeEntry.Text;
		
	}
}
