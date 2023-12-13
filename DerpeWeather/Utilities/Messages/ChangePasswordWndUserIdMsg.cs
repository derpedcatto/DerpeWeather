using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Passes <see cref="Guid"/> UserID from <see cref="MainWindow"/> to
    /// <see cref="ChangePasswordVM"/>.
    /// </summary>
    public class ChangePasswordWndUserIdMsg : ValueChangedMessage<Guid>
    {
        public ChangePasswordWndUserIdMsg(Guid value) : base(value)
        {
        }
    }
}
