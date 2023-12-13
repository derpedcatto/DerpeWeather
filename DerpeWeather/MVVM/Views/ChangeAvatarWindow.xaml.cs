using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.ViewModels;
using DerpeWeather.Utilities.Messages;
using System;
using System.Windows;

namespace DerpeWeather.MVVM.Views
{
    /// <summary>
    /// Interaction logic for ChangeAvatarWindow.xaml
    /// </summary>
    public partial class ChangeAvatarWindow : Window
    {
        private readonly ChangeAvatarVM _viewModel;
        private readonly IMessenger _messenger;

        public bool IsAvatarChangeSuccess { get; private set; }



        public ChangeAvatarWindow(
            ChangeAvatarVM viewModel,
            IMessenger messenger)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;

            _messenger.Register<ChangeAvatarSuccessMsg>(this, OnChangeAvatarSuccessMsgReceived);
        }



        private void OnChangeAvatarSuccessMsgReceived(object recipient, ChangeAvatarSuccessMsg message)
        {
            IsAvatarChangeSuccess = true;
            this.Close();
        }



        private void Window_Closed(object sender, System.EventArgs e)
        {
            _viewModel.Dispose();
            _messenger.UnregisterAll(this);
        }
    }
}
