using DerpeWeather.MVVM.ViewModels;
using System;
using System.Windows;

namespace DerpeWeather.MVVM.Views
{
    /// <summary>
    /// Interaction logic for UserPreferencesWindow.xaml
    /// </summary>
    public partial class UserPreferencesWindow : Window, IDisposable
    {
        private readonly UserPreferencesVM _viewModel;
        private bool disposedValue;



        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserPreferencesWindow(UserPreferencesVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }



        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _viewModel.Dispose();
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
