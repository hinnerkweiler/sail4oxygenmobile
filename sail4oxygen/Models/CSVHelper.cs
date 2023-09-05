using System;
using System.Globalization;

namespace sail4oxygen.Models
{
	public static class CSVHelper
	{
        const int headerRowNumber = 10;

        const int maxMeasureAge = 30;

       
        public static async Task<bool> AddLocation(string file, Location location)
        {
            try
            {
                string csvText = await File.ReadAllTextAsync(file);
                string[] csvLines = csvText.Split('\n');

                if (await VerifyCsvIsValidFile(csvLines))
                {
                    string newCsvText = csvLines[0];
                    for (int i = 1; i < csvLines.Length-1; i++)
                    {
                        switch (i)
                        {
                            //Copy first n lines
                            case < headerRowNumber:
                                newCsvText += csvLines[i];
                                break;
                            //add Lat Lon in Header 
                            case headerRowNumber:
                                var newHeaderLine = csvLines[i].Replace("\n", "").Replace("\r", "");
                                newCsvText += newHeaderLine + ",Latitude,Longitude,Boatname\n\r";
                                break;
                            //add LatLonValue to each row past 
                            default:
                                var newLine = csvLines[i].Replace("\n", "").Replace("\r", "");
                                newCsvText += newLine + "," + location.Latitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + location.Longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) +"," + PreferencesHelper.BoatName + "\n\r";
                                break;
                        }
                    }
                    try
                    {
                        await File.WriteAllTextAsync(file, newCsvText);

                    }
                    catch (Exception ex)
                    {
                        Models.SharedData.LastError = Resources.Languages.lang.FileWriteLastError + "  (" + ex.Message + ")";
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("CsvHelper.AddLocation – Something went wrong: " + ex.Message);
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
                    (fileContent[headerRowNumber].IndexOf(',') > -1 ) &&
                    (!Regex.IsMatch(fileContent[headerRowNumber+1], @"""(\d+,\d+)""")))  // all decimals must be in en-US format
                {
                    //recent measurement
                    //find timestamp in row 4 column 2 and check if file is recent
                    var measure = fileContent[headerRowNumber + 1].Split(',');
                    var timestamp = DateTime.Parse(measure[0]+ " " + measure[1], new CultureInfo("en-US"));
                    var fileage = DateTime.Now.ToUniversalTime() - timestamp;
#if DEBUG
                    Console.WriteLine("VerifyCsvIsValidFile – Measurement timestamp: " + timestamp.ToString());
#endif
                    if (fileage.TotalMinutes > maxMeasureAge)
                    {
                        if (await Application.Current.MainPage.DisplayAlert(Resources.Languages.lang.FileTooOldAlertTitel, Resources.Languages.lang.FileTooOldAlertText1+ " " + Math.Round(fileage.TotalMinutes,MidpointRounding.AwayFromZero) + " " + Resources.Languages.lang.FileTooOldAlertText2, Resources.Languages.lang.ContinueSending, Resources.Languages.lang.CorrectCoordinates))
                        {
                            return true;
                        }
                        else
                        {
                            Models.SharedData.LastError = Resources.Languages.lang.FileTooOldLastError;
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
                    Models.SharedData.LastError = Resources.Languages.lang.WrongFileLastError;
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

