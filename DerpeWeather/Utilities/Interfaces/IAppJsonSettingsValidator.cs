using System.Threading.Tasks;

namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// Interface to test if `appsettings.json` data is valid.
    /// </summary>
    public interface IAppJsonSettingsValidator
    {
        /// <summary>
        /// Validate given SQL Connection String.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>
        /// true - connection was set.
        /// false - connection error.
        /// </returns>
        Task<bool> ValidateSQLConnectionStringAsync(string connectionString);

        /// <summary>
        /// Validate if API Key is valid and connection to Weather API Connection is valid.
        /// </summary>
        /// <param name="apiKey">Given API key.</param>
        /// <returns>
        /// true - connection was set.
        /// false - connection error.
        /// </returns>
        Task<bool> ValidateWeatherAPIConnectionAsync(string apiKey);
    }
}
