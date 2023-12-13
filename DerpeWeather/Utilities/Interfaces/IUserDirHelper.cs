namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// Helper class to work with AppData app data folder.
    /// </summary>
    public interface IUserDirHelper
    {
        /// <param name="userId">Database User GUID.</param>
        /// <returns>
        /// On success - full string path to AppData app UserData user folder.
        /// On fail - null.
        /// </returns>
        string? GetUserDir(System.Guid userId);
    }
}
