using CommunityToolkit.Mvvm.Messaging.Messages;

namespace sail4oxygen.Models;

public class LocationPropertyChangedMessage : ValueChangedMessage<double>
{
    public string PropertyName { get; }

    public LocationPropertyChangedMessage(string propertyName, double newValue) : base(newValue)
    {
        PropertyName = propertyName;
    }
}