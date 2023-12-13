using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Passes <see cref="Guid"/> UserID from <see cref="MainWindow"/> to
    /// <see cref="ChangeUsernameVM"/>.
    /// </summary>
    public class ChangeUsernameWndUserIdMsg : ValueChangedMessage<Guid>
    {
        public ChangeUsernameWndUserIdMsg(Guid value) : base(value)
        {
        }
    }
}
