using DerpeWeather.MVVM.ViewModels;
using System.Windows;

namespace DerpeWeather.MVVM.Views
{
    /// <summary>
    /// Interaction logic for UserPreferencesWindow.xaml
    /// </summary>
    public partial class UserPreferencesWindow : Window
    {
        public UserPreferencesWindow(UserPreferencesVM viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
