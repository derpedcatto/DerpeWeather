using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DerpeWeather.Utilities.Messages
{
    /// <summary>
    /// Message with value of New Tracked Weather Location string that 
    /// gets sent from <see cref="MVVM.ViewModels.AddLocationVM"/>
    /// to <see cref="MVVM.ViewModels.MainWindowVM"/> 
    /// to update ListView with new field.
    /// </summary>
    public class NewLocationStrMsg : ValueChangedMessage<string>
    {
        public NewLocationStrMsg(string message) : base(message) { }
    }
}
