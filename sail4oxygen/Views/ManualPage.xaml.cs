namespace sail4oxygen.Views;

public partial class ManualPage : ContentPage
{
    ViewModels.PdfReaderPageVM manualPageVM = new ViewModels.PdfReaderPageVM();

	public ManualPage()
	{
		this.BindingContext = manualPageVM;
		InitializeComponent();
    }
}
