using DerpeWeather.Utilities.Enums;

namespace DerpeWeather.MVVM.Models
{
    /// <summary>
    /// Model that represents User App Preferences.
    /// </summary>
    public class UserAppPreferences
    {
        public UserPreferenceTheme Theme { get; set; }
        public UserPreferenceUnits Units { get; set; }
    }
}
