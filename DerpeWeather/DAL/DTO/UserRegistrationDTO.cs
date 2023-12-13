namespace DerpeWeather.DAL.DTO
{
    /// <summary>
    /// Data Transfer model that represents all data for a successful
    /// User registration in DB.
    /// </summary>
    public class UserRegistrationDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
