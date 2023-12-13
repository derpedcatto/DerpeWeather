using DerpeWeather.MVVM.Models;
using DerpeWeather.Utilities.Interfaces;
using RestSharp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DerpeWeather.Utilities.Helpers.WeatherApi
{
    /// <summary>
    /// Implementation of the <see cref="IWeatherApiClient"/> interface that uses VisualCrossing to fetch data.
    /// </summary>
    public class VisualCrossingWeatherApiClient : IWeatherApiClient, IDisposable
    {
        private readonly IRestClient _restClient;
        private readonly IUserRepo _userRepo;
        private bool disposedValue;



        /// <summary>
        /// Default constructor.
        /// </summary>
        public VisualCrossingWeatherApiClient(IRestClient restClient, IUserRepo userRepo)
        {
            _restClient = restClient;
            _userRepo = userRepo;
        }



        public async Task<UserTrackedWeatherFieldItem>? GetWeatherData(Guid userId, string locationName, CancellationToken cancellationToken)
        {
            var user = _userRepo.GetUser(userId);
            var unitGroup = user.Preferences.Units switch
            {
                Enums.UserPreferenceUnits.US => "us",
                Enums.UserPreferenceUnits.METRIC => "metric",
                Enums.UserPreferenceUnits.UK => "uk"
            };


            var request = new RestRequest(@$"{locationName}?unitGroup={unitGroup}&key={App.WeatherAPIKey}&contentType=json", Method.Get);
            var response = await _restClient.ExecuteAsync<WeatherApiResponse>(request, cancellationToken: cancellationToken);

            if (response.IsSuccessful)
            {
                var apiResponse = response.Data;

                var weatherDetails = apiResponse.Days.Select(day => new WeatherDetailsItem
                {
                    Datetime = day.Datetime,
                    Tempmax = day.Tempmax,
                    Tempmin = day.Tempmin,
                    Temp = day.Temp,
                    Feelslike = day.Feelslike,
                    Humidity = day.Humidity,
                    Snow = day.Snow,
                    Pressure = day.Pressure,
                    Visibility = day.Visibility,
                    Conditions = day.Conditions,
                    Description = day.Description
                }).ToList();

                return new UserTrackedWeatherFieldItem
                {
                    Location = apiResponse.Address,
                    ResolvedLocation = apiResponse.ResolvedAddress,
                    Timezone = apiResponse.Timezone,
                    Temperature = apiResponse.CurrentConditions.Temp.ToString(),
                    Condition = apiResponse.CurrentConditions.Conditions,
                    Description = apiResponse.Description,
                    WeatherDetails = weatherDetails
                };
            }
            else
            {
                return null;
            }
        }



        public async Task<bool> ValidateApiKey(string apiKey, CancellationToken cancellationToken)
        {
            var request = new RestRequest(@$"London?unitGroup=us&include=current&key={apiKey}&contentType=json", Method.Get);
            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            return response.IsSuccessful;
        }



        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _restClient?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
