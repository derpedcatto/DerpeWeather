using DerpeWeather.Utilities.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace DerpeWeather.Utilities.Helpers.View
{
    /// <summary>
    /// Main implementation of <see cref="IWindowFactory"/> interface.
    /// </summary>
    public class WindowFactory : IWindowFactory
    {
        private readonly IServiceProvider _serviceProvider;



        public WindowFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }



        public T CreateWindow<T>(Window parentWindow = null) where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            PositionWindowRelativeToParent(window, parentWindow);
            return window;
        }



        /// <summary>
        /// Helper function that helps position a window to center center relative to its parent.
        /// </summary>
        private void PositionWindowRelativeToParent(Window window, Window? parentWindow)
        {
            if (parentWindow != null)
            {
                window.WindowStartupLocation = WindowStartupLocation.Manual;
                window.Left = parentWindow.Left + (parentWindow.Width - window.Width) / 2;
                window.Top = parentWindow.Top + (parentWindow.Height - window.Height) / 2;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }
    }
}
