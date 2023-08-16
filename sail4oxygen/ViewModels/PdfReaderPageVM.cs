using System;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace sail4oxygen.ViewModels
{
	public class PdfReaderPageVM : ObservableObject
    {
        private Stream m_pdfDocumentStream;
        public Stream PdfDocumentStream
        {
            get
            {
                return m_pdfDocumentStream;
            }
            set
            {
                m_pdfDocumentStream = value;
                OnPropertyChanged("PdfDocumentStream");
            }
        }


        public PdfReaderPageVM()
		{
            m_pdfDocumentStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream(Models.FaqHelper.PdfManualFileResult.FullPath);
            _=LoadFile();
        }

        private async Task<bool> LoadFile()
        {
            try
            {
                PdfDocumentStream = await Models.FaqHelper.PdfManualFileResult.OpenReadAsync();
                return true;
            }
            catch (Exception ex)
            {
                //Display error when file picker failed to open files.
                string message;
                if (ex != null && string.IsNullOrEmpty(ex.Message) == false)
                    message = ex.Message;
                else
                    message = "File open failed.";
                Application.Current?.MainPage?.DisplayAlert("Error", message, "OK");
            }
            return false;
        }
	}
}

