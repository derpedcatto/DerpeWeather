using System.Security;

namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// Hashing service.
    /// </summary>
    public interface IHashService
    {
        /// <summary>
        /// Converts given string to a hashed string.
        /// </summary>
        /// <param name="source">Original string.</param>
        /// <returns>Hashed string.</returns>
        string HashString(string source);

        /// <summary>
        /// Converts given SecureString to a hashed normal string.
        /// </summary>
        /// <param name="source">Original SecureString.</param>
        /// <returns>Hashed string.</returns>
        string HashString(SecureString source);
    }
}
