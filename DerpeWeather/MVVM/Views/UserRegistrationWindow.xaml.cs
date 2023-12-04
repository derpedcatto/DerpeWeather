using DerpeWeather.Utilities.Interfaces;
using DerpeWeather.ViewModels;
using System.ComponentModel;
using System.Security;
using System.Windows;

namespace DerpeWeather.Views
{
    /// <summary>
    /// Interaction logic for UserRegistrationWindow.xaml
    /// </summary>
    public partial class UserRegistrationWindow : Window, IPasswordContainer
    {
        public SecureString Password => UserPasswordBox.SecurePassword;



        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserRegistrationWindow(UserRegistrationVM viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }



        /// <summary>
        /// Event handler that closes current window and sets 'DialogResult' to 'true' if <see cref="UserRegistrationVM.IsLoginSuccessful" is set to true./>
        /// </summary>
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserRegistrationVM.IsRegistrationSuccessful))
            {
                if ((sender as UserRegistrationVM).IsRegistrationSuccessful)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
        }



        public void Dispose()
        {
            Password.Dispose();
        }
    }
}
