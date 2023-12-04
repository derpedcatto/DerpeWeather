using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.DAL.DTO;
using DerpeWeather.MVVM.Models;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace DerpeWeather.ViewModels
{
    public partial class ChooseUserVM : ObservableObject, IDisposable
    {
        #region Variables

        private bool disposedValue;
        private readonly CancellationTokenSource _cts;

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

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChooseUserVM(
            IUserRepo userRepo,
            IMessenger messenger,
            IAvatarImageManager avatarImageManager)
        {
            _cts = new();

            _userRepo = userRepo;
            _messenger = messenger;
            _avatarImageManager = avatarImageManager;

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
                var userLoginDTO = new UserLoginDTO()
                {
                    Username = SelectedUser.Username,
                    AvatarPath = SelectedUser.AvatarPath
                };

                var userLoginWindow = App.Current.Services.GetRequiredService<UserLoginWindow>();
                _messenger.Send(new UserLoginDTOMsg(userLoginDTO));

                userLoginWindow.ShowDialog();
                if (userLoginWindow.IsLoginSuccessful)
                {
                    var user = await _userRepo.GetUserAsync(SelectedUser.Username, _cts.Token);
                    var userId = user.Id;

                    var mainWindow = App.Current.Services.GetRequiredService<MainWindow>();
                    mainWindow.Show();

                    _messenger.Send(new UserLoginIdMsg(userId));
                    _messenger.Send<CloseChooseUserWindowMsg>();
                }
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
        /// <para>Logic for clicking 'Register' button.</para>
        /// <para>Displays modal <see cref="UserRegistrationWindow"/> and prompts user to enter data for registration.</para>
        /// <para>On success - modal window saves user to database and this function calls <see cref="UpdateUsersList"/>.</para>
        /// <para>On fail - does nothing.</para>
        /// </summary>
        [RelayCommand]
        private void RegisterNewUserClick()
        {
            var userRegistrationWindow = App.Current.Services.GetRequiredService<UserRegistrationWindow>();
            userRegistrationWindow.ShowDialog();
            
            if (userRegistrationWindow.IsRegistrationSuccessful)
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
                var userLoginDTO = new UserLoginDTO()
                {
                    Username = SelectedUser.Username,
                    AvatarPath = SelectedUser.AvatarPath
                };

                var userLoginWindow = App.Current.Services.GetRequiredService<UserLoginWindow>();
                _messenger.Send(new UserLoginDTOMsg(userLoginDTO));

                userLoginWindow.ShowDialog();

                if (userLoginWindow.IsLoginSuccessful == true)
                {
                    await _userRepo.DeleteUserAsync(SelectedUser.Username, _cts.Token);
                    UpdateUsersList();
                }
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
                    foreach (var user in users)
                    {
                        UsersList.Add(new UserLoginListItem()
                        {
                            Username = user.Username,
                            AvatarPath = _avatarImageManager.GetUserAvatar(user.Id)!
                        });
                    }
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
