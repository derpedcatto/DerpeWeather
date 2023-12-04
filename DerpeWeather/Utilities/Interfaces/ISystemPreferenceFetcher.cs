using DerpeWeather.Utilities.Enums;

namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// Fetches system preferences.
    /// </summary>
    public interface ISystemPreferenceFetcher
    {
        /// <summary>
        /// Fetches system preference of temperature units (ex: Metric, US, UK, ...).
        /// </summary>
        UserPreferenceUnits GetUnitsPreference();

        /// <summary>
        /// Fetches system preference of theme (ex: Light, Dark, System, ...).
        /// </summary>
        UserPreferenceTheme GetThemePreference();
    }
}
