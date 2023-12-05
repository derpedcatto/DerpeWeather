using System.Windows;

namespace DerpeWeather.Utilities.Interfaces
{
    public interface IWindowFactory
    {
        T CreateWindow<T>() where T : Window;
        bool? ShowDialog<T>() where T : Window;
    }
}
