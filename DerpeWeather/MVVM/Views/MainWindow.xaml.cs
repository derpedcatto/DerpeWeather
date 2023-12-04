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
        private readonly IMessenger _messenger;
        private readonly MainWindowVM _viewModel;



        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindow(
            MainWindowVM viewModel,
            IMessenger messenger)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;
            _messenger.Register<UserLogoutMsg>(this, OnUserLogout);
        }



        private void OnUserLogout(object recipient, UserLogoutMsg message)
        {
            this.Close();
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
