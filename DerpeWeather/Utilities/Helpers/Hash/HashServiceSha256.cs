using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using DerpeWeather.Utilities.Interfaces;

namespace DerpeWeather.Utilities.Helpers.Hash
{
    /// <summary>
    /// Hashing service that implements <see cref="IHashService"/> interface.
    /// Uses `SHA-256` algorithm.
    /// </summary>
    public class HashServiceSha256 : IHashService
    {
        public string HashString(string source)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToHexString(
                sha256.ComputeHash(
                    Encoding.UTF8.GetBytes(source)
            ));
        }

        public string HashString(SecureString source)
        {
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(source);
            try
            {
                byte[] data = new byte[source.Length * 2];
                System.Runtime.InteropServices.Marshal.Copy(ptr, data, 0, data.Length);

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(data);
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        builder.Append(hashBytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
        }
    }
}
