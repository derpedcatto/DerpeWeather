using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DerpeWeather.MVVM.Models
{
    /// <summary>
    /// Model that gets returned from VisualCrossing Weather API.
    /// </summary>
    public class WeatherApiResponse
    {
        [JsonPropertyName("queryCost")]
        public int? QueryCost { get; set; }

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("resolvedAddress")]
        public string ResolvedAddress { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("tzoffset")]
        public double? Tzoffset { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("days")]
        public List<WeatherApiDay> Days { get; set; }

        [JsonPropertyName("alerts")]
        public List<object> Alerts { get; set; }

        [JsonPropertyName("currentConditions")]
        public WeatherApiCurrentConditions CurrentConditions { get; set; }
    }

    /// <summary>
    /// Model that gets returned from VisualCrossing Weather API.
    /// </summary>
    public class WeatherApiCurrentConditions
    {
        [JsonPropertyName("datetime")]
        public string Datetime { get; set; }

        [JsonPropertyName("datetimeEpoch")]
        public int? DatetimeEpoch { get; set; }

        [JsonPropertyName("temp")]
        public double? Temp { get; set; }

        [JsonPropertyName("feelslike")]
        public double? Feelslike { get; set; }

        [JsonPropertyName("humidity")]
        public double? Humidity { get; set; }

        [JsonPropertyName("dew")]
        public double? Dew { get; set; }

        [JsonPropertyName("precip")]
        public double? Precip { get; set; }

        [JsonPropertyName("precipprob")]
        public double? Precipprob { get; set; }

        [JsonPropertyName("snow")]
        public double? Snow { get; set; }

        [JsonPropertyName("snowdepth")]
        public double? Snowdepth { get; set; }

        [JsonPropertyName("preciptype")]
        public object Preciptype { get; set; }

        [JsonPropertyName("windgust")]
        public double? Windgust { get; set; }

        [JsonPropertyName("windspeed")]
        public double? Windspeed { get; set; }

        [JsonPropertyName("winddir")]
        public double? Winddir { get; set; }

        [JsonPropertyName("pressure")]
        public double? Pressure { get; set; }

        [JsonPropertyName("visibility")]
        public double? Visibility { get; set; }

        [JsonPropertyName("cloudcover")]
        public double? Cloudcover { get; set; }

        [JsonPropertyName("solarradiation")]
        public double? Solarradiation { get; set; }

        [JsonPropertyName("solarenergy")]
        public double? Solarenergy { get; set; }

        [JsonPropertyName("uvindex")]
        public double? Uvindex { get; set; }

        [JsonPropertyName("severerisk")]
        public double? Severerisk { get; set; }

        [JsonPropertyName("conditions")]
        public string Conditions { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("stations")]
        public List<object> Stations { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("sunrise")]
        public string Sunrise { get; set; }

        [JsonPropertyName("sunriseEpoch")]
        public int? SunriseEpoch { get; set; }

        [JsonPropertyName("sunset")]
        public string Sunset { get; set; }

        [JsonPropertyName("sunsetEpoch")]
        public int? SunsetEpoch { get; set; }

        [JsonPropertyName("moonphase")]
        public double? Moonphase { get; set; }
    }

    /// <summary>
    /// Model that gets returned from VisualCrossing Weather API.
    /// </summary>
    public class WeatherApiDay
    {
        [JsonPropertyName("datetime")]
        public string Datetime { get; set; }

        [JsonPropertyName("datetimeEpoch")]
        public int? DatetimeEpoch { get; set; }

        [JsonPropertyName("tempmax")]
        public double? Tempmax { get; set; }

        [JsonPropertyName("tempmin")]
        public double? Tempmin { get; set; }

        [JsonPropertyName("temp")]
        public double? Temp { get; set; }

        [JsonPropertyName("feelslikemax")]
        public double? Feelslikemax { get; set; }

        [JsonPropertyName("feelslikemin")]
        public double? Feelslikemin { get; set; }

        [JsonPropertyName("feelslike")]
        public double? Feelslike { get; set; }

        [JsonPropertyName("dew")]
        public double? Dew { get; set; }

        [JsonPropertyName("humidity")]
        public double? Humidity { get; set; }

        [JsonPropertyName("precip")]
        public double? Precip { get; set; }

        [JsonPropertyName("precipprob")]
        public double? Precipprob { get; set; }

        [JsonPropertyName("precipcover")]
        public double? Precipcover { get; set; }

        [JsonPropertyName("preciptype")]
        public object Preciptype { get; set; }

        [JsonPropertyName("snow")]
        public double? Snow { get; set; }

        [JsonPropertyName("snowdepth")]
        public double? Snowdepth { get; set; }

        [JsonPropertyName("windgust")]
        public double? Windgust { get; set; }

        [JsonPropertyName("windspeed")]
        public double? Windspeed { get; set; }

        [JsonPropertyName("winddir")]
        public double? Winddir { get; set; }

        [JsonPropertyName("pressure")]
        public double? Pressure { get; set; }

        [JsonPropertyName("cloudcover")]
        public double? Cloudcover { get; set; }

        [JsonPropertyName("visibility")]
        public double? Visibility { get; set; }

        [JsonPropertyName("solarradiation")]
        public double? Solarradiation { get; set; }

        [JsonPropertyName("solarenergy")]
        public double? Solarenergy { get; set; }

        [JsonPropertyName("uvindex")]
        public double? Uvindex { get; set; }

        [JsonPropertyName("severerisk")]
        public double? Severerisk { get; set; }

        [JsonPropertyName("sunrise")]
        public string Sunrise { get; set; }

        [JsonPropertyName("sunriseEpoch")]
        public int? SunriseEpoch { get; set; }

        [JsonPropertyName("sunset")]
        public string Sunset { get; set; }

        [JsonPropertyName("sunsetEpoch")]
        public int? SunsetEpoch { get; set; }

        [JsonPropertyName("moonphase")]
        public double? Moonphase { get; set; }

        [JsonPropertyName("conditions")]
        public string Conditions { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("stations")]
        public List<string> Stations { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("hours")]
        public List<WeatherApiHour> Hours { get; set; }
    }

    /// <summary>
    /// Model that gets returned from VisualCrossing Weather API.
    /// </summary>
    public class WeatherApiHour
    {
        [JsonPropertyName("datetime")]
        public string Datetime { get; set; }

        [JsonPropertyName("datetimeEpoch")]
        public int? DatetimeEpoch { get; set; }

        [JsonPropertyName("temp")]
        public double? Temp { get; set; }

        [JsonPropertyName("feelslike")]
        public double? Feelslike { get; set; }

        [JsonPropertyName("humidity")]
        public double? Humidity { get; set; }

        [JsonPropertyName("dew")]
        public double? Dew { get; set; }

        [JsonPropertyName("precip")]
        public double? Precip { get; set; }

        [JsonPropertyName("precipprob")]
        public double? Precipprob { get; set; }

        [JsonPropertyName("snow")]
        public double? Snow { get; set; }

        [JsonPropertyName("snowdepth")]
        public double? Snowdepth { get; set; }

        [JsonPropertyName("preciptype")]
        public object Preciptype { get; set; }

        [JsonPropertyName("windgust")]
        public double? Windgust { get; set; }

        [JsonPropertyName("windspeed")]
        public double? Windspeed { get; set; }

        [JsonPropertyName("winddir")]
        public double? Winddir { get; set; }

        [JsonPropertyName("pressure")]
        public double? Pressure { get; set; }

        [JsonPropertyName("visibility")]
        public double? Visibility { get; set; }

        [JsonPropertyName("cloudcover")]
        public double? Cloudcover { get; set; }

        [JsonPropertyName("solarradiation")]
        public double? Solarradiation { get; set; }

        [JsonPropertyName("solarenergy")]
        public double? Solarenergy { get; set; }

        [JsonPropertyName("uvindex")]
        public double? Uvindex { get; set; }

        [JsonPropertyName("severerisk")]
        public double? Severerisk { get; set; }

        [JsonPropertyName("conditions")]
        public string Conditions { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("stations")]
        public List<string> Stations { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("sunrise")]
        public string Sunrise { get; set; }

        [JsonPropertyName("sunriseEpoch")]
        public int? SunriseEpoch { get; set; }

        [JsonPropertyName("sunset")]
        public string Sunset { get; set; }

        [JsonPropertyName("sunsetEpoch")]
        public int? SunsetEpoch { get; set; }

        [JsonPropertyName("moonphase")]
        public double? Moonphase { get; set; }
    }
}
