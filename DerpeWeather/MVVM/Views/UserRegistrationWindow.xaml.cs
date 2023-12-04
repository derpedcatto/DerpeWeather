using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.ViewModels;
using System;
using System.Security;
using System.Windows;

namespace DerpeWeather.Views
{
    /// <summary>
    /// Interaction logic for UserRegistrationWindow.xaml
    /// </summary>
    public partial class UserRegistrationWindow : Window, IPasswordContainer, IDisposable
    {
        private readonly IMessenger _messenger;
        private readonly UserRegistrationVM _viewModel;
        private bool disposedValue;

        public SecureString Password => UserPasswordBox.SecurePassword;
        public bool IsRegistrationSuccessful { get; private set; }



        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserRegistrationWindow(UserRegistrationVM viewModel, IMessenger messenger)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;
            _messenger.Register<RegistrationSuccessMsg>(this, OnRegistrationSuccessfulMsgReceived);
        }



        private void OnRegistrationSuccessfulMsgReceived(object recipient, RegistrationSuccessMsg message)
        {
            IsRegistrationSuccessful = true;
            this.Close();
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            _viewModel.Dispose();
            _messenger.UnregisterAll(this);
            this.Dispose();
        }



        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Password.Dispose();
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
