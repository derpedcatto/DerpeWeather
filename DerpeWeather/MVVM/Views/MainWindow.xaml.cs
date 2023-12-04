using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.ViewModels;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.Views;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace DerpeWeather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ChooseUserWindow _chooseUserWindow;

        private readonly IMessenger _messenger;
        private readonly MainWindowVM _viewModel;



        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindow(
            MainWindowVM viewModel,
            IMessenger messenger,
            ChooseUserWindow chooseUserWindow)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;
            _messenger.Register<UserLogoutMsg>(this, OnUserLogout);
            _chooseUserWindow = chooseUserWindow;
        }



        private void WeatherListView_SizeChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GridView gridView = WeatherListView.View as GridView;

            foreach (var column in gridView.Columns)
            {
                if (double.IsNaN(column.Width))
                {
                    column.Width = column.ActualWidth;
                }

                column.Width = double.NaN;
            }
        }



        /// <summary>
        /// Opens 'ChooseUserWindow' and closes the current one.
        /// </summary>
        /// <param name="message">Empty message.</param>
        private void OnUserLogout(object recipient, UserLogoutMsg message)
        {
            _chooseUserWindow.Show();

            this.Close();
        }

        /// <summary>
        /// Disposes of everything / unregisters messages on window close.
        /// </summary>
        private void Window_Closed(object sender, System.EventArgs e)
        {
            _viewModel.Dispose();
            _messenger.UnregisterAll(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.WeatherList.CollectionChanged += WeatherListView_SizeChanged;
        }
    }
}
