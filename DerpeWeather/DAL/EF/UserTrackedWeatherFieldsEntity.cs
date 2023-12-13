using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DerpeWeather.DAL.EF
{
    /// <summary>
    /// Entity Framework UserTrackedWeatherFields entity.
    /// </summary>
    public class UserTrackedWeatherFieldsEntity
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public string Location { get; set; }
        public string ResolvedLocation { get; set; }

        // Nav
        public UserEntity User { get; set; }
    }
}