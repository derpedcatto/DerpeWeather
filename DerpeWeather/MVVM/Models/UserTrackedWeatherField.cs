using System;

namespace DerpeWeather.MVVM.Models
{
    /// <summary>
    /// Model that represents weather location data to grab through API.
    /// </summary>
    public class UserTrackedWeatherField
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public string ResolvedLocation { get; set; }
    }
}
