using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Imaging;
using System;

namespace DerpeWeather.MVVM.Models
{
    /// <summary>
    /// Model that is used to display users in list in <see cref="ChooseUserWindow"/> view.
    /// </summary>
    public class UserLoginListItem : ObservableObject
    {
        private string _username;
        private string _avatarPath;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string AvatarPath
        {
            get => _avatarPath;
            set
            {
                _avatarPath = value;
                AvatarImage = LoadImage(_avatarPath);
            }
        }

        public BitmapImage AvatarImage { get; private set; }

        private BitmapImage LoadImage(string path)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
