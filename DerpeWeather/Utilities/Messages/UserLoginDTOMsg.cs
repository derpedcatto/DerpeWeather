using CommunityToolkit.Mvvm.Messaging.Messages;
using DerpeWeather.DAL.DTO;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Message that passes <see cref="UserDisplayInfoDTO"/>.
    /// </summary>
    public class UserLoginDTOMsg : ValueChangedMessage<UserDisplayInfoDTO>
    {
        public UserLoginDTOMsg(UserDisplayInfoDTO value) : base(value)
        {
        }
    }
}
