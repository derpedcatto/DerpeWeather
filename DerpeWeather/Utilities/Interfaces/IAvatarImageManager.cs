namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// Interface to work with User avatar images that are stored in AppData.
    /// </summary>
    public interface IAvatarImageManager
    {
        /// <summary>
        /// Prompts user (through a file dialog) to pick a file on his system that he wants to use as an avatar.
        /// </summary>
        /// <returns>
        /// <para>On fail - null.</para>
        /// <para>On success - string full path to desired image.</para>
        /// </returns>
        string? PromptFileDialog();

        /// <param name="userId">Database User GUID.</param>
        /// <returns>
        /// <para>On success - String path to user avatar stored in AppData app UserData folder (if avatar doesn't exist - places default avatar in place of it)</para> 
        /// <para>On fail - displays an error MessageBox and returns null.</para>
        /// </returns>
        string? GetUserAvatar(System.Guid userId);

        /// <summary>
        /// Copies avatar image file from <paramref name="avatarPath"/> to AppData app UserData folder.
        /// </summary>
        /// <param name="userId">Database User GUID.</param>
        /// <param name="avatarPath">Path to image avatar file.</param>
        void CopyAvatarToUserFolder(System.Guid userId, string avatarPath);

        /// <summary>
        /// Sets default avatar to user. Default avatar file is saved in AppData app UserData folder.
        /// </summary>
        /// <param name="userId">Database User GUID.</param>
        void SetDefaultAvatar(System.Guid userId);
    }
}
