using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.ViewModels;
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
        private readonly UserLoginVM _viewModel;
        public SecureString Password => UserPasswordBox.SecurePassword;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserLoginWindow(UserLoginVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }



        /// <summary>
        /// Event handler that closes current window and sets 'DialogResult' to 'true' if <see cref="UserLoginVM.IsLoginSuccessful" is set to true./>
        /// </summary>
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserLoginVM.IsLoginSuccessful))
            {
                if ((sender as UserLoginVM).IsLoginSuccessful)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
        }



        #region Dispose
        public void Dispose()
        {
            Password.Dispose();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Dispose();
            _viewModel.Dispose();
        }
        #endregion
    }
}
