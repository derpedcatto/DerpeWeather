using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DerpeWeather.DAL.EF
{
    public class UserTrackedWeatherFieldsEntity
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public string Location { get; set; }

        // Nav
        public UserEntity User { get; set; }
    }
}