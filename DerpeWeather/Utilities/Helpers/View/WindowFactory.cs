using DerpeWeather.Utilities.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace DerpeWeather.Utilities.Helpers.View
{
    public class WindowFactory : IWindowFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T CreateWindow<T>() where T : Window
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public bool? ShowDialog<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            return window.ShowDialog();
        }
    }
}
