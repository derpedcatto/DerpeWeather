using System.Collections.Generic;

namespace DerpeWeather.MVVM.Models
{
    /// <summary>
    /// Model that is used to display Weather Location data in a list in view.
    /// </summary>
    public class UserTrackedWeatherFieldItem
    {
        public string Location { get; set; }
        public string ResolvedLocation { get; set; }
        public string Timezone { get; set; }
        public string Temperature { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public List<WeatherDetailsItem> WeatherDetails { get; set; }
    }
}
