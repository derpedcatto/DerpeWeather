using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.ViewModels;
using DerpeWeather.Utilities.Messages;
using System;
using System.Windows;

namespace DerpeWeather.MVVM.Views
{
    /// <summary>
    /// Interaction logic for AppJsonSettingsWindow.xaml
    /// </summary>
    public partial class AppJsonSettingsWindow : Window, IDisposable
    {
        private readonly AppJsonSettingsVM _viewModel;
        private readonly IMessenger _messenger;
        private bool disposedValue;



        public AppJsonSettingsWindow(
            AppJsonSettingsVM viewModel,
            IMessenger messenger)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;

            _messenger.Register<ValidAppJsonSettingsMsg>(this, OnValidAppJsonSettingsMsgReceived);
        }



        private void OnValidAppJsonSettingsMsgReceived(object recipient, ValidAppJsonSettingsMsg message)
        {
            this.Dispose();
            this.Close();
        }



        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _viewModel.Dispose();
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
