using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DerpeWeather.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="AddLocationWindow"/>.
    /// </summary>
    public partial class AddLocationVM : ObservableObject, IDisposable
    {
        #region Variables

        private readonly CancellationTokenSource _cts;
        private bool disposedValue;

        private readonly IMessenger _messenger;
        private readonly IUserInputValidator _userInputValidator;
        private readonly IWeatherApiClient _weatherApiClient;

        private Guid _userId;

        [ObservableProperty]
        private string _LocationName;

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddLocationVM(
            IMessenger messenger,
            IUserInputValidator userInputValidator,
            IWeatherApiClient weatherApiClient)
        {
            _cts = new();

            _messenger = messenger;
            _userInputValidator = userInputValidator;
            _weatherApiClient = weatherApiClient;

            _messenger.Register<AddLocationUserIdMsg>(this, AddLocationUserIdMsgReceived);
        }



        /// <summary>
        /// Checks if <see cref="LocationName"/> is valid by making a call to API.
        /// On success - sends <see cref="CloseAddLocationWndMsg"/> and
        /// <see cref="NewLocationStrMsg"/> with <see cref="LocationName"/>
        /// On fail - displays Message Box with error data.
        /// </summary>
        [RelayCommand]
        private async Task SaveLocationBtnClickAsync()
        {
            LocationName = LocationName.Trim();

            var weatherData = await _weatherApiClient.GetWeatherData(_userId, LocationName, _cts.Token);
            string errorMsg;

            if (weatherData != null)
            {
                errorMsg = _userInputValidator.CheckLocationName(_userId, LocationName, weatherData.ResolvedLocation);
                if (errorMsg == string.Empty)
                {
                    _messenger.Send(new NewLocationStrMsg(LocationName));
                    _messenger.Send(new CloseAddLocationWndMsg());
                    return;
                }
            }
            else
            {
                errorMsg = "Location name is invalid!";
            }

            AdonisUI.Controls.MessageBox.Show(
                errorMsg,
                "SaveLocationBtnClick error!",
                AdonisUI.Controls.MessageBoxButton.OK,
                AdonisUI.Controls.MessageBoxImage.Error
            );
            return;
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
