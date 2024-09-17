using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;



namespace sail4oxygen;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"*/*", Label = "GEOMAR")]

public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
#if DEBUG
        System.Console.WriteLine("S4O *!*!*!*!*!*!*!*!*!*!*!*!*! ON CREATE");
#endif
        //IntentWorker();
        
    }

    protected override void OnResume()
    {
        base.OnResume();
#if DEBUG
        System.Console.WriteLine("S4O *!*!*!*!*!*!*!*!*!*!*!*!*! ON RESUME");
#endif
        IntentWorker(Intent);  // Use the current intent

        Platform.OnResume(this);
    }

    protected override void OnNewIntent(Android.Content.Intent intent)
    {
        base.OnNewIntent(intent);
#if DEBUG
        System.Console.WriteLine("S4O *!*!*!*!*!*!*!*!*!*!*!*!*! ON INTENT");
#endif
        IntentWorker(intent);  // Use the new intent

        Platform.OnNewIntent(intent);
    }

    private void IntentWorker(Intent intent)
    {
        var action = intent.Action;
        var type = intent.Type;

        // Store the intent data in a static variable
        if (Intent.ActionSend.Equals(action) && type != null)
        {
            var uri = (Android.Net.Uri)intent.GetParcelableExtra(Intent.ExtraStream);
#if DEBUG
            System.Console.WriteLine("CSV Received: " + uri);
#endif
            // Create copy in local app space and store the path in a static variable
            var appFilePath = AndroidHelpers.UriResolver.CopyFileFromUriToAppSpace(this, uri);

            Models.SharedData.StartFromShare = true;
            Models.SharedData.FileUri = appFilePath;
        }
    }

    
}

