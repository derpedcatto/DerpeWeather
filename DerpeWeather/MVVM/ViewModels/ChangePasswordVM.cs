using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using System;
using System.Security;

namespace DerpeWeather.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="ChangePasswordWindow"/>.
    /// </summary>
    public partial class ChangePasswordVM : ObservableObject, IDisposable
    {
        #region Variables

        private bool disposedValue;

        private readonly IMessenger _messenger;
        private readonly IUserRepo _userRepo;
        private readonly IUserInputValidator _userInputValidator;
        private readonly IHashService _hashService;

        private Guid _userId;

        #endregion



        public ChangePasswordVM(
            IMessenger messenger,
            IUserRepo userRepo,
            IUserInputValidator userInputValidator,
            IHashService hashService) 
        { 
            _messenger = messenger;
            _userRepo = userRepo;
            _userInputValidator = userInputValidator;
            _hashService = hashService;

            _messenger.Register<ChangePasswordWndUserIdMsg>(this, OnChangePasswordWndUserIdMsgReceived);
        }



        /// <summary>
        /// On success - changes user password and sends <see cref="ChangePasswordSuccessMsg"/>.
        /// On fail - displays Message Box with error data.
        /// </summary>
        [RelayCommand]
        private void SetPasswordBtnClick(object o)
        {
            using SecureString userPassword = (o as IPasswordContainer)?.Password!;
            string errorMsg = _userInputValidator.CheckPassword(userPassword);
            if (errorMsg == string.Empty)
            {
                _userRepo.UpdateUserPassword(_userId, _hashService.HashString(userPassword));
                _messenger.Send<ChangePasswordSuccessMsg>();
            }
            else
            {
                AdonisUI.Controls.MessageBox.Show(
                    "Set Password Error!",
                    errorMsg,
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
            }
        }



        private void OnChangePasswordWndUserIdMsgReceived(object recipient, ChangePasswordWndUserIdMsg message)
        {
            _userId = message.Value;
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
