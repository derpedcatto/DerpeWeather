using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.ViewModels;
using DerpeWeather.Utilities.Messages;
using System;
using System.Windows;

namespace DerpeWeather.MVVM.Views
{
    /// <summary>
    /// Interaction logic for ChangeUsernameWindow.xaml
    /// </summary>
    public partial class ChangeUsernameWindow : Window
    {
        private readonly ChangeUsernameVM _viewModel;
        private readonly IMessenger _messenger;

        public bool IsUsernameChangeSuccess { get; private set; }



        public ChangeUsernameWindow(
            ChangeUsernameVM viewModel,
            IMessenger messenger)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;

            _messenger.Register<ChangeUsernameSuccessMsg>(this, OnUsernameChangeSuccessMsgReceived);
        }



        private void OnUsernameChangeSuccessMsgReceived(object recipient, ChangeUsernameSuccessMsg message)
        {
            IsUsernameChangeSuccess = true;
            this.Close();
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            _viewModel.Dispose();
            _messenger.UnregisterAll(this);
        }
    }
}
