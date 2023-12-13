using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.Models;
using DerpeWeather.MVVM.Views;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.Views;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DerpeWeather.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="MainWindow"/>.
    /// </summary>
    public partial class MainWindowVM : ObservableObject, IDisposable
    {
        #region Variables

        private readonly CancellationTokenSource _cts;
        private bool disposedValue;

        private readonly IUserRepo _userRepo;
        private readonly IMessenger _messenger;
        private readonly IWeatherApiClient _weatherApiClient;
        private readonly IWindowFactory _windowFactory;

        private Guid _userId;

        [ObservableProperty]
        private Visibility _LoadingIndicatorVisibility;

        [ObservableProperty]
        private bool _ButtonsEnabled;

        [ObservableProperty]
        public ObservableCollection<UserTrackedWeatherFieldItem> _WeatherList;

        [ObservableProperty]
        private UserTrackedWeatherFieldItem _SelectedWeatherItem;


        bool _isLocationSortAscending;
        private ICommand _sortCommand;
        public ICommand SortCommand
        {
            get
            {
                _sortCommand ??= new RelayCommand<string>(Sort);
                return _sortCommand;
            }
        }

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindowVM(
            IUserRepo userRepo,
            IMessenger messenger,
            IWeatherApiClient weatherApiClient,
            IWindowFactory windowFactory)
        {
            _cts = new();
            _isLocationSortAscending = true;

            _userRepo = userRepo;
            _messenger = messenger;
            _weatherApiClient = weatherApiClient;
            _windowFactory = windowFactory;

            _messenger.Register<UserLoginIdMsg>(this, OnUserLoginIdMsgReceived);
            _messenger.Register<NewLocationStrMsg>(this, OnAddNewLocationToListMsgReceived);

            WeatherList = new();
        }



        #region RelayCommand

        /// <summary>
        /// Opens a <see cref="ChooseUserWindow"/> and sends <see cref="UserLogoutMsg"/> to close <see cref="MainWindow"/>.
        /// </summary>
        [RelayCommand]
        private void UserLogoutClick()
        {
            var chooseUserWindow = _windowFactory.CreateWindow<ChooseUserWindow>(App.Current.MainWindow);
            chooseUserWindow.Show();

            _messenger.Send(new UserLogoutMsg());
        }

        /// <summary>
        /// Opens a <see cref="UserPreferencesWindow"/> and sends <see cref="PrefWndUserIdMsg"/> to it.
        /// </summary>
        [RelayCommand]
        private void UserPreferencesClick()
        {
            var userPreferencesWindow = _windowFactory.CreateWindow<UserPreferencesWindow>(App.Current.MainWindow);
            _messenger.Send(new PrefWndUserIdMsg(_userId));
            userPreferencesWindow!.ShowDialog();

            userPreferencesWindow.Dispose();
        }

        /// <summary>
        /// Refreshes <see cref="WeatherList"/> with data from API call.
        /// </summary>
        [RelayCommand]
        private async Task RefreshListClick()
        {
            SwitchLoadingIndicatorState(true);
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
                        var apiResponseObj = await _weatherApiClient.GetWeatherData(_userId, field.Location, _cts.Token);
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
            SwitchLoadingIndicatorState(false);
        }

        /// <summary>
        /// Opens a <see cref="AddLocationWindow/> and sends <see cref="AddLocationUserIdMsg"/> to it.
        /// On success - triggers <see cref="OnAddNewLocationToListMsgReceived(object, NewLocationStrMsg)"/>.
        /// </summary>
        [RelayCommand]
        private void AddLocationClick()
        {
            var addLocationWindow = _windowFactory.CreateWindow<AddLocationWindow>(App.Current.MainWindow);
            _messenger.Send<AddLocationUserIdMsg>(new(_userId));
            addLocationWindow!.ShowDialog();
            
            // On success - Receives message to `OnAddNewLocationToListMsgReceived`
        }

        /// <summary>
        /// Deletes <see cref="SelectedWeatherItem"/> from database and <see cref="WeatherList"/>.
        /// </summary>
        [RelayCommand]
        private void DeleteSelectedItemClick()
        {
            if (SelectedWeatherItem != null)
            {
                _userRepo.DeleteTrackedField(_userId, SelectedWeatherItem.Location);
                WeatherList.Remove(SelectedWeatherItem);
            }
        }

        /// <summary>
        /// Opens <see cref="WeatherDetailsWindow"/> and sends <see cref="PassWeatherDetailsMsg"/> 
        /// to it with <see cref="SelectedWeatherItem"/> weather details.
        /// </summary>
        [RelayCommand]
        private void DetailsClick()
        {
            if (SelectedWeatherItem != null)
            {
                var detailsWindow = _windowFactory.CreateWindow<WeatherDetailsWindow>(App.Current.MainWindow);
                _messenger.Send(new PassWeatherDetailsMsg(SelectedWeatherItem.WeatherDetails));
                detailsWindow.ShowDialog();
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

            var user = await _userRepo.GetUserAsync(_userId, _cts.Token);
            App.Current.SwitchColorScheme(user.Preferences.Theme);

            await RefreshListClick();
        }

        /// <summary>
        /// Receive message from <see cref="Views.AddLocationWindow"/> with name of location string.
        /// Function adds a new location entry to DB and adds new entry to <see cref="WeatherList"/>.
        /// </summary>
        /// <param name="message">Tracked Location name string.</param>
        private async void OnAddNewLocationToListMsgReceived(object recipient, NewLocationStrMsg message)
        {
            SwitchLoadingIndicatorState(true);

            var weatherField = await _weatherApiClient.GetWeatherData(_userId, message.Value, _cts.Token);

            if (weatherField != null)
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                weatherField.Location = textInfo.ToTitleCase(weatherField.Location);

                await _userRepo.AddNewTrackedLocationAsync(_userId, weatherField.Location, weatherField.ResolvedLocation, _cts.Token);
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

            SwitchLoadingIndicatorState(false);
        }

        #endregion



        /// <summary>
        /// Sorts Location column alphabetically.
        /// </summary>
        /// <param name="columnName"></param>
        private void Sort(string columnName)
        {
            if (columnName == "Location")
            {
                _isLocationSortAscending = !_isLocationSortAscending;

                var sortedRows = _isLocationSortAscending 
                    ? WeatherList.OrderBy(user => user.Location).ToList()
                    : WeatherList.OrderByDescending(user => user.Location).ToList();

                WeatherList.Clear();
                foreach (var row in sortedRows)
                {
                    WeatherList.Add(row);
                }
            }
        }



        private void SwitchLoadingIndicatorState(bool isLoading)
        {
            if (isLoading) 
            {
                ButtonsEnabled = false;
                LoadingIndicatorVisibility = Visibility.Visible;
            }
            else
            {
                ButtonsEnabled = true;
                LoadingIndicatorVisibility = Visibility.Collapsed;
            }
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
