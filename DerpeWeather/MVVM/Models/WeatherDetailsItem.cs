namespace DerpeWeather.MVVM.Models
{
    /// <summary>
    /// Model that is used to display detailed Location weather data in a list in view.
    /// </summary>
    public class WeatherDetailsItem
    {
        public string Datetime { get; set; }
        public double? Tempmax { get; set; }
        public double? Tempmin { get; set; }
        public double? Temp { get; set; }
        public double? Feelslike { get; set; }
        public double? Humidity { get; set; }
        public double? Snow { get; set; }
        public double? Pressure { get; set; }
        public double? Visibility { get; set; }
        public string Conditions { get; set; }
        public string Description { get; set; }
    }
}
