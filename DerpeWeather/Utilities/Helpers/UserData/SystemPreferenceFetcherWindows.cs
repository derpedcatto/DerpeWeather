using DerpeWeather.Utilities.Enums;
using DerpeWeather.Utilities.Interfaces;
using Microsoft.Win32;
using System.Globalization;

namespace DerpeWeather.Utilities.Helpers.UserData
{
    public class SystemPreferenceFetcherWindows : ISystemPreferenceFetcher
    {
        public UserPreferenceUnits GetUnitsPreference()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            RegionInfo regionInfo = new(currentCulture.LCID);

            // You might need to customize this logic based on the regions and their unit preferences
            return regionInfo.TwoLetterISORegionName switch
            {
                "US" => UserPreferenceUnits.US,
                "GB" => UserPreferenceUnits.UK,
                _ => UserPreferenceUnits.METRIC,
            };
        }

        public UserPreferenceTheme GetThemePreference()
        {
            const string keyName = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            const string valueName = "AppsUseLightTheme";

            int registryValue = (int)Registry.GetValue(keyName, valueName, -1);

            if (registryValue == 0)
            {
                return UserPreferenceTheme.DARK;
            }
            else if (registryValue == 1)
            {
                return UserPreferenceTheme.LIGHT;
            }
            else
            {
                return UserPreferenceTheme.DARK;
            }
        }
    }
}
