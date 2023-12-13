using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.Models;
using DerpeWeather.Utilities.Messages;
using System;
using System.Collections.ObjectModel;

namespace DerpeWeather.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="WeatherDetailsWindow"/>.
    /// </summary>
    public partial class WeatherDetailsVM : ObservableObject, IDisposable
    {
        #region Variables

        private readonly IMessenger _messenger;
        private bool disposedValue;

        [ObservableProperty]
        private ObservableCollection<WeatherDetailsItem> _WeatherDetailsList;

        [ObservableProperty]
        private WeatherDetailsItem _SelectedWeatherDetailItem;

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public WeatherDetailsVM(IMessenger messenger)
        {
            WeatherDetailsList = new();

            _messenger = messenger;

            _messenger.Register<PassWeatherDetailsMsg>(this, OnPassWeatherDetailsMsgReceived);
        }



        private void OnPassWeatherDetailsMsgReceived(object recipient, PassWeatherDetailsMsg message)
        {
            foreach (var item in message.Value)
            {
                WeatherDetailsList.Add(item);
            }

            SelectedWeatherDetailItem = WeatherDetailsList[0];
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
