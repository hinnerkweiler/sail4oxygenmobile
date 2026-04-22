namespace sail4oxygen.Views;

public partial class ManualPage
{
    public ManualPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var fileName = Models.FaqHelper.PdfManualFileName;
        var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

        if (!File.Exists(filePath))
            await Models.FaqHelper.CopyReleaseItemToAppFolder(fileName);

        if (!File.Exists(filePath))
        {
            await DisplayAlert("Error", "Manual file not found.", "OK");
            return;
        }

#if ANDROID
        await Launcher.Default.OpenAsync(new OpenFileRequest(
            "Manual",
            new ReadOnlyFile(filePath)));
#else
        PdfWebView.Source = new UrlWebViewSource { Url = $"file://{filePath}" };
#endif
    }
}