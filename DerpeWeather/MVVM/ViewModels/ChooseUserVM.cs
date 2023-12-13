using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.DAL.DTO;
using DerpeWeather.MVVM.Models;
using DerpeWeather.MVVM.Views;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DerpeWeather.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="ChooseUserWindow"/>.
    /// </summary>
    public partial class ChooseUserVM : ObservableObject, IDisposable
    {
        #region Variables

        private readonly CancellationTokenSource _cts;
        private bool disposedValue;

        private readonly IMessenger _messenger;
        private readonly IAvatarImageManager _avatarImageManager;
        private readonly IUserRepo _userRepo;
        private readonly IWindowFactory _windowFactory;
        private readonly ISystemPreferenceFetcher _systemPreferenceFetcher;

        [ObservableProperty]
        private Visibility _LoadingIndicatorVisibility;

        [ObservableProperty]
        private bool _ButtonsEnabled;

        /// <summary>
        /// User list from DB to display in view.
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<UserLoginListItem> _UsersList;

        /// <summary>
        /// Selected user in list.
        /// </summary>
        [ObservableProperty]
        private UserLoginListItem? _SelectedUser;

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChooseUserVM(
            IUserRepo userRepo,
            IMessenger messenger,
            IAvatarImageManager avatarImageManager,
            IWindowFactory windowFactory,
            ISystemPreferenceFetcher systemPreferenceFetcher)
        {
            _cts = new();

            LoadingIndicatorVisibility = Visibility.Collapsed;
            ButtonsEnabled = true;

            _userRepo = userRepo;
            _messenger = messenger;
            _avatarImageManager = avatarImageManager;
            _windowFactory = windowFactory;
            _systemPreferenceFetcher = systemPreferenceFetcher;

            UsersList = new();
            UpdateUsersList();

            _messenger.Register<ValidAppJsonSettingsMsg>(this, OnValidAppJsonSettingsMsgReceived);

            App.Current.SwitchColorScheme(_systemPreferenceFetcher.GetThemePreference());
        }



        #region RelayCommand

        /// <summary>
        /// <para>On success - sends a <see cref="UserLoginIdMsg"/> with selected UserID and <see cref="CloseChooseUserWndMsg"/></para>
        /// <para>On fail - displays a Message Box.</para>
        /// </summary>
        [RelayCommand]
        private async Task LoginClick()
        {
            if (SelectedUser == null)
            {
                AdonisUI.Controls.MessageBox.Show(
                    "User is not chosen!",
                    "Login Error",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );

                return;
            }

            try
            {
                var userLoginDTO = new UserDisplayInfoDTO()
                {
                    Username = SelectedUser.Username,
                    AvatarPath = SelectedUser.AvatarPath
                };

                var userLoginWindow = _windowFactory.CreateWindow<UserLoginWindow>(App.Current.MainWindow);
                _messenger.Send(new UserLoginDTOMsg(userLoginDTO));

                userLoginWindow.ShowDialog();

                if (userLoginWindow.IsLoginSuccessful)
                {
                    var user = await _userRepo.GetUserAsync(SelectedUser.Username, _cts.Token);
                    var userId = user.Id;

                    var mainWindow = _windowFactory.CreateWindow<MainWindow>(App.Current.MainWindow);
                    mainWindow.Show();

                    _messenger.Send(new UserLoginIdMsg(userId));
                    _messenger.Send<CloseChooseUserWndMsg>();
                }

                userLoginWindow.Dispose();
            }
            catch (OperationCanceledException) 
            {
                
            }
            catch (Exception ex)
            {
                AdonisUI.Controls.MessageBox.Show(
                    ex.Message,
                    "Login Error",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
            }
        }

        /// <summary>
        /// <para>Displays modal <see cref="UserRegistrationWindow"/> and prompts user to enter data for registration.</para>
        /// <para>On success - modal window saves user to database and this function calls <see cref="UpdateUsersList"/>.</para>
        /// <para>On fail - does nothing.</para>
        /// </summary>
        [RelayCommand]
        private void RegisterNewUserClick()
        {
            var userRegistrationWindow = _windowFactory.CreateWindow<UserRegistrationWindow>(App.Current.MainWindow);
            userRegistrationWindow.ShowDialog();
            
            if (userRegistrationWindow.IsRegistrationSuccessful)
            {
                Application.Current.Dispatcher.InvokeAsync(() => 
                    AdonisUI.Controls.MessageBox.Show(
                        "Registration successful!",
                        "Success!",
                        AdonisUI.Controls.MessageBoxButton.OK,
                        AdonisUI.Controls.MessageBoxImage.Information
                    )
                );

                UpdateUsersList();
            }

            userRegistrationWindow.Dispose();
        }

        /// <summary>
        /// Displays modal <see cref="UserLoginWindow"/> that prompts user to enter password.
        /// On success - deletes a user from database and <see cref="UsersList"/>.
        /// On fail - does nothing.
        /// </summary>
        [RelayCommand]
        private async Task DeleteSelectedUserClick()
        {
            if (SelectedUser == null)
            {
                AdonisUI.Controls.MessageBox.Show(
                    "User is not chosen!",
                    "Login Error",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );

                return;
            }

            try
            {
                var userLoginDTO = new UserDisplayInfoDTO()
                {
                    Username = SelectedUser.Username,
                    AvatarPath = SelectedUser.AvatarPath
                };

                var userLoginWindow = _windowFactory.CreateWindow<UserLoginWindow>(App.Current.MainWindow);
                _messenger.Send(new UserLoginDTOMsg(userLoginDTO));

                userLoginWindow.ShowDialog();

                if (userLoginWindow.IsLoginSuccessful == true)
                {
                    await _userRepo.DeleteUserAsync(SelectedUser.Username, _cts.Token);
                    UsersList.Remove(SelectedUser);
                    SelectedUser = null;
                }

                userLoginWindow.Dispose();
            }
            catch (OperationCanceledException)
            {

            }
            catch ( Exception ex )
            {
                AdonisUI.Controls.MessageBox.Show(
                    ex.Message,
                    "Login Error",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
            }
        }

        /// <summary>
        /// Displays a modal <see cref="AppJsonSettingsWindow"/> that prompts to change app data.
        /// On success - calls <see cref="OnValidAppJsonSettingsMsgReceived(object, ValidAppJsonSettingsMsg)"/>.
        /// On fail - does nothing.
        /// </summary>
        [RelayCommand]
        private void EditAppSettingsClick()
        {
            var appJsonSettingsWindow = _windowFactory.CreateWindow<AppJsonSettingsWindow>(App.Current.MainWindow);
            appJsonSettingsWindow.ShowDialog();

            // OnValidAppJsonSettingsMsgReceived is called.
        }

        #endregion



        #region ListView list functions

        /// <summary>
        /// Updates <see cref="UsersList"/> with all database users.
        /// </summary>
        private async Task LoadUsersListFromDbAsync()
        {
            try
            {
                var users = await _userRepo.GetAllUsersAsync(_cts.Token);
                if (users != null)
                {
                    var tempList = new List<UserLoginListItem>();
                    foreach (var user in users)
                    {
                        tempList.Add(new UserLoginListItem()
                        {
                            Username = user.Username,
                            AvatarPath = _avatarImageManager.GetUserAvatar(user.Id)!
                        });
                    }
                    UsersList = new(tempList);
                }
            }
            catch ( OperationCanceledException )
            {

            }
            catch (Exception ex )
            {
                AdonisUI.Controls.MessageBox.Show(
                    "LoadUsersListFromDb error",
                    ex.Message,
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Information
                );

                return;
            }
        }

        /// <summary>
        /// Clears <see cref="UsersList"/> and refreshes all it's data with <see cref="LoadUsersListFromDbAsync"/> function.
        /// </summary>
        private async void UpdateUsersList()
        {
            SwitchLoadingIndicatorState(true);

            UsersList.Clear();
            await LoadUsersListFromDbAsync();

            SwitchLoadingIndicatorState(false);
        }

        #endregion



        private void OnValidAppJsonSettingsMsgReceived(object recipient, ValidAppJsonSettingsMsg message)
        {
            _messenger.Send(new CloseChooseUserWndMsg());
        }



        private void SwitchLoadingIndicatorState(bool isLoading)
        {
            if (isLoading)
            {
                ButtonsEnabled = false;
                LoadingIndicatorVisibility = Visibility.Visible;
            }
            else
            {
                LoadingIndicatorVisibility = Visibility.Collapsed;
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
                UsersList = null;
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
