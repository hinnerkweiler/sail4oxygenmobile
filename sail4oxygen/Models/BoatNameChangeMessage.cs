using CommunityToolkit.Mvvm.Messaging.Messages;

namespace sail4oxygen.Models;

public class BoatNameChangeMessage : ValueChangedMessage<string>
{
    public BoatNameChangeMessage(string value) : base(value)
    {
    }    
}