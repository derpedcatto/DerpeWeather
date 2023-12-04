using DerpeWeather.Utilities.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace DerpeWeather.Utilities.Helpers.UserData
{
    public class UserDirHelper : IUserDirHelper
    {
        public string? GetUserDir(Guid userId)
        {
            try
            {
                string folderPath = Path.Combine(App.AppDataRootFolderPath, App.UserDataFolderName, userId.ToString());
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
