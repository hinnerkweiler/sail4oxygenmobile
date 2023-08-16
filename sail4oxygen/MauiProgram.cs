using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;

namespace sail4oxygen;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("LucidaGrande.ttf", "LucidaGrande");
            })
            .ConfigureEssentials(essentials =>
            {
                     essentials.AddAppAction(new AppAction("website", "Go to sail4oxygen.org", icon: "messages"));
                     essentials.AddAppAction(new AppAction("id2", "Tekeover Sonde", icon: "calendar"));
                     essentials.AddAppAction(new AppAction("id3", "App Info", icon: "information"));
            });
		builder.ConfigureSyncfusionCore();
#if DEBUG
        builder.Logging.AddDebug();
#endif
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Models.PrivatData.SyncfusionKey);
        return builder.Build();
	}
}

