using System;
using CommunityToolkit.Mvvm.ComponentModel;
namespace sail4oxygen.Models
{
	//Holds a Coordinate in Degrees and Decimal Minutes (dd° mm.mmmmm)
	
	public partial class Coordinate : ObservableObject
	{
		[ObservableProperty]
		private char _direction;

		private int _degrees;
		
		public int Degrees
		{
			get => _degrees;
			set
			{
				SetProperty(ref _degrees, value);
			}
		}
		
		private double _minutes;
		
		public double Minutes
		{
			get
			{
				return Math.Round(_minutes, 3);
			}
			set
			{
				SetProperty(ref _minutes, value);
			}
		}
		
		public double ToDouble()
		{
			// Convert the Coordinate dd° mm.mmmmm to a double.
			
			var sign = (Direction == 'N' || Direction == 'E') ? 1 : -1;
			return sign * (Degrees + (Minutes / 60));
		}
        
        
    }
}

