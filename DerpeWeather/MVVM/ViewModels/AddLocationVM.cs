using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using System;

namespace DerpeWeather.MVVM.ViewModels
{
    public partial class AddLocationVM : ObservableObject, IDisposable
    {
        private readonly IMessenger _messenger;
        private readonly IUserInputValidator _userInputValidator;
        private Guid _userId;

        [ObservableProperty]
        private string _LocationName;
        private bool disposedValue;



        public AddLocationVM(IMessenger messenger, IUserInputValidator userInputValidator) 
        { 
            _messenger = messenger;
            _userInputValidator = userInputValidator;

            _messenger.Register<AddLocationUserIdMsg>(this, AddLocationUserIdMsgReceived);
        }



        [RelayCommand]
        private void SaveLocationBtnClick()
        {
            string errorMsg = _userInputValidator.CheckLocationName(_userId, LocationName);
            if (errorMsg == string.Empty)
            {
                _messenger.Send(new NewTrackedWeatherLocationMsg(LocationName));
                _messenger.Send(new CloseAddLocationWindowMsg());
            }
            else
            {
                AdonisUI.Controls.MessageBox.Show(
                    errorMsg,
                    "SaveLocationBtnClick error!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
            }
        }



        private void AddLocationUserIdMsgReceived(object recipient, AddLocationUserIdMsg message)
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
