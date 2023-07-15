using System;
using System.Globalization;

namespace sail4oxygen.Models
{
	public static class CSVHelper
	{
        const int headerRowNumber = 10;
        public static async Task<bool> AddLocation(Uri file, Location location)
        {
            try
            {
                string csvText = await File.ReadAllTextAsync(file.LocalPath);
                string[] csvLines = csvText.Split('\n');

                if (await VerifyCsvIsValidFile(csvLines))
                {
                    string newCsvText = csvLines[0];
                    for (int i = 0; i < csvLines.Length; i++)
                    {
                        
                        switch (i)
                        {
                            //Copy first n lines
                            case < headerRowNumber:
                                newCsvText += csvLines[i];
                                break;
                            //add Lat Lon in Header 
                            case headerRowNumber:
                                newCsvText += csvLines[i] += ",Latitude,Longitude\n";
                                break;
                            //add LatLonValue to each row past 
                            default:
                                newCsvText += csvLines[i] += "," + location.Latitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + location.Longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "\n";
                                break;
                        }
                    }

                    await File.WriteAllTextAsync(file.LocalPath, newCsvText);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("CsvHelper.AddLocation – Something went wrong: ", ex.Message);
            }
#if DEBUG
            Console.WriteLine("CsvHelper.Addlocation – Something went wrong");
#endif
            return false;
        }

        static async Task<bool> VerifyCsvIsValidFile (string[] fileContent)
        { 
            try {
                //correct file content
                if (fileContent[1].StartsWith("Kor MEASUREMENT DATA FILE EXPORT") && 
                    fileContent[headerRowNumber].StartsWith("Date (MM/DD/YYYY)") && 
                    (fileContent[headerRowNumber].IndexOf(',') > -1))
                {
                    //recent measurement
                    //find timestamp in row 4 column 2 and check if it is file is recent
                    var measure = fileContent[headerRowNumber + 1].Split(',');
                    var timestamp = DateTime.Parse(measure[0]+ " " + measure[1], new CultureInfo("en-US"));
                    var fileage = DateTime.Now - timestamp;
#if DEBUG
                    Console.WriteLine("VerifyCsvIsValidFile – Measurement timestamp: ", timestamp.ToString());
#endif
                    if (fileage.TotalMinutes > 45)
                    {
                        if (await Application.Current.MainPage.DisplayAlert("File too old", $"Your measurement is {fileage} Minutes old. Are you still nearby where you took it or have you adjusted the coordinates before sending? ", "Yes","No"))
                        {
                            return true;
                        }
                        else
                        {
                            Models.SharedData.LastError = $"Measruement is {fileage.TotalMinutes} Minutes old and coordinates were not adjusted";
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    Models.SharedData.LastError = "File is not a valid measurement file.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Models.SharedData.LastError = "VerifyCsvIsValidFile: " + ex.Message;
                Console.WriteLine(Models.SharedData.LastError);
                await Application.Current.MainPage.DisplayAlert("Error", $"{Models.SharedData.LastError}", "Cancel");
                return false;
            }   
        }
    }
}

