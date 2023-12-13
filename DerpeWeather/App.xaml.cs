using AdonisUI;
using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.DAL;
using DerpeWeather.DAL.Repos;
using DerpeWeather.MVVM.ViewModels;
using DerpeWeather.MVVM.Views;
using DerpeWeather.Utilities.Enums;
using DerpeWeather.Utilities.Helpers.Hash;
using DerpeWeather.Utilities.Helpers.UserData;
using DerpeWeather.Utilities.Helpers.View;
using DerpeWeather.Utilities.Helpers.WeatherApi;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.ViewModels;
using DerpeWeather.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Text.Json;
using DerpeWeather.MVVM.Models;
using System.Threading.Tasks;

namespace DerpeWeather
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Variables

        private static IWindowFactory _windowFactory;
        private static IAppJsonSettingsValidator _appJsonSettingsValidator;
        private static ISystemPreferenceFetcher _systemPreferenceFetcher;

        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }


        /// <summary>
        /// File path to 'appsettings.json' file located in 'DerpeWeather' AppData folder.
        /// </summary>
        public static string AppSettingsFilePath { get; private set; }

        /// <summary>
        /// Connection string to connect to SQL Server Database.
        /// </summary>
        public static string SQLConnectionString { get; private set; }


        /// <summary>
        /// API key to authenticate to VisualCrossing weather.
        /// </summary>
        public static string WeatherAPIKey { get; private set; }

        /// <summary>
        /// URI that stores a base URI string to form new VisualCrossing Weather API requests from.
        /// </summary>
        public static string WeatherAPIBaseUri { get; private set; }


        /// <summary>
        /// Name of app folder in AppData to store all settings.
        /// </summary>
        public static string AppDataRootFolderName { get; private set; }

        /// <summary>
        /// Full path to a <see cref="AppDataRootFolderName"/> folder.
        /// </summary>
        public static string AppDataRootFolderPath { get; private set; }

        /// <summary>
        /// Folder name for storing each User settings.
        /// </summary>
        public static string RootUserDataFolderName { get; private set; }

        /// <summary>
        /// Name for an avatar file stored in users personal folder (no extension).
        /// Every avatar image MUST be with this name.
        /// </summary>
        public static string UserAvatarFileName { get; private set; }


        /// <summary>
        /// Default avatar image that is set if user didn't choose anything during registration / avatar image is corrupted.
        /// </summary>
        public static Bitmap DefaultUserAvatarImage { get; private set; }

        #endregion



        /// <summary>
        /// Default constructor.
        /// </summary>
        public App()
        {
            WeatherAPIBaseUri = @$"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/";

            AppDataRootFolderName = "DerpeWeather";
            AppDataRootFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataRootFolderName);
            RootUserDataFolderName = "UserData";
            UserAvatarFileName = "avatar";

            DefaultUserAvatarImage = DerpeWeather.Properties.Resources.defaultavatar;

            AppSettingsFilePath = Path.Combine(AppDataRootFolderPath, "appsettings.json");


            Services = ConfigureServices();

            this.InitializeComponent();
        }



        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<EFContext>(options =>
                options.UseSqlServer(SQLConnectionString),
                ServiceLifetime.Scoped
            );
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserInputValidator, UserInputValidator>();
            services.AddScoped<IWeatherApiClient, VisualCrossingWeatherApiClient>();
            services.AddScoped<IAppJsonSettingsValidator, AppJsonSettingsValidator>();

            // Stateless services
            services.AddSingleton<IRestClient>(new RestClient(App.WeatherAPIBaseUri));
            services.AddSingleton<IHashService, HashServiceSha256>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>();
            services.AddSingleton<IUserDirHelper, UserDirHelper>();
            services.AddSingleton<ISystemPreferenceFetcher, SystemPreferenceFetcherWindows>();
            services.AddSingleton<IAvatarImageManager, AvatarImageManager>();
            services.AddSingleton<IWindowFactory, WindowFactory>();


            // ViewModels
            services.AddTransient<ChooseUserVM>();
            services.AddTransient<UserLoginVM>();
            services.AddTransient<UserRegistrationVM>();
            services.AddTransient<MainWindowVM>();
            services.AddTransient<UserPreferencesVM>();
            services.AddTransient<AddLocationVM>();
            services.AddTransient<ChangeUsernameVM>();
            services.AddTransient<ChangePasswordVM>();
            services.AddTransient<ChangeAvatarVM>();
            services.AddTransient<AppJsonSettingsVM>();
            services.AddTransient<WeatherDetailsVM>();


            // Windows
            services.AddTransient<ChooseUserWindow>();
            services.AddTransient<UserRegistrationWindow>();
            services.AddTransient<UserLoginWindow>();
            services.AddTransient<MainWindow>();
            services.AddTransient<UserPreferencesWindow>();
            services.AddTransient<AddLocationWindow>();
            services.AddTransient<ChangeUsernameWindow>();
            services.AddTransient<ChangePasswordWindow>();
            services.AddTransient<ChangeAvatarWindow>();
            services.AddTransient<AppJsonSettingsWindow>();
            services.AddTransient<WeatherDetailsWindow>();

            return services.BuildServiceProvider();
        }



        protected override void OnStartup(StartupEventArgs e) => OnStartupAsync(e);
        protected async Task OnStartupAsync(StartupEventArgs e)
        {
            base.OnStartup(e);

            _windowFactory = App.Current.Services.GetRequiredService<IWindowFactory>();
            _appJsonSettingsValidator = App.Current.Services.GetRequiredService<IAppJsonSettingsValidator>();
            _systemPreferenceFetcher = App.Current.Services.GetRequiredService<ISystemPreferenceFetcher>();

            this.SwitchColorScheme(false);

            var messageBox = new AdonisUI.Controls.MessageBoxModel
            {
                Icon = AdonisUI.Controls.MessageBoxImage.Error,
                Buttons = new[] { AdonisUI.Controls.MessageBoxButtons.Yes() }
            };


            if (CreateAppDataDir())
            {
                if (LoadAndApplyAppSettings())
                {
                    bool isWeatherApiValid = await _appJsonSettingsValidator.ValidateWeatherAPIConnectionAsync(WeatherAPIKey);
                    bool isSqlValid = await _appJsonSettingsValidator.ValidateSQLConnectionStringAsync(SQLConnectionString);

                    if (isSqlValid && isWeatherApiValid)
                    {
                        var startupWindow = _windowFactory.CreateWindow<ChooseUserWindow>(App.Current.MainWindow);
                        startupWindow.Show();
                        return;
                    }
                    else
                    {
                        messageBox.Caption = "App Data is invalid!";
                        messageBox.Text = "Please enter valid data in the following window.";

                        App.Current.Dispatcher.BeginInvoke(new Action(() =>
                            AdonisUI.Controls.MessageBox.Show(messageBox)
                        ));
                    }
                }
                else
                {
                    messageBox.Caption = "LoadAppSettings error!";
                    messageBox.Text = "Please enter valid data in the following window.";

                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
                        AdonisUI.Controls.MessageBox.Show(messageBox)
                    ));
                }

                var settingsWindow = _windowFactory.CreateWindow<AppJsonSettingsWindow>(null);
                settingsWindow.Show();
            }
        }



        /// <summary>
        /// Switches app theme in all windows based on <paramref name="isDark"/> value.
        /// </summary>
        public void SwitchColorScheme(bool isDark)
        {
            AdonisUI.ResourceLocator.SetColorScheme(
                Application.Current.Resources,
                isDark ? ResourceLocator.DarkColorScheme : ResourceLocator.LightColorScheme);
        }

        /// <summary>
        /// Switches app theme in all windows based on <paramref name="theme"/> value.
        /// </summary>
        public void SwitchColorScheme(UserPreferenceTheme theme)
        {
            switch (theme)
            {
                case UserPreferenceTheme.LIGHT:
                    AdonisUI.ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.LightColorScheme);
                    break;
                case UserPreferenceTheme.DARK:
                    AdonisUI.ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);
                    break;
                case UserPreferenceTheme.SYSTEM:
                    SwitchColorScheme(_systemPreferenceFetcher.GetThemePreference());
                    break;
            }
        }



        /// <summary>
        /// Creates folders for <see cref="AppDataRootFolderPath"/>
        /// and <see cref="RootUserDataFolderName"/> inside of it.
        /// </summary>
        /// <returns>
        /// <para>true - Operation success.</para>
        /// <para>false - Operation failed.</para>
        /// </returns>
        private static bool CreateAppDataDir()
        {
            try
            {
                if (!Directory.Exists(AppDataRootFolderPath))
                {
                    Directory.CreateDirectory(AppDataRootFolderPath);
                    Directory.CreateDirectory(Path.Combine(AppDataRootFolderPath, RootUserDataFolderName));
                }
                return true;
            }
            catch (IOException ex)
            {
                var messageBox = new AdonisUI.Controls.MessageBoxModel
                {
                    Caption = "OnStartup Error - Can't create 'AppData/Local' folder!",
                    Text = ex.Message,
                    Icon = AdonisUI.Controls.MessageBoxImage.Error,
                    Buttons = new[] { AdonisUI.Controls.MessageBoxButtons.Yes() }
                };

                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    AdonisUI.Controls.MessageBox.Show(messageBox)
                ));

                return false;
            }
        }



        /// <summary>
        /// Loads and applies app settings from `appsettings.json` file in <see cref="AppSettingsFilePath"/>.
        /// If file doesn't exist, it displays a <see cref="AppJsonSettingsWindow"/> and
        /// prompts user to enter the valid data.
        /// </summary>
        /// <returns>
        /// <para>true - Operation success.</para>
        /// <para>false - Operation failed.</para>
        /// </returns>
        public static bool LoadAndApplyAppSettings()
        {
            try
            {
                if (!File.Exists(AppSettingsFilePath))
                {
                    var defaultAppSettings = new AppJsonSettings
                    {
                        SQLConnectionString = @"MyConnectionString",
                        WeatherAPIKey = "MyAPIKey"
                    };

                    File.WriteAllText(AppSettingsFilePath, JsonSerializer.Serialize(defaultAppSettings));

                    var settingsWindow = _windowFactory.CreateWindow<AppJsonSettingsWindow>(null);
                    settingsWindow.ShowDialog();
                }

                var appSettingsJson = File.ReadAllText(AppSettingsFilePath);
                var appSettings = JsonSerializer.Deserialize<AppJsonSettings>(appSettingsJson);

                SQLConnectionString = appSettings.SQLConnectionString;
                WeatherAPIKey = appSettings.WeatherAPIKey;

                return true;
            }
            catch (Exception ex)
            {
                var messageBox = new AdonisUI.Controls.MessageBoxModel
                {
                    Caption = "OnStartup Error - Can't work with 'usersettings.json' file!",
                    Text = ex.Message,
                    Icon = AdonisUI.Controls.MessageBoxImage.Error,
                    Buttons = new[] { AdonisUI.Controls.MessageBoxButtons.Yes() }
                };

                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    AdonisUI.Controls.MessageBox.Show(messageBox)
                ));

                return false;
            }
        }


        /// <summary>
        /// Saves and applies <see cref="AppJsonSettings"/> in <see cref="AppSettingsFilePath"/> `appsettings.json` file.
        /// </summary>
        /// <param name="appSettings">Settings object to save.</param>
        public static void SaveAndApplyAppSettings(AppJsonSettings appSettings)
        {
            try
            {
                string appSettingsPath = Path.Combine(AppDataRootFolderPath, "appsettings.json");
                string appSettingsJson = JsonSerializer.Serialize(appSettings);

                SQLConnectionString = appSettings.SQLConnectionString;
                WeatherAPIKey = appSettings.WeatherAPIKey;

                File.WriteAllText(appSettingsPath, appSettingsJson);
            }
            catch (Exception ex)
            {
                var messageBox = new AdonisUI.Controls.MessageBoxModel
                {
                    Caption = "SaveAppSettings Error",
                    Text = ex.Message,
                    Icon = AdonisUI.Controls.MessageBoxImage.Error,
                    Buttons = new[] { AdonisUI.Controls.MessageBoxButtons.Yes() }
                };

                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    AdonisUI.Controls.MessageBox.Show(messageBox)
                ));
            }
        }
    }
}
