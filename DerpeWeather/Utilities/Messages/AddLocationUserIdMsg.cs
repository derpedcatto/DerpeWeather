using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace DerpeWeather.Utilities.Messages
{
    public class AddLocationUserIdMsg : ValueChangedMessage<System.Guid>
    {
        public AddLocationUserIdMsg(Guid value) : base(value) { }
    }
}
