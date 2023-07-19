using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;



namespace sail4oxygen;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"*/*", Label = "Send to GEOMAR")]

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
        IntentWorker();

        Platform.OnResume(this);
    }

    protected override void OnNewIntent(Android.Content.Intent intent)
    {
        base.OnNewIntent(intent);
#if DEBUG
        System.Console.WriteLine("S4O *!*!*!*!*!*!*!*!*!*!*!*!*! ON INTENT");
#endif
        IntentWorker();

        Platform.OnNewIntent(intent);
    }

    private void IntentWorker()
    {
        var intent = Intent;
        var action = intent.Action;
        var type = intent.Type;


        //store the intent data in a static variable
        if (Intent.ActionSend.Equals(action) && type != null)
        {
        
            var uri = (Android.Net.Uri)intent.GetParcelableExtra(Intent.ExtraStream);
#if DEBUG
            System.Console.WriteLine("CSV Recived: " + uri);
#endif
            //create copy in local app space and store the path in a static variable
            var appFilePath = AndroidHelpers.UriResolver.CopyFileFromUriToAppSpace(this, uri);

            Models.SharedData.StartFromShare = true;
            Models.SharedData.FileUri = appFilePath;
        }
    }
    
}

