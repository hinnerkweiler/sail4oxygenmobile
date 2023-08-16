using System;


namespace sail4oxygen.Models
{
	public static class FaqHelper
	{
		
		public static FileResult PdfManualFileResult
        {
            get
            {
				if (File.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, PdfManualFileName)))
				{
						return new FileResult(Path.Combine(FileSystem.Current.AppDataDirectory, PdfManualFileName), "application/pdf");
				}
				SharedData.LastError = "Manual file not found!";
				Console.WriteLine(SharedData.LastError + " - " + Path.Combine(FileSystem.Current.AppDataDirectory, PdfManualFileName));
				return null;
			}
		}

        public static string PdfManualFileName
        {
            get
            {
                if (System.Globalization.RegionInfo.CurrentRegion.Name.ToUpper() == "DE")
                {
                    return $"exo3_manual_de.pdf";
                }
                return $"exo3_manual_de.pdf";
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

		
		public static void Init()
		{
			if (!File.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, PdfManualFileName)))
			{
				_ = CopyReleaseItemToAppFolder(PdfManualFileName);
			}
		}


		public async static Task CopyReleaseItemToAppFolder(string name)
		{
			if (await FileSystem.AppPackageFileExistsAsync(PdfManualFileName))
			{
				try
				{
					var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, name);

					using (Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(name)) // read pdf stored as MauiAsset
					using (BinaryReader reader = new BinaryReader(inputStream)) // read bytes from pdf file as stream
					using (FileStream outputStream = File.Create(filePath)) // create destination pdf file
					using (BinaryWriter writer = new BinaryWriter(outputStream)) // write output stream to destination pdf
					{
						// Read bytes from input stream and write to output stream
						byte[] buffer = new byte[4096];
						int bytesRead;
						while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
						{
							writer.Write(buffer, 0, bytesRead);
						}
					}

                    Preferences.Set("ManualDownloaded", DateTime.MinValue.AddDays(1).ToString());
#if DEBUG
                    Console.WriteLine("***** Manual copied");
#endif
                }
                catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
            
#if DEBUG
            Console.WriteLine("***** Manual NOT copied");
#endif
        }

		public async static Task GetManualFromServer()
		{
			if (ManualDownloaded < DateTime.Now.AddDays(-1))
			{
				await DownloadPdf(url: PrivatData.PdfManualUrl, name: PdfManualFileName, mimeType: "application/pdf");
			}
		}

        private async static Task<FileResult> DownloadPdf(string url, string name, string mimeType)
		{
			string filePath = Path.Combine(FileSystem.AppDataDirectory, name);
            

		
            try
			{
				using (var client = new HttpClient
						{
							Timeout = TimeSpan.FromSeconds(5)
						})
				{
					using (var stream = await client.GetStreamAsync(url))
					{
						using (var fileStream = File.Create(filePath))
						{
							stream.CopyTo(fileStream);
						}
					}
				}

				Preferences.Set("FaqDownloaded", DateTime.Now.ToString());
				return new FileResult(filePath, "application/pdf");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine("Could not Download FAQ File: " + ex.Message);
			}
			return null;
		}
	}
}

