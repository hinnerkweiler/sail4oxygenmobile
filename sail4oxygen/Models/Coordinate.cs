using System;
using CommunityToolkit.Mvvm.ComponentModel;
namespace sail4oxygen.Models
{
	public partial class Coordinate : ObservableObject
	{
		[ObservableProperty]
		public char direction;

		[ObservableProperty]
		public int degrees;

		[ObservableProperty]
		public double minutes;
    }
}

