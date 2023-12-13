using DerpeWeather.Utilities.Interfaces;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DerpeWeather.Utilities.Helpers.UserData
{
    public class AvatarImageManager : IAvatarImageManager
    {
        private readonly IUserDirHelper _userDirHelper;



        /// <summary>
        /// Default constructor.
        /// </summary>
        public AvatarImageManager(IUserDirHelper userDirHelper)
        {
            _userDirHelper = userDirHelper;
        }



        public string? GetUserAvatar(Guid userId)
        {
            try
            {
                string avatarSearchPattern = App.UserAvatarFileName + ".*";
                string? userFolderPath = _userDirHelper.GetUserDir(userId);
                string? avatarPath = Directory.GetFiles(userFolderPath, avatarSearchPattern).FirstOrDefault();

                if (avatarPath == null)
                {
                    SetDefaultAvatar(userId);
                    avatarPath = Directory.GetFiles(userFolderPath, avatarSearchPattern).FirstOrDefault();

                    AdonisUI.Controls.MessageBox.Show(
                        "User avatar is missing! Setting default avatar in place.",
                        "GetUserAvatar Warning",
                        AdonisUI.Controls.MessageBoxButton.OK,
                        AdonisUI.Controls.MessageBoxImage.Exclamation
                    );

                    SetDefaultAvatar(userId);
                }

                return Path.Combine(userFolderPath, avatarPath!);
            }
            catch (IOException ex)
            {
                AdonisUI.Controls.MessageBox.Show(
                    ex.Message,
                    "GetUserAvatar error!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
                return null;
            }
        }

        public string? PromptFileDialog()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;",
                FilterIndex = 1,
                Multiselect = false
            };

            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        public void CopyAvatarToUserFolder(Guid userId, string avatarPath)
        {
            string? userDir = _userDirHelper.GetUserDir(userId);
            if (userDir == null) { return; }

            DeleteAvatarFiles(userDir);

            string imageExtension = Path.GetExtension(avatarPath);
            string newAvatarPath = Path.Combine(userDir, $"{App.UserAvatarFileName}{Path.GetExtension(imageExtension)}");

            File.Copy(avatarPath, newAvatarPath);
        }

        public void SetDefaultAvatar(Guid userId)
        {
            string? userDir = _userDirHelper.GetUserDir(userId);
            if (userDir == null) { return; }

            DeleteAvatarFiles(userDir);

            string newAvatarPath = Path.Combine(userDir, $"{App.UserAvatarFileName}.png");

            using Bitmap tempBitmap = new(App.DefaultUserAvatarImage);
            tempBitmap.Save(newAvatarPath, System.Drawing.Imaging.ImageFormat.Png);
        }



        /// <summary>
        /// Deletes leftover avatar image files from user data folder.
        /// </summary>
        /// <param name="userDir">Full path to AppData app UserData user folder.</param>
        private void DeleteAvatarFiles(string userDir)
        {
            string[] filesToDelete = Directory.GetFiles(userDir, $"{App.UserAvatarFileName}.*");
            foreach (string file in filesToDelete)
            {
                File.Delete(file);
            }
        }
    }
}
