using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace sail4oxygen.Models
{
	public class LocationChangeMessage : ValueChangedMessage<Location>

    {
        public LocationChangeMessage(Location value) : base(value)
		{
		}

		
	}

}

