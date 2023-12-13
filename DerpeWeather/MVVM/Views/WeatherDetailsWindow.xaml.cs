using DerpeWeather.MVVM.ViewModels;
using System;
using System.Windows;

namespace DerpeWeather.MVVM.Views
{
    /// <summary>
    /// Interaction logic for WeatherDetailsWindow.xaml
    /// </summary>
    public partial class WeatherDetailsWindow : Window
    {
        private readonly WeatherDetailsVM _viewModel;



        public WeatherDetailsWindow(WeatherDetailsVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            _viewModel.Dispose();
        }
    }
}
