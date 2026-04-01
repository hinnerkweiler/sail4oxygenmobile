namespace sail4oxygen;

public partial class App : Application
{
    

    public App()
	{
		InitializeComponent();
		UserAppTheme = AppTheme.Dark;
		MainPage = new AppShell();
	}

	protected override void OnStart()
	{
		base.OnStart();
			
	}

	protected override void OnResume()
	{
		base.OnResume();
        
    }

    }

