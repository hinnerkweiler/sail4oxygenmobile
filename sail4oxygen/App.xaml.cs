using sail4oxygen.Services;

namespace sail4oxygen;

public partial class App : Application
{
    

    public App()
	{
		InitializeComponent();
	// Access the singleton instance to ensure it is initialized early
		var locationService = LocationService.Instance;

		MainPage = new AppShell();
		
	}

	protected override void OnStart()
	{
		base.OnStart();
		LocationUpdate();
	}

	protected override void OnResume()
	{
		base.OnResume();
        LocationUpdate();
    }

	private void LocationUpdate()
	{
		try
		{
			var locationService = LocationService.Instance;
			if (locationService == null)
			{
				throw new InvalidOperationException("LocationService instance is null.");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error during OnStart: {ex.Message}");
		}
	}

}


