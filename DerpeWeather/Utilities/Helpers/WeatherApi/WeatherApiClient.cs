using DerpeWeather.MVVM.Models;
using DerpeWeather.Utilities.Interfaces;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace DerpeWeather.Utilities.Helpers.WeatherApi
{
    public class WeatherApiClient : IWeatherApiClient, IDisposable
    {
        private readonly IRestClient _restClient;
        private readonly IUserRepo _userRepo;
        private bool disposedValue;



        public WeatherApiClient(IRestClient restClient, IUserRepo userRepo)
        {
            _restClient = restClient;
            _userRepo = userRepo;
        }



        public async Task<UserTrackedWeatherFieldItem>? GetWeatherDataForToday(Guid userId, string locationName)
        {
            var user = _userRepo.GetUser(userId);
            var unitGroup = user.Preferences.Units switch
            {
                Enums.UserPreferenceUnits.US => "us",
                Enums.UserPreferenceUnits.METRIC => "metric",
                Enums.UserPreferenceUnits.UK => "uk"
            };


            var request = new RestRequest(@$"{locationName}?unitGroup={unitGroup}&key={App.WeatherAPIKey}&contentType=json", Method.Get);
            var response = await _restClient.ExecuteAsync<WeatherApiResponse>(request);

            if (response.IsSuccessful)
            {
                var apiResponse = response.Data;

                return new UserTrackedWeatherFieldItem
                {
                    Location = apiResponse.Address,
                    Timezone = apiResponse.Timezone,
                    Temperature = apiResponse.CurrentConditions.Temp.ToString(),
                    Condition = apiResponse.CurrentConditions.Conditions,
                    Description = apiResponse.Description
                };
            }
            else
            {
                return null;
            }
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
