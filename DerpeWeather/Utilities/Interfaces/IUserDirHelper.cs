namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// Helper class to work with AppData app data folder.
    /// </summary>
    public interface IUserDirHelper
    {
        /// <param name="userId">Database User GUID.</param>
        /// <returns>
        /// <para>On success - full string path to AppData app UserData user folder.</para>
        /// <para>On fail - null.</para>
        /// </returns>
        string? GetUserDir(System.Guid userId);
    }
}
