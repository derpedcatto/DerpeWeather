using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Message that gets sent from <see cref="ChangeAvatarVM"/> and all other ViewModels
    /// that display avatar images. Passes a string resembling Avatar Image Path.
    /// Also sent to <see cref="ChangeAvatarWindow"/> and signals that avatar changed successfully.
    /// </summary>
    public class ChangeAvatarSuccessMsg : ValueChangedMessage<string>
    {
        public ChangeAvatarSuccessMsg(string value) : base(value)
        {
        }
    }
}
