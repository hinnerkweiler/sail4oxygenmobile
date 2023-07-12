using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

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
                     essentials.AddAppAction(new AppAction("id1", "Return Sonde", icon: "messages"));
                     essentials.AddAppAction(new AppAction("id2", "Tekeover Sonde", icon: "calendar"));
                     essentials.AddAppAction(new AppAction("id3", "App Info", icon: "information"));
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

