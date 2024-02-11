using System;
using CommunityToolkit.Mvvm.ComponentModel;
namespace sail4oxygen.Models
{
	//Holds a Coordinate in Degrees and Decimal Minutes (dd° mm.mmmmm)
	
	public partial class Coordinate : ObservableObject
	{
		[ObservableProperty]
		private char _direction;

		[ObservableProperty]
		private int _degrees;

		[ObservableProperty]
		private double _minutes;
		
		public double ToDouble()
		{
			// Convert the Coordinate dd° mm.mmmmm to a double.
			
			var sign = (Direction == 'N' || Direction == 'E') ? 1 : -1;
			return sign * (Degrees + (Minutes / 60));
		}
    }
}

