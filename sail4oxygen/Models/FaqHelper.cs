using System;
namespace sail4oxygen.Models
{
	public static class FaqHelper
	{
		
		public static FileResult PdfManualFileResult
        {
            get
            {
				if (File.Exists(Path.Combine(FileSystem.AppDataDirectory, PdfManualFileName)))
				{
						return new FileResult(Path.Combine(FileSystem.AppDataDirectory, PdfManualFileName), "application/pdf");
				}
				SharedData.LastError = "Manual file not found!";
				Console.WriteLine(SharedData.LastError);
				return null;
			}
		}

        private static string PdfManualFileName
        {
            get
            {
                if (System.Globalization.RegionInfo.CurrentRegion.Name.ToUpper() == "DE")
                {
                    return "exo3_manual_de.pdf";
                }
                return "exo3_manual_en.pdf";
            }
        }

        /// Returns true if the faq is downloaded and stored in app folder
        private static DateTime ManualDownloaded
		{
			get
			{
				if (Preferences.ContainsKey("ManualDownloaded") && Preferences.Get("ManualDownloaded", "") != "")
				{
					var lastDownload = DateTime.Parse(Preferences.Get("ManualDownloaded", DateTime.Now.ToString()));
					return lastDownload;
				}
				return DateTime.MinValue;
			}
		}

		//download the pdf from server and store it in app folder
		public async static Task<FileResult> DownloadManual()
		{
			return await DownloadPdf(url: PrivatData.PdfManualUrl, name: PdfManualFileName, mimeType: "application/pdf");
		}

        //public async static Task<FileResult> DownloadFaq()
        //{
        //    return await DownloadPdf(url: PrivatData.PdfFaqUrl, name: PdfManualFileName, mimeType: "application/pdf");
        //}


        private async static Task<FileResult> DownloadPdf(string url, string name, string mimeType)
		{
			string filePathDownload = Path.Combine(FileSystem.AppDataDirectory, name);

            try
			{
				using (var client = new HttpClient())
				{
					using (var stream = await client.GetStreamAsync(url))
					{
						using (var fileStream = File.Create(filePathDownload))
						{
							stream.CopyTo(fileStream);
						}
					}
				}
			}
			catch (Exception ex)
			{
				System.Console.WriteLine("Could not Download FAQ File: " + ex.Message);
			}

			Preferences.Set("FaqDownloaded", DateTime.Now.ToString());

			return new FileResult(filePathDownload, "application/pdf");
		}
	}
}

