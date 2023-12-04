using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.DAL;
using DerpeWeather.DAL.Repos;
using DerpeWeather.MVVM.ViewModels;
using DerpeWeather.MVVM.Views;
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

namespace DerpeWeather
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Variables
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }

        public static string SQLConnectionString { get; private set; } 

        public static string WeatherAPIKey { get; private set; }
        public static string WeatherAPIBaseUri { get; private set; }

        public static string AppDataRootFolderName { get; private set; } 
        public static string AppDataRootFolderPath { get; private set; }
        public static string UserDataFolderName { get; private set; }
        public static string UserAvatarFileName { get; private set; }

        public static Bitmap DefaultUserAvatar { get; private set; }
        #endregion



        public App()
        {
            SQLConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=DerpeWeatherApp;Integrated Security=True"; // ;Pooling=False;Encrypt=True;Trust Server Certificate=True
            WeatherAPIKey = "FYUGK5XRT77PFQ2WPZ5EASF8X";
            WeatherAPIBaseUri = @$"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/";

            AppDataRootFolderName = "DerpeWeather";
            AppDataRootFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataRootFolderName);
            UserDataFolderName = "UserData";
            UserAvatarFileName = "avatar";

            DefaultUserAvatar = DerpeWeather.Properties.Resources.defaultavatar;


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

            services.AddSingleton<IRestClient>( new RestClient(App.WeatherAPIBaseUri) );
            services.AddSingleton<IWeatherApiClient, WeatherApiClient>();

            services.AddSingleton<IHashService, HashServiceSha256>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>();
            services.AddSingleton<IUserInputValidator, UserInputValidator>();
            services.AddSingleton<IUserDirHelper, UserDirHelper>();
            services.AddSingleton<ISystemPreferenceFetcher, SystemPreferenceFetcherWindows>();
            services.AddSingleton<IAvatarImageManager, AvatarImageManager>();
            services.AddSingleton<IWindowService, WindowService>();
            
            services.AddTransient<ChooseUserVM>();
            services.AddTransient<UserLoginVM>();
            services.AddTransient<UserRegistrationVM>();
            services.AddTransient<MainWindowVM>();
            services.AddTransient<UserPreferencesVM>();
            services.AddTransient<AddLocationVM>();

            services.AddTransient<ChooseUserWindow>(s => 
                new ChooseUserWindow(s.GetRequiredService<ChooseUserVM>(), s.GetRequiredService<IMessenger>())
            );
            services.AddTransient<UserRegistrationWindow>(s => 
                new UserRegistrationWindow(s.GetRequiredService<UserRegistrationVM>())
            );
            services.AddTransient<UserLoginWindow>(s => 
                new UserLoginWindow(s.GetRequiredService<UserLoginVM>())
            );
            services.AddTransient<MainWindow>(s =>
                new MainWindow(s.GetRequiredService<MainWindowVM>(), s.GetRequiredService<IMessenger>(), s.GetRequiredService<ChooseUserWindow>())
            );
            services.AddTransient<UserPreferencesWindow>(s =>
                new UserPreferencesWindow(s.GetRequiredService<UserPreferencesVM>())
            );
            services.AddTransient<AddLocationWindow>(s =>
                new AddLocationWindow(s.GetRequiredService<AddLocationVM>(), s.GetRequiredService<IMessenger>())
            );

            return services.BuildServiceProvider();
        }



        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (CreateAppDataDir())
            {
                // ChooseUserWindow startupWindow = new();
                var startupWindow = App.Current.Services.GetService<ChooseUserWindow>();
                startupWindow!.Show();
            }
        }



        /// <summary>
        /// Creates folders for saving user data if they are not created
        /// </summary>
        private bool CreateAppDataDir()
        {
            try
            {
                if (!Directory.Exists(AppDataRootFolderPath))
                {
                    Directory.CreateDirectory(AppDataRootFolderPath);
                    Directory.CreateDirectory(Path.Combine(AppDataRootFolderPath, UserDataFolderName));
                }
                return true;
            }
            catch (IOException ex)
            {
                AdonisUI.Controls.MessageBox.Show(ex.Message, "OnStartup Error - Can't create 'AppData/Local' folder!", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Error);
                return false;
            }
        }
    }
}
