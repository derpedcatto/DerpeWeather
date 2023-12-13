using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Message that passes <see cref="Guid"/> UserID from <see cref="ChooseUserVM"/>
    /// to a window that works with logged in user.
    /// </summary>
    public class UserLoginIdMsg : ValueChangedMessage<System.Guid>
    {
        public UserLoginIdMsg(Guid value) : base(value)
        {
        }
    }
}
