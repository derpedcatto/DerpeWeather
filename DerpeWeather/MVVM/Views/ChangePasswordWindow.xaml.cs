using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.ViewModels;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.ViewModels;
using System;
using System.Security;
using System.Windows;

namespace DerpeWeather.MVVM.Views
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window, IPasswordContainer
    {
        private readonly ChangePasswordVM _viewModel;
        private readonly IMessenger _messenger;
        private bool disposedValue;

        public SecureString Password => UserPasswordBox.SecurePassword;

        public bool IsPasswordChangeSuccess { get; private set; }



        public ChangePasswordWindow(
            ChangePasswordVM viewModel,
            IMessenger messenger)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;

            _messenger.Register<ChangePasswordSuccessMsg>(this, OnChangePasswordSuccessMsgReceived);
        }



        private void OnChangePasswordSuccessMsgReceived(object recipient, ChangePasswordSuccessMsg message)
        {
            IsPasswordChangeSuccess = true;
            this.Close();
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            _viewModel.Dispose();
            this.Dispose();
        }



        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Password?.Dispose();
                    _messenger.UnregisterAll(this);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }

        #endregion
    }
}
