using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;



namespace sail4oxygen;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"*/*", Label = "Send to GEOMAR")]

public class MainActivity : MauiAppCompatActivity
{
    public static string CsvFromShare { get; set; }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var intent = Intent;
        var action = intent.Action;
        var type = intent.Type;

    
        //store the intent data in a static variable
        if (Intent.ActionSend.Equals(action) && type != null)
        {
            if ("text/comma-separated-values".Equals(type))
            {   
                var uri = (Android.Net.Uri)intent.GetParcelableExtra(Intent.ExtraStream);

#if DEBUG
                System.Console.WriteLine("CSV Recived ********************************************************" + uri);
#endif
                var appFilePath = AndroidHelpers.UriResolver.CopyFileFromUriToAppSpace(this, uri);

                if (!string.IsNullOrEmpty(appFilePath.LocalPath))
                {
                    // Use the app file path as needed
                    Console.WriteLine($"App file path: {appFilePath}");
                }

                Models.SharedData.FileUri = appFilePath;
            }

        }
        
    }

    protected override void OnResume()
    {
        base.OnResume();

        Platform.OnResume(this);
    }

    protected override void OnNewIntent(Android.Content.Intent intent)
    {
        base.OnNewIntent(intent);

        Platform.OnNewIntent(intent);
    }
    
}

