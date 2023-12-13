using System.Windows;

namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// Provides methods for creating and displaying windows.
    /// </summary>
    public interface IWindowFactory
    {
        /// <summary>
        /// Creates a new window of type T (a subclass of Window).
        /// </summary>
        /// <param name="parentWindow">The parent window of the window to be created.</param>
        /// <returns>A new window of type T.</returns>
        T CreateWindow<T>(Window parentWindow) where T : Window;
    }
}
