using DerpeWeather.MVVM.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// Interface for the Weather API client.
    /// </summary>
    public interface IWeatherApiClient
    {
        /// <summary>
        /// Fetches the weather data for today and next 7 days for a specific user and location.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="locationName">The name of the location.</param>
        /// <param name="cancellationToken">A token that signals the cancellation of the operation.</param>
        /// <returns>A <see cref="UserTrackedWeatherFieldItem"/> object containing the weather data, or null if the operation fails.</returns>
        Task<UserTrackedWeatherFieldItem>? GetWeatherData(Guid userId, string locationName, CancellationToken cancellationToken);

        /// <summary>
        /// Validates the provided API key.
        /// </summary>
        /// <param name="apiKey">The API key to validate.</param>
        /// <param name="cancellationToken">A token that signals the cancellation of the operation.</param>
        /// <returns>True if the API key is valid, false otherwise.</returns>
        Task<bool> ValidateApiKey(string apiKey, CancellationToken cancellationToken);
    }
}
