using System.Windows;

namespace DerpeWeather.Utilities.Interfaces
{
    public interface IWindowService
    {
        void ShowWindow<T>() where T : Window;
        bool? ShowDialogWindow<T>() where T : Window;
    }
}
