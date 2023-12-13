using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.Utilities.Helpers.UserData;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DerpeWeather.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="ChangeAvatarWindow"/>.
    /// </summary>
    public partial class ChangeAvatarVM : ObservableObject, IDisposable
    {
        #region Variables

        private readonly IMessenger _messenger;
        private readonly IAvatarImageManager _avatarImageManager;
        private readonly IUserInputValidator _userInputValidator;
        private bool disposedValue;

        private Guid _userId;

        [ObservableProperty]
        private string _UserAttachmentBtnString;

        private string? _userAvatarPath;

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChangeAvatarVM(
            IMessenger messenger,
            IAvatarImageManager avatarImageManager,
            IUserInputValidator userInputValidator)
        {
            _avatarImageManager = avatarImageManager;
            _messenger = messenger;
            _userInputValidator = userInputValidator;

            _messenger.Register<ChangeAvatarWndUserIdMsg>(this, OnChangeAvatarWndUserIdMsgReceived);

            UserAttachmentBtnString = "Select Path";
        }



        /// <summary>
        /// Prompts user to choose an avatar file in his system.
        /// On success - saves avatar path string to <see cref="_userAvatarPath"/> 
        /// and renames <see cref="UserAttachmentBtnString"/> to file name.
        /// On fail - displays Message Box with error data.
        /// </summary>
        [RelayCommand]
        private void AddAttachmentBtnClick()
        {
            string? imagePath = _avatarImageManager.PromptFileDialog();
            if (imagePath != null && imagePath != string.Empty)
            {
                string errorMsg = _userInputValidator.CheckAvatar(imagePath);

                if (errorMsg == string.Empty)
                {
                    _userAvatarPath = imagePath;
                    UserAttachmentBtnString = Path.GetFileName(imagePath);
                }
                else
                {
                    AdonisUI.Controls.MessageBox.Show(
                        "Avatar error!",
                        errorMsg,
                        AdonisUI.Controls.MessageBoxButton.OK,
                        AdonisUI.Controls.MessageBoxImage.Error
                    );
                }
            }
        }

        /// <summary>
        /// Sets avatar from <see cref="_userAvatarPath"/> that is chosen by user through <see cref="AddAttachmentBtnClick"/>.
        /// On success - saves avatar in user UserData folder in AppData and sends <see cref="ChangeAvatarSuccessMsg"/> to view.
        /// On fail displays a Message Box with error data.
        /// </summary>
        [RelayCommand]
        private void ChangeAvatarBtnClick()
        {
            string errorMsg = _userInputValidator.CheckAvatar(_userAvatarPath);
            if (errorMsg == string.Empty)
            {
                try
                {
                    _avatarImageManager.CopyAvatarToUserFolder(_userId, _userAvatarPath);
                    _messenger.Send(new ChangeAvatarSuccessMsg(_avatarImageManager.GetUserAvatar(_userId)));
                    return;
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }
            }

            AdonisUI.Controls.MessageBox.Show(
                errorMsg,
                "Avatar error!",
                AdonisUI.Controls.MessageBoxButton.OK,
                AdonisUI.Controls.MessageBoxImage.Error
            );
        }



        private void OnChangeAvatarWndUserIdMsgReceived(object recipient, ChangeAvatarWndUserIdMsg message)
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
