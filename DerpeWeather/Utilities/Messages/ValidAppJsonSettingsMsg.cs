using CommunityToolkit.Mvvm.Messaging.Messages;
using DerpeWeather.MVVM.Models;

namespace DerpeWeather.Utilities.Messages
{
    public class ValidAppJsonSettingsMsg : ValueChangedMessage<AppJsonSettings>
    {
        public ValidAppJsonSettingsMsg(AppJsonSettings value) : base(value)
        {
        }
    }
}
