using DerpeWeather.Utilities.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DerpeWeather.Utilities.Helpers.UserData
{
    /// <summary>
    /// Main implementation of <see cref="IAppJsonSettingsValidator"/> interface.
    /// </summary>
    public class AppJsonSettingsValidator : IAppJsonSettingsValidator, IDisposable
    {
        private bool disposedValue;
        private readonly IWeatherApiClient _weatherApiClient;
        


        public AppJsonSettingsValidator(IWeatherApiClient weatherApiClient)
        {
            _weatherApiClient = weatherApiClient;
        }



        public async Task<bool> ValidateSQLConnectionStringAsync(string sqlConnectionString)
        {
            bool isValid;

            using var _ctsSql = new CancellationTokenSource();
            try
            {
                using var conn = new SqlConnection(sqlConnectionString);
                await conn.OpenAsync(_ctsSql.Token);

                isValid = true;
            }
            catch (Exception ex)
            {
                // Console.WriteLine("SQL String error: " + ex.Message);
                _ctsSql.Cancel();
                isValid = false;
            }

            return isValid;
        }


        public async Task<bool> ValidateWeatherAPIConnectionAsync(string weatherApiKey)
        {
            bool keyIsValid;

            using var _ctsApi = new CancellationTokenSource();
            try
            {
                keyIsValid = await _weatherApiClient.ValidateApiKey(weatherApiKey, _ctsApi.Token);
            }
            catch (Exception ex)
            {
                _ctsApi.Cancel();
                keyIsValid = false;
            }
            
            /*
            if (!keyIsValid)
            {
                Console.WriteLine("KeyIsValid error");
            }
            */

            return keyIsValid;
        }



        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

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
