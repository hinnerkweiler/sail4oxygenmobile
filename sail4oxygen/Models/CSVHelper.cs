using System;
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
                string[] csvHeader = csvLines[0].Split(',');
                string[] csvValues = csvLines[1].Split(',');

                string newCsvText = csvLines[0] + ";Latitude;Longitude\n";

                for (int i = 1; i < csvLines.Length; i++)
                {
                    newCsvText += csvLines[i] + "," + location.Latitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "," + location.Longitude.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + "\n";
                }

                await File.WriteAllTextAsync(file.LocalPath, newCsvText);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: ", ex.Message);
            }
            return false;
        }
    }
}

