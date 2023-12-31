﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.Models;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using System;
using System.Security;

namespace DerpeWeather.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="UserLoginWindow"/>.
    /// </summary>
    public partial class UserLoginVM : ObservableObject, IDisposable
    {
        #region Variables

        private bool disposedValue;

        private readonly IHashService _hashService;
        private readonly IMessenger _messenger;
        private readonly IUserRepo _userRepo;
        private readonly IUserInputValidator _userInputValidator;

        [ObservableProperty]
        private bool isLoginSuccessful;

        public UserLoginListItem SelectedUser { get; private set; }

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserLoginVM(
            IUserRepo userRepo,
            IMessenger messenger, 
            IHashService hashService,
            IUserInputValidator userInputValidator)
        {
            _hashService = hashService;
            _messenger = messenger;
            _userRepo = userRepo;
            _userInputValidator = userInputValidator;

            _messenger.Register<UserLoginDTOMsg>(this, OnUserLoginDTOReceived);
        }



        /// <summary>
        /// Sets <see cref="IsLoginSuccessful"/> to 'true' on validation success.
        /// </summary>
        /// <param name="o"><see cref="IPasswordContainer"/> instance.</param>
        [RelayCommand]
        private void LoginBtnClick(object o)
        {
            using SecureString userPassword = (o as IPasswordContainer)?.Password!;
            if (IsPasswordValid(userPassword))
            {
                _messenger.Send(new LoginSuccessMsg());
            }
        }

        /// <summary>
        /// Validates user password with the one in in DB.
        /// </summary>
        /// <param name="password">User inputted password.</param>
        /// <returns>
        /// <para>On success - true.</para>
        /// <para>On fail - displays error MessageBox and returns false.</para>
        /// </returns>
        private bool IsPasswordValid(SecureString password)
        {
            string errorMsg = _userInputValidator.CheckPassword(password);

            if (errorMsg == string.Empty)
            {
                var userId = _userRepo.GetUser(SelectedUser.Username)!.Id;
                string hashedPassword = _hashService.HashString(password);
                password.Dispose();

                if (_userRepo.IsUserPasswordValid(userId, hashedPassword))
                {
                    return true;
                }
                else
                {
                    AdonisUI.Controls.MessageBox.Show(
                        "Invalid password!",
                        "Login error",
                        AdonisUI.Controls.MessageBoxButton.OK,
                        AdonisUI.Controls.MessageBoxImage.Error
                    );
                }
            }
            else
            {
                AdonisUI.Controls.MessageBox.Show(
                    errorMsg,
                    "Password error!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
                password.Dispose();
            }

            return false;
        }



        /// <summary>
        /// Accepts message <see cref="UserLoginDTOMsg"/> from <see cref="MainWindow"/>. Sets <see cref="Username"/> and <see cref="AvatarPath"/>.
        /// </summary>
        /// <param name="message">Contains user data for displaying in window.</param>
        private void OnUserLoginDTOReceived(object recipient, UserLoginDTOMsg message)
        {
            SelectedUser = new()
            {
                Username = message.Value.Username,
                AvatarPath = message.Value.AvatarPath
            };

            /*
            Username = message.Value.Username;
            AvatarPath = message.Value.AvatarPath;
            */
        }



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
