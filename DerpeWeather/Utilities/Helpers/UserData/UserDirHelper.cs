using DerpeWeather.Utilities.Interfaces;
using System;
using System.IO;

namespace DerpeWeather.Utilities.Helpers.UserData
{
    /// <summary>
    /// Main implementation for <see cref="IUserDirHelper"/>.
    /// </summary>
    public class UserDirHelper : IUserDirHelper
    {
        public string? GetUserDir(Guid userId)
        {
            try
            {
                string folderPath = Path.Combine(App.AppDataRootFolderPath, App.RootUserDataFolderName, userId.ToString());
                Directory.CreateDirectory(folderPath);
                return folderPath;
            }
            catch (IOException ex)
            {
                AdonisUI.Controls.MessageBox.Show(
                    ex.Message,
                    "GetUserDataFolder error!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
            }

            return null;
        }
    }
}
