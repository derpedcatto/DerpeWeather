using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DerpeWeather.DAL.EF
{
    /// <summary>
    /// Entity Framework User entity.
    /// </summary>
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Nav
        public UserAppPreferencesEntity AppPreferences { get; set; }
        public ICollection<UserTrackedWeatherFieldsEntity> TrackedWeatherFields { get; set; }
    }
}
