using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.DAL.DTO;
using DerpeWeather.MVVM.Models;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DerpeWeather.ViewModels
{
    public partial class ChooseUserVM : ObservableObject, IDisposable
    {
        private readonly UserLoginWindow _userLoginWindow;
        private readonly UserRegistrationWindow _userRegistrationWindow;

        private readonly IWindowService _windowService;
        private readonly IMessenger _messenger;
        private readonly IAvatarImageManager _avatarImageManager;
        private readonly IUserRepo _userRepo;

        /// <summary>
        /// User list from DB to display in view.
        /// </summary>
        public ObservableCollection<UserLoginListItem> UsersList
        {
            get => _usersList;
            set => SetProperty(ref _usersList, value);
        }
        private ObservableCollection<UserLoginListItem> _usersList;

        /// <summary>
        /// Selected user in list.
        /// </summary>
        [ObservableProperty]
        private UserLoginListItem _SelectedUser;
        private bool disposedValue;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChooseUserVM(
            IUserRepo userRepo,
            IMessenger messenger,
            IAvatarImageManager avatarImageManager,
            UserLoginWindow userLoginWindow,
            UserRegistrationWindow userRegistrationWindow,
            IWindowService windowService)
        {
            _userRepo = userRepo;
            _messenger = messenger;
            _avatarImageManager = avatarImageManager;
            _userRegistrationWindow = userRegistrationWindow;
            _windowService = windowService;

            _userLoginWindow = userLoginWindow;
            _userRegistrationWindow = userRegistrationWindow;

            UsersList = new();
            UpdateUsersList();
        }



        #region RelayCommand

        /// <summary>
        /// <para>Logic for clicking 'Login' button.</para>
        /// <para>On success - sends a message to <see cref="ChooseUserWindow"/> with selected UserID and triggers <see cref="ChooseUserWindow.OnOpenMainWindow(object, OpenMainWindowMsg)"/>. </para>
        /// <para>On fail - displays a MessageBox.</para>
        /// </summary>
        [RelayCommand]
        private void LoginClick()
        {
            if (SelectedUser == null)
            {
                AdonisUI.Controls.MessageBox.Show(
                    "User is not chosen!",
                    "Login Error",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
            }
            else
            {
                var userLoginDTO = new UserLoginDTO()
                {
                    Username = SelectedUser.Username,
                    AvatarPath = SelectedUser.AvatarPath
                };

                _messenger.Send(new UserLoginDTOMsg(userLoginDTO));

                var loginSuccess = _userLoginWindow.ShowDialog();
                if (loginSuccess == true)
                {
                    // _messenger.Send(new OpenMainWindowMsg(_userRepo.GetUser(SelectedUser.Username)!.Id));
                    _messenger.Send(new UserLoginIdMsg(_userRepo.GetUser(SelectedUser.Username)!.Id));
                    _windowService.ShowWindow<MainWindow>();
                    // _userLoginWindow.Close();
                }
            }
        }

        /// <summary>
        /// <para>Logic for clicking 'Register' button.</para>
        /// <para>Displays modal <see cref="UserRegistrationWindow"/> and prompts user to enter data for registration.</para>
        /// <para>On success - modal window saves user to database and this function calls <see cref="UpdateUsersList"/>.</para>
        /// <para>On fail - does nothing.</para>
        /// </summary>
        [RelayCommand]
        private void RegisterNewUserClick()
        {
            var result = _userRegistrationWindow.ShowDialog();
            if (result == true)
            {
                AdonisUI.Controls.MessageBox.Show(
                    "Registration successful!",
                    "Success!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Information
                );

                UpdateUsersList();
            }
        }

        [RelayCommand]
        private void DeleteSelectedUserClick()
        {
            if (SelectedUser == null)
            {
                AdonisUI.Controls.MessageBox.Show(
                    "User is not chosen!",
                    "Login Error",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
            }
            else
            {
                var userLoginDTO = new UserLoginDTO()
                {
                    Username = SelectedUser.Username,
                    AvatarPath = SelectedUser.AvatarPath
                };

                _messenger.Send(new UserLoginDTOMsg(userLoginDTO));

                var loginSuccess = _userLoginWindow.ShowDialog();
                if (loginSuccess == true)
                {
                    _userRepo.DeleteUser(SelectedUser.Username);
                    UpdateUsersList();
                }
            }
        }

        #endregion



        #region ListView list functions

        /// <summary>
        /// Updates <see cref="UsersList"/> with all database users.
        /// </summary>
        private async Task LoadUsersListFromDbAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();
            if (users != null)
            {
                foreach (var user in _userRepo.GetAllUsers())
                {
                    UsersList.Add(new UserLoginListItem()
                    {
                        Username = user.Username,
                        AvatarPath = _avatarImageManager.GetUserAvatar(user.Id)!
                    });
                }
            }
        }

        /// <summary>
        /// Clears <see cref="UsersList"/> and refreshes all it's data with <see cref="LoadUsersListFromDbAsync"/> function.
        /// </summary>
        private async void UpdateUsersList()
        {
            UsersList.Clear();
            await LoadUsersListFromDbAsync();
        }

        #endregion



        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _messenger.UnregisterAll(this);
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
