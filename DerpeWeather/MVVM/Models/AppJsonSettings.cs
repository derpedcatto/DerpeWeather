namespace DerpeWeather.MVVM.Models
{
    /// <summary>
    /// Model for JSON data from `appsettings.json` in app AppData folder.
    /// </summary>
    public class AppJsonSettings
    {
        public string SQLConnectionString { get; set; }
        public string WeatherAPIKey { get; set; }
    }
}
