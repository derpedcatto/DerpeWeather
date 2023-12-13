using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.Views;
using DerpeWeather.Utilities.Enums;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DerpeWeather.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="UserPreferencesWindow"/>.
    /// </summary>
    public partial class UserPreferencesVM : ObservableObject, IDisposable
    {
        #region Variables

        private readonly CancellationTokenSource _cts;
        private bool disposedValue;

        private readonly IUserRepo _userRepo;
        private readonly IMessenger _messenger;
        private readonly IAvatarImageManager _avatarImageManager;
        private readonly IWindowFactory _windowFactory;

        private Guid _userId;

        [ObservableProperty]
        private bool _ButtonsEnabled;

        [ObservableProperty]
        private string _Username;
        [ObservableProperty]
        private BitmapImage _AvatarImage;

        [ObservableProperty]
        private UserPreferenceTheme _UserTheme;
        [ObservableProperty]
        private UserPreferenceUnits _UserUnits;
        
        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserPreferencesVM(
            IUserRepo userRepo,
            IMessenger messenger,
            IAvatarImageManager avatarImageManager,
            IWindowFactory windowFactory)
        {
            _cts = new();

            _userRepo = userRepo;
            _messenger = messenger;
            _avatarImageManager = avatarImageManager;
            _windowFactory = windowFactory;

            _messenger.Register<PrefWndUserIdMsg>(this, OnPreferencesWindowPassUserMsgReceived);

            ButtonsEnabled = true;
        }



        /// <summary>
        /// Updates user data in view.
        /// </summary>
        private void UpdateUserDisplayData()
        {
            var user = _userRepo.GetUser(_userId);
            Username = user.Username;

            AvatarImage = new();
            AvatarImage.BeginInit();
            AvatarImage.UriSource = new(_avatarImageManager.GetUserAvatar(_userId));
            AvatarImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            AvatarImage.CacheOption = BitmapCacheOption.OnLoad;
            AvatarImage.EndInit();
        }



        #region RelayCommands

        [RelayCommand]
        private void ChangeUsernameClick()
        {
            ButtonsStateSwitch(true);

            var changeUsernameWindow = _windowFactory.CreateWindow<ChangeUsernameWindow>(App.Current.MainWindow);
            _messenger.Send(new ChangeUsernameWndUserIdMsg(_userId));
            
            changeUsernameWindow.ShowDialog();
            if (changeUsernameWindow.IsUsernameChangeSuccess)
            {
                AdonisUI.Controls.MessageBox.Show(
                    "Success!",
                    "Username changed successfully!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Information
                );

                UpdateUserDisplayData();
            }

            ButtonsStateSwitch(false);
        }

        [RelayCommand]
        private void ChangePasswordClick()
        {
            ButtonsStateSwitch(true);

            var changePasswordWindow = _windowFactory.CreateWindow<ChangePasswordWindow>(App.Current.MainWindow);
            _messenger.Send(new ChangePasswordWndUserIdMsg(_userId));

            changePasswordWindow.ShowDialog();

            if (changePasswordWindow.IsPasswordChangeSuccess)
            {
                AdonisUI.Controls.MessageBox.Show(
                    "Success!",
                    "Password changed successfully!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Information
                );
            }

            ButtonsStateSwitch(false);
        }

        [RelayCommand]
        private void ChangeAvatarClick()
        {
            ButtonsStateSwitch(true);

            var changeAvatarWindow = _windowFactory.CreateWindow<ChangeAvatarWindow>(App.Current.MainWindow);
            _messenger.Send(new ChangeAvatarWndUserIdMsg(_userId));

            changeAvatarWindow.ShowDialog();

            if (changeAvatarWindow.IsAvatarChangeSuccess)
            {
                AdonisUI.Controls.MessageBox.Show(
                    "Success!",
                    "Avatar changed successfully!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Information
                );

                UpdateUserDisplayData();
            }

            ButtonsStateSwitch(false);
        }

        [RelayCommand]
        private async Task UserThemeChanged(UserPreferenceTheme theme)
        {
            ButtonsStateSwitch(true);

            await _userRepo.UpdateUserThemeAsync(_userId, UserTheme, _cts.Token);

            App.Current.SwitchColorScheme(theme);

            ButtonsStateSwitch(false);
        }

        [RelayCommand]
        private async Task UserUnitsChanged(UserPreferenceUnits units)
        {
            ButtonsStateSwitch(true);

            await _userRepo.UpdateUserUnitsAsync(_userId, UserUnits, _cts.Token);

            ButtonsStateSwitch(false);
        }

        #endregion



        private void UserPreferencesVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(UserTheme):
                    UserThemeChanged(UserTheme);
                    break;
                case nameof(UserUnits):
                    UserUnitsChanged(UserUnits);
                    break;
            }
        }



        private void OnPreferencesWindowPassUserMsgReceived(object recipient, PrefWndUserIdMsg message)
        {
            _userId = message.Value;

            var user = _userRepo.GetUser(_userId);
            UserTheme = user.Preferences.Theme;
            UserUnits = user.Preferences.Units;

            PropertyChanged += UserPreferencesVM_PropertyChanged;

            UpdateUserDisplayData();
        }



        private void ButtonsStateSwitch(bool lockButtons)
        {
            if (lockButtons)
            {
                ButtonsEnabled = false;
            }
            else
            {
                ButtonsEnabled = true;
            }
        }



        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _messenger.UnregisterAll(this);
                    _cts.Cancel();
                    _cts.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
