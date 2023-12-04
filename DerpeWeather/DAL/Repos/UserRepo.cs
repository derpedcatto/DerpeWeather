using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DerpeWeather.DAL.DTO;
using DerpeWeather.DAL.EF;
using DerpeWeather.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using DerpeWeather.MVVM.Models;

namespace DerpeWeather.DAL.Repos
{
    public class UserRepo : IUserRepo
    {
        // private bool disposedValue;
        private readonly EFContext _dbContext;



        public UserRepo(EFContext dbContext)
        {
            _dbContext = dbContext;
        }



        #region CREATE

        public void AddNewUser([NotNull] UserRegistrationDTO user, ISystemPreferenceFetcher systemPreferenceFetcher)
        {
            UserEntity newUser = new()
            {
                Id = Guid.NewGuid(),
                Username = user.Username,
                Password = user.Password
            };

            // Default app preferences
            UserAppPreferencesEntity userAppPreferences = new()
            {
                Id = Guid.NewGuid(),
                Theme = systemPreferenceFetcher.GetThemePreference(),
                Units = systemPreferenceFetcher.GetUnitsPreference()
            };
            newUser.AppPreferences = userAppPreferences;

            List<UserTrackedWeatherFieldsEntity> userTrackedWeatherFieldsList = new();
            newUser.TrackedWeatherFields = userTrackedWeatherFieldsList;

            _dbContext.Add(newUser);
            _dbContext.SaveChanges();
        }

        public async Task AddNewUserAsync([NotNull] UserRegistrationDTO user, ISystemPreferenceFetcher systemPreferenceFetcher)
        {
            UserEntity newUser = new()
            {
                Id = Guid.NewGuid(),
                Username = user.Username,
                Password = user.Password
            };

            // Default app preferences
            UserAppPreferencesEntity userAppPreferences = new()
            {
                Id = Guid.NewGuid(),
                Theme = systemPreferenceFetcher.GetThemePreference(),
                Units = systemPreferenceFetcher.GetUnitsPreference()
            };
            newUser.AppPreferences = userAppPreferences;

            var userTrackedWeatherFieldsList = new List<UserTrackedWeatherFieldsEntity>();
            newUser.TrackedWeatherFields = userTrackedWeatherFieldsList;

            _dbContext.Add(newUser);
            await _dbContext.SaveChangesAsync();
        }

        #endregion



        #region READ

        public ICollection<UserDTO> GetAllUsers()
        {
            return _dbContext.Users
                .Include(user => user.AppPreferences)
                .Include(user => user.TrackedWeatherFields)
                .Select(userDB => ConvertToUserDTO(userDB))
                .ToList();
        }

        public async Task<ICollection<UserDTO>> GetAllUsersAsync()
        {
            return await _dbContext.Users
                .Include(user => user.AppPreferences)
                .Include(user => user.TrackedWeatherFields)
                .Select(userDB => ConvertToUserDTO(userDB))
                .ToListAsync();
        }

        public UserDTO? GetUser(Guid userId)
        {
            var userEntity = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (userEntity == null) { return null; }

            return ConvertToUserDTO(userEntity);
        }

        public UserDTO? GetUser(string username)
        {
            var userEntity = _dbContext.Users.FirstOrDefault(x => x.Username == username);
            if (userEntity == null) { return null; }

            return ConvertToUserDTO(userEntity);
        }

        public bool IsUserPasswordValid(Guid userId, string hashedPassword)
        {
            var user = _dbContext.Users.FirstOrDefault(i => i.Id == userId);
            if (user != null)
            {
                if (user.Password == hashedPassword)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion



        #region UPDATE

        private void UpdateUserPassword(UserEntity? existingUser, string password)
        {
            if (existingUser != null)
            {
                existingUser.Password = password;
                _dbContext.SaveChanges();
            }
        }

        public void UpdateUserPassword(Guid userId, string password)
        {
            UserEntity? existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            UpdateUserPassword(existingUser, password);
        }

        public void UpdateUserPassword(string username, string password)
        {
            UserEntity? existingUser = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            UpdateUserPassword(existingUser, password);
        }


        private void UpdateUserUsername(UserEntity? existingUser, string username)
        {
            if (existingUser != null)
            {
                existingUser.Username = username;
                _dbContext.SaveChanges();
            }
        }

        public void UpdateUserUsername(Guid userId, string username)
        {
            UserEntity? existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            UpdateUserUsername(existingUser, username);
        }

        public void UpdateUserUsername(string oldUsername, string username)
        {
            UserEntity? existingUser = _dbContext.Users.FirstOrDefault(u => u.Username == oldUsername);
            UpdateUserUsername(existingUser, username);
        }


        private void AddNewTrackedLocation(UserEntity? existingUser, string locationName)
        {
            if (existingUser != null)
            {
                _dbContext.Entry(existingUser).Reload();
                var field = new UserTrackedWeatherFieldsEntity()
                {
                    Location = locationName,
                    UserId = existingUser.Id,
                    User = existingUser
                };
                existingUser.TrackedWeatherFields.Add(field);
                _dbContext.SaveChanges();
            }
        }

        public void AddNewTrackedLocation(Guid userId, string locationName)
        {
            UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            AddNewTrackedLocation(user, locationName);
        }

        public void AddNewTrackedLocation(string username, string locationName)
        {
            UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            AddNewTrackedLocation(user, locationName);
        }

        #endregion



        #region DELETE

        private void DeleteUser(UserEntity? userEntity)
        {
            if (userEntity != null)
            {
                _dbContext.Users.Remove(userEntity);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteUser(Guid userId)
        {
            UserEntity? userToDelete = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            DeleteUser(userToDelete);
        }

        public void DeleteUser(string username)
        {
            UserEntity? userToDelete = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            DeleteUser(userToDelete);
        }


        public void DeleteTrackedField(Guid userId, string locationName)
        {
            UserEntity? user = _dbContext.Users.Where(i => i.Id == userId).FirstOrDefault();
            if (user != null)
            {
                var trackedField =
                    user.TrackedWeatherFields
                    .Where(n => n.Location == locationName)
                    .FirstOrDefault();

                if (trackedField != null)
                {
                    user.TrackedWeatherFields.Remove(trackedField);
                    _dbContext.SaveChanges();
                }
            }
        }

        #endregion



        #region Helpers

        private static UserDTO ConvertToUserDTO(UserEntity userEntity)
        {
            var userDTO = new UserDTO()
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                Preferences = new MVVM.Models.UserAppPreferences
                {
                    Theme = userEntity.AppPreferences.Theme,
                    Units = userEntity.AppPreferences.Units
                },
                TrackedWeatherFields = userEntity.TrackedWeatherFields
                    .Select(field => new MVVM.Models.UserTrackedWeatherField
                    {
                        Id = field.Id,
                        Location = field.Location
                    })
                    .ToList()
            };

            return userDTO;
        }

        #endregion



        #region Dispose
        /*
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        */
        #endregion
    }
}
