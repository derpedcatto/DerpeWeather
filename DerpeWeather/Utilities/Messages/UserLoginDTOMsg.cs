using CommunityToolkit.Mvvm.Messaging.Messages;
using DerpeWeather.DAL.DTO;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Message that passes <see cref="UserLoginDTO"/>.
    /// </summary>
    public class UserLoginDTOMsg : ValueChangedMessage<UserLoginDTO>
    {
        public UserLoginDTOMsg(UserLoginDTO value) : base(value)
        {
        }
    }
}
