using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Message that passes <see cref="Guid"/> UserID from <see cref="MainWindowVM"/>
    /// to <see cref="AddLocationVM"/>.
    /// </summary>
    public class AddLocationUserIdMsg : ValueChangedMessage<System.Guid>
    {
        public AddLocationUserIdMsg(Guid value) : base(value) { }
    }
}
