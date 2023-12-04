using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.ViewModels;
using System;
using System.ComponentModel;
using System.Security;
using System.Windows;

namespace DerpeWeather.Views
{
    /// <summary>
    /// Interaction logic for UserLoginWindow.xaml
    /// </summary>
    public partial class UserLoginWindow : Window, IPasswordContainer
    {
        private readonly IMessenger _messenger;
        private readonly UserLoginVM _viewModel;

        public SecureString Password => UserPasswordBox.SecurePassword;

        public bool IsLoginSuccessful { get; private set; }



        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserLoginWindow(UserLoginVM viewModel, IMessenger messenger)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;
            _messenger.Register<LoginSuccessMsg>(this, OnLoginSuccess);
        }



        private void OnLoginSuccess(object recipient, LoginSuccessMsg message)
        {
            this.IsLoginSuccessful = true;
            this.Close();
        }



        private void Window_Closed(object sender, System.EventArgs e)
        {
            Dispose();
            _viewModel.Dispose();
            _messenger.UnregisterAll(this);
        }



        public void Dispose()
        {
            Password.Dispose();
        }
    }
}
