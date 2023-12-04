using DerpeWeather.Utilities.Interfaces;
using System;
using System.Windows;

namespace DerpeWeather.Utilities.Helpers.View
{
    public class WindowService : IWindowService
    {
        public void ShowWindow<T>() where T : Window
        {
            var window = Activator.CreateInstance<T>();
            window.Show();
        }

        public bool? ShowDialogWindow<T>() where T : Window
        {
            var window = Activator.CreateInstance<T>();
            return window.ShowDialog();
        }
    }
}
