using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DerpeWeather.Views
{
    /// <summary>
    /// Interaction logic for ChooseUserWindow.xaml
    /// </summary>
    public partial class ChooseUserWindow : Window
    {
        private readonly ChooseUserVM _viewModel;
        private readonly IMessenger _messenger;



        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChooseUserWindow(ChooseUserVM viewModel, IMessenger messenger)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;
            // _messenger.Register<OpenMainWindowMsg>(this, OnOpenMainWindow);
        }



        /// <summary>
        /// On <see cref="OpenMainWindowMsg"passing/> received - closes current window and opens <see cref="MainWindow"/>,
        /// sending message <see cref="UserLoginIdMsg"/> with <paramref name="userIdMsg"/> value to it.
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="userIdMsg"></param>
        /*private void OnOpenMainWindow(object recipient, OpenMainWindowMsg userIdMsg)
        {
            var mainWindow = App.Current.Services.GetService<MainWindow>()!;
            _messenger.Send(new UserLoginIdMsg(userIdMsg.Value));
            mainWindow.Show();
            this.Close();
        }
        */



        /// <summary>
        /// Disposes of everything / unregisters messages on window close.
        /// </summary>
        private void Window_Closed(object sender, System.EventArgs e)
        {
            _viewModel.Dispose();
            _messenger.UnregisterAll(this);
        }
    }
}
