using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using System;

namespace DerpeWeather.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="ChangeUsernameWindow"/>.
    /// </summary>
    public partial class ChangeUsernameVM : ObservableObject, IDisposable
    {
        #region Variables

        private bool disposedValue;

        private readonly IUserRepo _userRepo;
        private readonly IUserInputValidator _userInputValidator;
        private readonly IMessenger _messenger;

        private Guid _userId;

        [ObservableProperty]
        private string _Username;

        #endregion



        public ChangeUsernameVM(
            IUserRepo userRepo,
            IUserInputValidator userInputValidator,
            IMessenger messenger)
        {
            _userRepo = userRepo;
            _userInputValidator = userInputValidator;
            _messenger = messenger;

            _messenger.Register<ChangeUsernameWndUserIdMsg>(this, OnChangeUsernameWndUserIdMsgReceived);
        }



        /// <summary>
        /// On success - changes user username to <see cref="Username"/> and
        /// sends <see cref="ChangeUsernameSuccessMsg"/>.
        /// On fail - displays Message Box with error data.
        /// </summary>
        [RelayCommand]
        private void SetUsernameBtnClick()
        {
            string errorMsg = _userInputValidator.CheckUsername(Username);
            if (errorMsg == string.Empty)
            {
                Username = Username.Trim();
                _userRepo.UpdateUserUsername(_userId, Username);
                _messenger.Send<ChangeUsernameSuccessMsg>();
            }
            else
            {
                AdonisUI.Controls.MessageBox.Show(
                    errorMsg,
                    "Nickname error!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
            }
        }



        private void OnChangeUsernameWndUserIdMsgReceived(object recipient, ChangeUsernameWndUserIdMsg message)
        {
            _userId = message.Value;
        }



        #region Disposable

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
