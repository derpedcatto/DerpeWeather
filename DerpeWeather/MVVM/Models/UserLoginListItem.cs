using CommunityToolkit.Mvvm.ComponentModel;

namespace DerpeWeather.MVVM.Models
{
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
            set => SetProperty(ref _avatarPath, value);
        }
    }
}
