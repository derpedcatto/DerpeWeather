using System;

namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// Validates user inputted data.
    /// </summary>
    public interface IUserInputValidator
    {
        /// <summary>
        /// Validates given username (including if it is duplicate in DB).
        /// </summary>
        /// <returns>
        /// On success - empty string, On fail - string error message.
        /// </returns>
        string CheckUsername(string username);

        /// <summary>
        /// Validates given password.
        /// </summary>
        /// <returns>
        /// On success - empty string. On fail - string error message.
        /// </returns>
        string CheckPassword(System.Security.SecureString password);

        /// <summary>
        /// Validates given string path to a user avatar.
        /// </summary>
        /// <returns>On success - empty string. On fail - string error message.
        /// </returns>
        string CheckAvatar(string filePath);

        /// <summary>
        /// Validates given location name string.
        /// </summary>
        /// <param name="resolvedLocation">Resolved location name (city name, ...).</param>
        /// <param name="userId">User DB ID (to check for duplicate location name).</param>
        /// <returns>
        /// On success - empty string.
        /// On fail - string error message.
        /// </returns>
        string CheckLocationName(Guid userId, string locationName, string resolvedLocation);
    }
}
