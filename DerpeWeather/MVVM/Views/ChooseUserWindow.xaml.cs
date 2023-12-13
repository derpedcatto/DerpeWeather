using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.Utilities.Messages;
using DerpeWeather.ViewModels;
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
            App.Current.MainWindow = this;

            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;
            _messenger.Register<CloseChooseUserWndMsg>(this, CloseChooseUserWindowMsgReceived);
        }



        private void CloseChooseUserWindowMsgReceived(object recipient, CloseChooseUserWndMsg message)
        {
            this.Close();
        }



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
