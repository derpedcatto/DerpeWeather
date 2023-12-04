using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.Models;
using DerpeWeather.MVVM.Views;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DerpeWeather.MVVM.ViewModels
{
    public partial class MainWindowVM : ObservableObject, IDisposable
    {
        #region Variables

        private readonly CancellationTokenSource _cts;
        private bool disposedValue;

        private readonly IUserRepo _userRepo;
        private readonly IMessenger _messenger;
        private readonly IWeatherApiClient _weatherApiClient;
        private Guid _userId;

        /// <summary>
        /// Weather fields list to display in View listview.
        /// </summary>
        public ObservableCollection<UserTrackedWeatherFieldItem> WeatherList
        {
            get => _weatherList;
            set => SetProperty(ref _weatherList, value);
        }
        private ObservableCollection<UserTrackedWeatherFieldItem> _weatherList;

        [ObservableProperty]
        private UserTrackedWeatherFieldItem _SelectedWeatherItem;

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindowVM(
            IUserRepo userRepo,
            IMessenger messenger,
            IWeatherApiClient weatherApiClient)
        {
            _cts = new();

            _userRepo = userRepo;
            _messenger = messenger;
            _weatherApiClient = weatherApiClient;

            _messenger.Register<UserLoginIdMsg>(this, OnUserLoginIdMsgReceived);
            _messenger.Register<NewTrackedWeatherLocationMsg>(this, OnAddNewLocationToListMsgReceived);

            WeatherList = new();
        }



        #region RelayCommand

        [RelayCommand]
        private void UserLogoutClick()
        {
            var chooseUserWindow = App.Current.Services.GetRequiredService<ChooseUserWindow>();
            chooseUserWindow.Show();

            _messenger.Send(new UserLogoutMsg());
        }

        [RelayCommand]
        private void UserPreferencesClick()
        {
            /*
            var userPreferencesWindow = App.Current.Services.GetService<UserPreferencesWindow>();
            userPreferencesWindow!.ShowDialog();
            */
        }

        [RelayCommand]
        private async Task RefreshListClick()
        {
            try
            {
                var user = await _userRepo.GetUserAsync(_userId, _cts.Token);
                var userWeatherList = user.TrackedWeatherFields;

                WeatherList ??= new();

                if (userWeatherList.Count >= 0)
                {
                    WeatherList.Clear();

                    var tasks = userWeatherList.Select(async field =>
                    {
                        var apiResponseObj = await _weatherApiClient.GetWeatherDataForToday(_userId, field.Location);
                        if (apiResponseObj != null)
                        {
                            WeatherList.Add(apiResponseObj);
                        }
                    });

                    await Task.WhenAll(tasks);
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                AdonisUI.Controls.MessageBox.Show(
                    ex.Message,
                    "Refresh List Error!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
            }
        }

        [RelayCommand]
        private void AddLocationClick()
        {
            var addLocationWindow = App.Current.Services.GetService<AddLocationWindow>();
            _messenger.Send<AddLocationUserIdMsg>(new(_userId));
            addLocationWindow!.ShowDialog();

            // On success - Receives message to `OnAddNewLocationToListMsgReceived`
        }

        [RelayCommand]
        private void DeleteSelectedItemClick()
        {
            if (SelectedWeatherItem != null)
            {
                _userRepo.DeleteTrackedField(_userId, SelectedWeatherItem.Location);
                WeatherList.Remove(SelectedWeatherItem);
            }
        }

        #endregion



        #region MessageHandlers

        /// <summary>
        /// Receives message from <see cref="Views.ChooseUserWindow"/> with logged in User GUID from <paramref name="message"/>, sets it to <see cref="_userRepo"/>.
        /// </summary>
        /// <param name="message">Contains User Database GUID.</param>
        private async void OnUserLoginIdMsgReceived(object recipient, UserLoginIdMsg message)
        {
            _userId = message.Value;
            await RefreshListClick();
        }

        /// <summary>
        /// Receive message from <see cref="Views.AddLocationWindow"/> with name of location string.
        /// Function adds a new location entry to DB and adds new entry to <see cref="WeatherList"/>.
        /// </summary>
        /// <param name="message">Tracked Location name string.</param>
        private async void OnAddNewLocationToListMsgReceived(object recipient, NewTrackedWeatherLocationMsg message)
        {
            var weatherField = await _weatherApiClient.GetWeatherDataForToday(_userId, message.Value);

            if (weatherField != null)
            {
                await _userRepo.AddNewTrackedLocationAsync(_userId, message.Value, _cts.Token);
                WeatherList.Add(weatherField);
            }
            else
            {
                AdonisUI.Controls.MessageBox.Show(
                    "Entered location is not valid.",
                    "Add Location Error!",
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error
                );
            }
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
