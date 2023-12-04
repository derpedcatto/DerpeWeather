using System;
using System.Collections.Generic;

namespace DerpeWeather.MVVM.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public UserAppPreferences Preferences { get; set; }
        public List<UserTrackedWeatherField> TrackedWeatherFields { get; set; }
    }
}
