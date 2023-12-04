using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Message that passes <see cref="Guid"/> with UserID. Passed to <see cref="MainWindow"/> for it to know which user must be logged in (by ID).
    /// </summary>
    public class OpenMainWindowMsg : ValueChangedMessage<Guid>
    {
        public OpenMainWindowMsg(Guid userId) : base(userId)
        {
        }
    }
}
