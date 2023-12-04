using DerpeWeather.MVVM.Models;
using System;
using System.Threading.Tasks;

namespace DerpeWeather.Utilities.Interfaces
{
    public interface IWeatherApiClient
    {
        Task<UserTrackedWeatherFieldItem>? GetWeatherDataForToday(Guid userId, string locationName);
    }
}
