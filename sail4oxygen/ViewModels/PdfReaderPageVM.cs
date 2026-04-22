using CommunityToolkit.Mvvm.ComponentModel;

namespace sail4oxygen.ViewModels
{
    public class PdfReaderPageVM : ObservableObject
    {
        private string _pdfFilePath;
        public string PdfFilePath
        {
            get => _pdfFilePath;
            set => SetProperty(ref _pdfFilePath, value);
        }

        public PdfReaderPageVM()
        {
            var fileResult = Models.FaqHelper.PdfManualFileResult;
            if (fileResult != null)
                PdfFilePath = fileResult.FullPath;
        }
    }
}

