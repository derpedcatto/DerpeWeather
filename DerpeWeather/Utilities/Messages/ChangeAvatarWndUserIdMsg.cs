using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Passes <see cref="Guid"/> UserID from <see cref="MainWindow"/> to
    /// <see cref="ChangeAvatarVM"/>.
    /// </summary>
    public class ChangeAvatarWndUserIdMsg : ValueChangedMessage<Guid>
    {
        public ChangeAvatarWndUserIdMsg(Guid value) : base(value)
        {
        }
    }
}
