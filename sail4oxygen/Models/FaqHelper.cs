using System;
using Newtonsoft.Json.Linq;

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

                    Preferences.Set("ManualDownloaded", DateTime.MinValue.AddDays(1));
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
                await DownloadPdf(url: PrivatData.ManualPdfUrl, name: PdfManualFileName, mimeType: "application/pdf");
        }


        private async static Task<DateTime> GetLatestManualUpdateDateTime(string url)
        {
#if DEBUG
            Console.WriteLine("********Download Json from Server");
#endif
            
            DateTime lastUpdate = DateTime.MinValue;
            string updateJson = "";
            try
            {
                using (var client = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(5)
                })
                {
                    updateJson = await client.GetStringAsync(url);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Console.WriteLine("Could not Download JSON File: " + ex.Message);
#endif
            return lastUpdate;
            }

            
            try
            {
#if DEBUG
                Console.WriteLine("********Decode Json ");
#endif
                var json = JObject.Parse(updateJson);
                lastUpdate = json["lastUpdate"].Value<DateTime>();
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Console.WriteLine("Could not parse JSON String: " + updateJson + " | Throws: " + ex.Message);
#endif
            }
            return lastUpdate;
        }


        public async static Task UpdateAssets()
        {

#if DEBUG
            Console.WriteLine("********Update Manual from Server");
#endif

            await GetManualFromServer();

#if DEBUG
            Console.WriteLine("********Update Manual from Server complete");
#endif
            
        }

        private async static Task<FileResult> DownloadPdf(string url, string name, string mimeType)
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, name);
#if DEBUG
            Console.WriteLine("********Download Manual from Server");
#endif
            try
            {
                using (var client = new HttpClient
                        {
                            Timeout = TimeSpan.FromSeconds(5)
                        })
                {
                    using (var stream = await client.GetStreamAsync(url).ConfigureAwait(false))
                    {
                        using (var fileStream = File.Create(filePath))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                    
                }

                Preferences.Set("ManualDownloaded", DateTime.Now);
                return new FileResult(filePath, "application/pdf");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Could not Download FAQ File: " + ex.Message);
            }
#if DEBUG
            Console.WriteLine("********Download Manual from Server finished");
#endif
            return null;
        }
    }
}

