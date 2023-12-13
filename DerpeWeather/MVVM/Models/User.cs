using System;
using System.Collections.Generic;

namespace DerpeWeather.MVVM.Models
{
    /// <summary>
    /// Model and represents User database model (with some changes) 
    /// and also that gets returned from <see cref="IUserRepo"/> .
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public UserAppPreferences Preferences { get; set; }
        public List<UserTrackedWeatherField> TrackedWeatherFields { get; set; }
    }
}
