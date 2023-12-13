using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.Models;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DerpeWeather.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="AppJsonSettingsWindow"/> window.
    /// </summary>
    public partial class AppJsonSettingsVM : ObservableObject, IDisposable
    {
        #region Variables

        private readonly CancellationTokenSource _cts;
        private bool disposedValue;

        private readonly IMessenger _messenger;
        private readonly IAppJsonSettingsValidator _appJsonSettingsValidator;

        [ObservableProperty]
        private string _SQLConnectionString;

        [ObservableProperty]
        private string _WeatherAPIKey;

        [ObservableProperty]
        private bool _ButtonsBlock;

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public AppJsonSettingsVM(
            IMessenger messenger,
            IAppJsonSettingsValidator appJsonSettingsValidator)
        {
            _cts = new();

            _messenger = messenger;
            _appJsonSettingsValidator = appJsonSettingsValidator;

            ButtonsBlock = false;
        }



        /// <summary>
        /// Validates inputted <see cref="SQLConnectionString"/> and <see cref="WeatherAPIKey"/>.
        /// On success - saves inputted data with <see cref="App.SaveAndApplyAppSettings(AppJsonSettings)"/>.
        /// On fail - displays a Message Box with error data.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task SaveSettingsBtnClick()
        {
            ButtonsBlock = true;
            bool sqlResult = await _appJsonSettingsValidator.ValidateSQLConnectionStringAsync(SQLConnectionString);
            bool weatherApiResult = await _appJsonSettingsValidator.ValidateWeatherAPIConnectionAsync(WeatherAPIKey);

            var messageBox = new AdonisUI.Controls.MessageBoxModel
            {
                Icon = AdonisUI.Controls.MessageBoxImage.Error,
                Buttons = new[] { AdonisUI.Controls.MessageBoxButtons.Yes() }
            };

            if (!sqlResult)
            {
                messageBox.Caption = "SQL String Error!";
                messageBox.Text = "SQL String is not valid.";

                AdonisUI.Controls.MessageBox.Show(messageBox);
            }
            if (!weatherApiResult)
            {
                messageBox.Caption = "Weather API Key Error!";
                messageBox.Text = "Weather API Key is not valid.";

                AdonisUI.Controls.MessageBox.Show(messageBox);
            }

            if (sqlResult && weatherApiResult)
            {
                AppJsonSettings settings = new() 
                {
                    SQLConnectionString = this.SQLConnectionString,
                    WeatherAPIKey = this.WeatherAPIKey
                };

                App.SaveAndApplyAppSettings(settings);

                messageBox.Icon = AdonisUI.Controls.MessageBoxImage.Information;
                messageBox.Caption = "Success!";
                messageBox.Text = "Settings are valid! Please restart the app.";
                AdonisUI.Controls.MessageBox.Show(messageBox);

                _messenger.Send(new ValidAppJsonSettingsMsg(settings));
            }

            ButtonsBlock = false;
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
