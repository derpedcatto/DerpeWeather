using System;
using System.Security;

namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// Password container for helping to track PasswordBox SecureString input in view without binding.
    /// </summary>
    public interface IPasswordContainer : IDisposable
    {
        public SecureString Password { get; }
    }
}
