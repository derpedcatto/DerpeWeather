using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Message that passes <see cref="Guid"/> UserID to <see cref="UserPreferencesVM"/>
    /// from <see cref="MainWindowVM"/>.
    /// </summary>
    public class PrefWndUserIdMsg : ValueChangedMessage<Guid>
    {
        public PrefWndUserIdMsg(Guid userId) : base(userId) { }
    }
}
