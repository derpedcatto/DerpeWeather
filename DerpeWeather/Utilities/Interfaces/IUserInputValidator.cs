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
        /// <para>On success - empty string.</para>
        /// <para>On fail - string error message.</para>
        /// </returns>
        string CheckUsername(string username);

        /// <summary>
        /// Validates given password.
        /// </summary>
        /// <returns>
        /// <para>On success - empty string.</para>
        /// <para>On fail - string error message.</para>
        /// </returns>
        string CheckPassword(System.Security.SecureString password);

        /// <summary>
        /// Validates given path to user avatar.
        /// </summary>
        /// <returns>
        /// <para>On success - empty string.</para>
        /// <para>On fail - string error message.</para>
        /// </returns>
        string CheckAvatar(string filePath);

        /// <summary>
        /// Validates given location string.
        /// </summary>
        /// <param name="location">Location name (city name, ...).</param>
        /// <param name="userId">User DB ID (to check for duplicate location name).</param>
        /// <returns>
        /// <para>On success - empty string.</para>
        /// <para>On fail - string error message.</para>
        /// </returns>
        string CheckLocationName(Guid userId, string location);
    }
}
