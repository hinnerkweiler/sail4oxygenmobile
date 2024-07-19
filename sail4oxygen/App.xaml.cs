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
		var locationService = LocationService.Instance;
	}

	protected override void OnResume()
	{
		base.OnResume();
        
    }

    }

