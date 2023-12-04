using DerpeWeather.Utilities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DerpeWeather.DAL.EF
{
    public class UserAppPreferencesEntity
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public UserPreferenceTheme Theme { get; set; }
        public UserPreferenceUnits Units { get; set; }

        // Nav
        public UserEntity User { get; set; }
    }
}
