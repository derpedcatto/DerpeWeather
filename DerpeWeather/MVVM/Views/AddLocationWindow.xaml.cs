using CommunityToolkit.Mvvm.Messaging;
using DerpeWeather.MVVM.ViewModels;
using DerpeWeather.Utilities.Messages;
using System.Windows;

namespace DerpeWeather.MVVM.Views
{
    /// <summary>
    /// Interaction logic for AddLocationWindow.xaml
    /// </summary>
    public partial class AddLocationWindow : Window
    {
        private readonly AddLocationVM _viewModel;
        private readonly IMessenger _messenger;



        public AddLocationWindow(AddLocationVM viewModel, IMessenger messenger)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _messenger = messenger;

            _messenger.Register<CloseAddLocationWindowMsg>(this, OnCloseAddLocationWindowMsgReceived);
        }



        private void OnCloseAddLocationWindowMsgReceived(object recipient, CloseAddLocationWindowMsg message)
        {
            this.Close();
        }


        private void Window_Closed(object sender, System.EventArgs e)
        {
            _messenger.UnregisterAll(this);
            _viewModel.Dispose();
        }
    }
}
