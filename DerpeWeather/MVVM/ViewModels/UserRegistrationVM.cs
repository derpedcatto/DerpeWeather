using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.DAL.DTO;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace DerpeWeather.ViewModels
{
    public partial class UserRegistrationVM : ObservableObject, IDisposable
    {
        #region Variables 

        private readonly CancellationTokenSource _cts;

        private readonly IHashService _hashService;
        private readonly IUserRepo _userRepo;
        private readonly IUserInputValidator _userInputValidator;
        private readonly IAvatarImageManager _avatarImageManager;
        private readonly ISystemPreferenceFetcher _systemPreferenceFetcher;
        private readonly IMessenger _messenger;

        [ObservableProperty]
        private string? _Username;
        [ObservableProperty]
        private string? _UserAttachmentBtnString;
        private string? _userAvatarPath;
        private bool disposedValue;

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserRegistrationVM(
            IUserRepo userRepo,
            IHashService hashService,
            IUserInputValidator userInputValidator,
            IAvatarImageManager avatarImageManager,
            ISystemPreferenceFetcher systemPreferenceFetcher,
            IMessenger messenger)
        {
            _cts = new();

            _userRepo = userRepo;
            _hashService = hashService;
            _userInputValidator = userInputValidator;
            _avatarImageManager = avatarImageManager;
            _systemPreferenceFetcher = systemPreferenceFetcher;
            _messenger = messenger;

            _userAvatarPath = string.Empty;

            _UserAttachmentBtnString = "Attachment (optional)";
        }



        /// <summary>
        /// Logic on clicking 'Add Attachment' button. Sets <see cref="UserAttachmentBtnString"/> as path to prompted avatar file. 
        /// On error - displays error MessageBox.
        /// </summary>
        [RelayCommand]
        private void AddAttachmentBtnClick()
        {
            string? imagePath = _avatarImageManager.PromptFileDialog();
            if (imagePath != null && imagePath != string.Empty)
            {
                string pathErrorMsg = _userInputValidator.CheckAvatar(imagePath);

                if (pathErrorMsg == string.Empty)
                {
                    _userAvatarPath = imagePath;
                    UserAttachmentBtnString = Path.GetFileName(imagePath);
                }
                else
                {
                    AdonisUI.Controls.MessageBox.Show(
                        "Avatar error!", 
                        pathErrorMsg, 
                        AdonisUI.Controls.MessageBoxButton.OK, 
                        AdonisUI.Controls.MessageBoxImage.Error
                    );
                }
            }
        }

        /// <summary>
        /// Logic on clicking 'Register' button. Validates inputted data through <see cref="IsUserDataValid(string, SecureString, string)"/>
        /// and saves user to database and sets <see cref="IsRegistrationSuccessful"/> to 'true' on success.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task RegisterBtnClickAsync(object o)
        {
            using SecureString userPassword = (o as IPasswordContainer)?.Password!;
            if (IsUserDataValid(Username, userPassword, _userAvatarPath))
            {
                string password = _hashService.HashString(userPassword);

                UserRegistrationDTO model = new()
                {
                    Username = this.Username,
                    Password = password
                };

                await _userRepo.AddNewUserAsync(model, _systemPreferenceFetcher, _cts.Token);
                var userId = _userRepo.GetUser(Username)!.Id;

                if (_userAvatarPath != string.Empty)
                {
                    _avatarImageManager.CopyAvatarToUserFolder(userId, _userAvatarPath);
                }
                else
                {
                    _avatarImageManager.SetDefaultAvatar(userId);
                }

                _messenger.Send(new RegistrationSuccessMsg());
            }
        }



        /// <summary>
        /// Calls validation functions from <see cref="_userInputValidator"/>.
        /// </summary>
        /// <returns>
        /// <para>On success - true.</para>
        /// <para>On fail - displays error MessageBox and returns false.</para>
        /// </returns>
        private bool IsUserDataValid(string username, SecureString password, string avatarPath)
        {
            string errorMsg = string.Empty;

            errorMsg += _userInputValidator.CheckUsername(username);
            if (errorMsg != string.Empty)
            {
                AdonisUI.Controls.MessageBox.Show(
                    errorMsg, 
                    "Nickname error!", 
                    AdonisUI.Controls.MessageBoxButton.OK, 
                    AdonisUI.Controls.MessageBoxImage.Error
                );
                return false;
            }

            errorMsg += _userInputValidator.CheckPassword(password);
            if (errorMsg != string.Empty)
            {
                AdonisUI.Controls.MessageBox.Show(
                    errorMsg, 
                    "Password error!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
                return false;
            }

            if (avatarPath != string.Empty)
            {
                errorMsg += _userInputValidator.CheckAvatar(avatarPath);
                if (errorMsg != string.Empty)
                {
                    AdonisUI.Controls.MessageBox.Show(
                        errorMsg,
                        "Avatar Image error!",
                        AdonisUI.Controls.MessageBoxButton.OK,
                        AdonisUI.Controls.MessageBoxImage.Error
                    );
                    return false;
                }
            }

            return true;
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
