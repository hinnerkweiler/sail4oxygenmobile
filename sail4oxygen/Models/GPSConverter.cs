using System;

namespace sail4oxygen.Models
{
	public enum Orientation {isLatitude, isLongitude}

	public static class GPSConverter
	{
		//Static functions that 
		// - can take a double coordinate and retun them as a string in the format of degrees and minutes (dd° mm.mmm') and n/s/e/w
		// - can take a string in the format of degrees and minutes (dd° mm.mmm') and return a double coordinate
		// - can take a double coordinate and return an object of type Coordinate that has seperate properties for degrees, minutes and n/s/e/w
		
		public static string DoubleToDegreesMinutes(double coordinate, Orientation orientation)
		{
			//Takes a double coordinate and returns it as a string in the format of degrees and minutes (dd° mm.mmm') and n/s/e/w
			//direction is a string that can be "n", "s", "e" or "w"
			//coordinate is a double coordinate
			//returns a string in the format of degrees and minutes (dd° mm.mmm') and n/s/e/w
			//example: doubleToDegreesMinutes(51.123456, "n") returns "51° 12.3456' N"
			
			//Get the degrees
			int degrees = (int)coordinate;

			//Get the minutes
			double minutes = (coordinate - degrees) * 60;

			//Return the string
			return degrees.ToString() + "° " + minutes.ToString("0.0000") + "' " + GetOrientationChar(orientation,coordinate).ToString();
		}

		private static double DegreesMinutesToDouble(string coordinate)
		{
			//Takes a string in the format of degrees and minutes (dd° mm.mmm') and returns a double coordinate
			//coordinate is a string in the format of degrees and minutes (dd° mm.mmm')
			//returns a double coordinate
			//example: DegreesMinutesTodouble("51° 12.3456'") returns 51.123456
			
			//Split the string into degrees and minutes
			string[] splitCoordinate = coordinate.Split('°', '\'');
			
			//Get the degrees
			int degrees = int.Parse(splitCoordinate[0]);
			
			//Get the minutes
			double minutes = double.Parse(splitCoordinate[1]);
			
			//Return the double coordinate
			return degrees + (minutes / 60);
		}

		
		public static Coordinate CoordinateDoubleToCoordinate(double coordinate, Orientation orientation )
		{
			//Takes a double coordinate and returns an object of type Coordinate that has seperate properties for degrees, minutes and n/s/e/w
			//coordinate is a double coordinate
			//returns an object of type Coordinate that has seperate properties for degrees, minutes and n/s/e/w
			//example: doubleToCoordinate(51.123456) returns an object of type Coordinate with the properties degrees = 51, minutes = 12.3456 and direction = "e"
			
			//Create a new Coordinate object
			Coordinate newCoordinate = new Coordinate();
			
			//Get the degrees
			newCoordinate.Degrees = (int)coordinate;
			
			//Get the minutes
			newCoordinate.Minutes = (coordinate - newCoordinate.Degrees) * 60;

			//Get the direction
			newCoordinate.Direction = GetOrientationChar(orientation,coordinate);
			
			//Return the Coordinate object
			return newCoordinate;
		}

		public static double CoordinateToCoordinatedouble(Coordinate coordinate)
		{
			//Takes an object of type Coordinate that has seperate properties for degrees, minutes and n/s/e/w and returns a double coordinate
			//coordinate is an object of type Coordinate that has seperate properties for degrees, minutes and n/s/e/w
			//returns a double coordinate
			//example: CoordinateTodouble(new Coordinate(51, 12.3456, "e")) returns 51.123456
			
			//Get the double coordinate
			double doubleCoordinate = coordinate.Degrees + (coordinate.Minutes / 60);
			
			//Return the double coordinate
			return doubleCoordinate;
		}

		public static double DegreesMinutesTodouble(int degrees, double minutes)
		{
			//Takes degrees and minutes and returns a double coordinate
			//degrees is an integer
			//minutes is a double
			//returns a double coordinate
			//example: DegreesMinutesTodouble(51, 12.3456) returns 51.123456
			
			//Get the double coordinate
			double doubleCoordinate = degrees + (minutes / 60);
			
			//Return the double coordinate
			return doubleCoordinate;
		}

		public static double DegreesMinutesTodouble(int degrees, double minutes, string direction, int doublePlaces = 6)
		{
			//Takes degrees, minutes, direction and double places and returns a double coordinate
			//degrees is an integer
			//minutes is a double
			//direction is a string that can be "N", "S", "E" or "W"
			//doublePlaces is an integer
			//returns a double coordinate
			//example: DegreesMinutesTodouble(51, 12.3456, "e", 4) returns 51.1235
			
			//Get the double coordinate
			double doubleCoordinate = degrees + (minutes / 60);
			
			//If the direction is south or west, make the double coordinate negative
			if (direction == "S" || direction == "W")
			{
				doubleCoordinate = doubleCoordinate * -1;
			}
			
			//Round the double coordinate to the specified number of double places
			doubleCoordinate = Math.Round(doubleCoordinate, doublePlaces, MidpointRounding.AwayFromZero);
			
			//Return the double coordinate
			return doubleCoordinate;
		}

		private static char GetOrientationChar(Orientation orientation, double value)
		{
            switch (orientation)
            {
                case Orientation.isLatitude:
                    if (value > 0)
                    {
                        return 'N';
                    }
                    else
                    {
                        return 'S';
                    }
                default:
                    if (value > 0)
                    {
                        return 'E';
                    }
                    else
                    {
                        return 'W';
                    }
                    
            }
        }

	}
}

