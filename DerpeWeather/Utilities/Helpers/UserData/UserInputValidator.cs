using DerpeWeather.Utilities.Interfaces;
using System;
using System.IO;
using System.Security;

namespace DerpeWeather.Utilities.Helpers.UserData
{
    /// <summary>
    /// Main implementation of <see cref="IUserInputValidator"/> interface.
    /// </summary>
    public class UserInputValidator : IUserInputValidator
    {
        private readonly IUserRepo _userRepo;



        public UserInputValidator(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }



        public string CheckUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return "Username must not be empty!";
            }

            username = username.Trim();
            if (username.Length < 3 || username.Length > 20)
            {
                return "Username must be 3-20 characters long.";
            }

            if (_userRepo.GetUser(username) != null)
            {
                return "User with this username already exists.";
            }

            return string.Empty;
        }

        public string CheckPassword(SecureString password)
        {
            if (password == null || password.Length < 3 || password.Length > 12)
            {
                return "Password must be 3-12 characters long.";
            }

            return string.Empty;
        }

        public string CheckAvatar(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return "FileExists Error (Path is not valid or File does not exist!)";
            }

            try
            {
                using FileStream fileStream = File.Open(filePath, FileMode.Open);
            }
            catch (IOException ex)
            {
                return $"PathIsValid Error ({ex.Message})";
            }

            return string.Empty;
        }

        public string CheckLocationName(Guid userId, string locationName, string resolvedLocation)
        {
            if (string.IsNullOrEmpty(locationName))
            {
                return "Location name is empty!";
            }

            var user = _userRepo.GetUser(userId);
            if (user != null)
            {
                foreach (var item in user.TrackedWeatherFields)
                {
                    if (string.Equals(item.ResolvedLocation, resolvedLocation, StringComparison.OrdinalIgnoreCase))
                    {
                        return "Location is already tracked.";
                    }
                }
            }
            else
            {
                return "User is null.";
            }


            return string.Empty;
        }
    }
}
