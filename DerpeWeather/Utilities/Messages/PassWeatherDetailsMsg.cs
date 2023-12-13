using CommunityToolkit.Mvvm.Messaging.Messages;
using DerpeWeather.MVVM.Models;
using System.Collections.Generic;

namespace DerpeWeather.Utilities.Messages
{
    public class PassWeatherDetailsMsg : ValueChangedMessage<List<WeatherDetailsItem>>
    {
        public PassWeatherDetailsMsg(List<WeatherDetailsItem> value) : base(value)
        {
        }
    }
}
