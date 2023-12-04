using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.Models;
using DerpeWeather.MVVM.Views;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DerpeWeather.MVVM.ViewModels
{
    public partial class MainWindowVM : ObservableObject, IDisposable
    {
        #region Variables

        private readonly UserPreferencesWindow _userPreferencesWindow;
        private readonly AddLocationWindow _addLocationWindow;

        private readonly IUserRepo _userRepo;
        private readonly IMessenger _messenger;
        private readonly IWeatherApiClient _weatherApiClient;
        private Guid _userId;
        private bool disposedValue;

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
        public MainWindowVM (IUserRepo userRepo,
            IMessenger messenger,
            IWeatherApiClient weatherApiClient,
            UserPreferencesWindow userPreferencesWindow,
            AddLocationWindow addLocationWindow)
        {
            _userRepo = userRepo;
            _messenger = messenger;
            _weatherApiClient = weatherApiClient;

            _userPreferencesWindow = userPreferencesWindow;

            _messenger.Register<UserLoginIdMsg>(this, OnUserLoginIdMsgReceived);
            _messenger.Register<NewTrackedWeatherLocationMsg>(this, OnAddNewLocationToListMsgReceived);
            _addLocationWindow = addLocationWindow;

            WeatherList = new();
        }



        #region RelayCommand

        [RelayCommand]
        private void UserLogoutClick()
        {
            _messenger.Send(new UserLogoutMsg());
        }

        [RelayCommand]
        private void UserPreferencesClick()
        {
            _userPreferencesWindow.ShowDialog();
        }

        [RelayCommand]
        private async Task RefreshListClick()
        {
            var user = _userRepo.GetUser(_userId);

            WeatherList ??= new();

            if (user.TrackedWeatherFields.Count >= 0)
            {
                WeatherList.Clear();

                foreach (var field in user.TrackedWeatherFields)
                {
                    var apiResponseObj = await _weatherApiClient.GetWeatherDataForToday(_userId, field.Location);
                    if (apiResponseObj != null)
                    {
                        WeatherList.Add(apiResponseObj);
                    }
                }
            }
        }

        [RelayCommand]
        private void AddLocationClick()
        {
            _messenger.Send<AddLocationUserIdMsg>(new(_userId));
            _addLocationWindow.ShowDialog();

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
                _userRepo.AddNewTrackedLocation(_userId, message.Value);
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
