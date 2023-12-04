using DerpeWeather.MVVM.Models;
using System;
using System.Collections.Generic;

namespace DerpeWeather.DAL.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public UserAppPreferences Preferences { get; set; }
        public List<UserTrackedWeatherField> TrackedWeatherFields { get; set; }
    }
}
