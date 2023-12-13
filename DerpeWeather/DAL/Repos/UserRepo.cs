using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DerpeWeather.DAL.DTO;
using DerpeWeather.DAL.EF;
using DerpeWeather.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using DerpeWeather.Utilities.Enums;
using DerpeWeather.MVVM.Models;

namespace DerpeWeather.DAL.Repos
{
    /// <summary>
    /// Main implementation of <see cref="IUserRepo"/> interface.
    /// </summary>
    public class UserRepo : IUserRepo
    {
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
                Theme = UserPreferenceTheme.SYSTEM,
                Units = systemPreferenceFetcher.GetUnitsPreference()
            };
            newUser.AppPreferences = userAppPreferences;

            List<UserTrackedWeatherFieldsEntity> userTrackedWeatherFieldsList = new();
            newUser.TrackedWeatherFields = userTrackedWeatherFieldsList;

            _dbContext.Add(newUser);
            _dbContext.SaveChanges();
        }

        public async Task AddNewUserAsync([NotNull] UserRegistrationDTO user, ISystemPreferenceFetcher systemPreferenceFetcher, CancellationToken cancellationToken)
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
                Theme = UserPreferenceTheme.SYSTEM,
                Units = systemPreferenceFetcher.GetUnitsPreference()
            };
            newUser.AppPreferences = userAppPreferences;

            var userTrackedWeatherFieldsList = new List<UserTrackedWeatherFieldsEntity>();
            newUser.TrackedWeatherFields = userTrackedWeatherFieldsList;

            _dbContext.Add(newUser);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        #endregion



        #region READ

        public ICollection<User> GetAllUsers()
        {
            return _dbContext.Users
                .Include(user => user.AppPreferences)
                .Include(user => user.TrackedWeatherFields)
                .Select(userDB => ConvertToUserDTO(userDB))
                .ToList();
        }



        public async Task<ICollection<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Include(user => user.AppPreferences)
                .Include(user => user.TrackedWeatherFields)
                .Select(userDB => ConvertToUserDTO(userDB))
                .ToListAsync(cancellationToken: cancellationToken);
        }



        public User? GetUser(Guid userId)
        {
            var userEntity = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (userEntity == null) { return null; }

            return ConvertToUserDTO(userEntity);
        }

        public User? GetUser(string username)
        {
            var userEntity = _dbContext.Users.FirstOrDefault(x => x.Username == username);
            if (userEntity == null) { return null; }

            return ConvertToUserDTO(userEntity);
        }



        public async Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken: cancellationToken);
            if (userEntity == null)
            {
                return null;
            }
            return ConvertToUserDTO(userEntity);
        }

        public async Task<User?> GetUserAsync(string username, CancellationToken cancellationToken)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken: cancellationToken);
            if (userEntity == null)
            {
                return null;
            }
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


        public async Task<bool> IsUserPasswordValidAsync(Guid userId, string hashedPassword, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(i => i.Id == userId, cancellationToken: cancellationToken);
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



        private async Task UpdateUserPasswordAsync(UserEntity? existingUser, string password, CancellationToken cancellationToken)
        {
            if (existingUser != null)
            {
                existingUser.Password = password;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateUserPasswordAsync(Guid userId, string password, CancellationToken cancellationToken)
        {
            UserEntity? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);
            await UpdateUserPasswordAsync(existingUser, password, cancellationToken);
        }

        public async Task UpdateUserPasswordAsync(string username, string password, CancellationToken cancellationToken)
        {
            UserEntity? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken: cancellationToken);
            await UpdateUserPasswordAsync(existingUser, password, cancellationToken);
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



        private async Task UpdateUserUsernameAsync(UserEntity? existingUser, string username, CancellationToken cancellationToken)
        {
            if (existingUser != null)
            {
                existingUser.Username = username;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateUserUsernameAsync(Guid userId, string username, CancellationToken cancellationToken)
        {
            UserEntity? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);
            await UpdateUserUsernameAsync(existingUser, username, cancellationToken);
        }

        public async Task UpdateUserUsernameAsync(string oldUsername, string username, CancellationToken cancellationToken)
        {
            UserEntity? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == oldUsername, cancellationToken: cancellationToken);
            await UpdateUserUsernameAsync(existingUser, username, cancellationToken);
        }



        private void AddNewTrackedLocation(UserEntity? existingUser, string locationName, string resolvedLocationName)
        {
            if (existingUser != null)
            {
                // _dbContext.Entry(existingUser).Reload();
                _dbContext.Entry(existingUser).Reload();
                var field = new UserTrackedWeatherFieldsEntity()
                {
                    Location = locationName,
                    ResolvedLocation = resolvedLocationName,
                    UserId = existingUser.Id,
                    User = existingUser
                };
                existingUser.TrackedWeatherFields.Add(field);
                _dbContext.SaveChanges();
            }
        }

        public void AddNewTrackedLocation(Guid userId, string locationName, string resolvedLocationName)
        {
            UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            AddNewTrackedLocation(user, locationName, resolvedLocationName);
        }

        public void AddNewTrackedLocation(string username, string locationName, string resolvedLocationName)
        {
            UserEntity? user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            AddNewTrackedLocation(user, locationName, resolvedLocationName);
        }



        private async Task AddNewTrackedLocationAsync(UserEntity? existingUser, string locationName, string resolvedLocationName, CancellationToken cancellationToken)
        {
            if (existingUser != null)
            {
                // _dbContext.Entry(existingUser).Reload();
                _dbContext.Entry(existingUser);
                var field = new UserTrackedWeatherFieldsEntity()
                {
                    Location = locationName,
                    ResolvedLocation = resolvedLocationName,
                    UserId = existingUser.Id,
                    User = existingUser
                };
                existingUser.TrackedWeatherFields.Add(field);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task AddNewTrackedLocationAsync(Guid userId, string locationName, string resolvedLocationName, CancellationToken cancellationToken)
        {
            UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);
            await AddNewTrackedLocationAsync(user, locationName, resolvedLocationName, cancellationToken);
        }

        public async Task AddNewTrackedLocationAsync(string username, string locationName, string resolvedLocationName, CancellationToken cancellationToken)
        {
            UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken: cancellationToken);
            await AddNewTrackedLocationAsync(user, locationName, resolvedLocationName, cancellationToken);
        }



        private void UpdateUserTheme(UserEntity? existingUser, UserPreferenceTheme theme)
        {
            if (existingUser != null)
            {
                existingUser.AppPreferences.Theme = theme;
                _dbContext.SaveChanges();
            }
        }

        public void UpdateUserTheme(Guid userId, UserPreferenceTheme theme)
        {
            UserEntity? existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            UpdateUserTheme(existingUser, theme);
        }

        public void UpdateUserTheme(string username, UserPreferenceTheme theme)
        {
            UserEntity? existingUser = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            UpdateUserTheme(existingUser, theme);
        }


        private async Task UpdateUserThemeAsync(UserEntity? existingUser, UserPreferenceTheme theme, CancellationToken cancellationToken)
        {
            if (existingUser != null)
            {
                existingUser.AppPreferences.Theme = theme;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateUserThemeAsync(Guid userId, UserPreferenceTheme theme, CancellationToken cancellationToken)
        {
            UserEntity? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);
            await UpdateUserThemeAsync(existingUser, theme, cancellationToken);
        }

        public async Task UpdateUserThemeAsync(string username, UserPreferenceTheme theme, CancellationToken cancellationToken)
        {
            UserEntity? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken: cancellationToken);
            await UpdateUserThemeAsync(existingUser, theme, cancellationToken);
        }



        private void UpdateUserUnits(UserEntity? existingUser, UserPreferenceUnits units)
        {
            if (existingUser != null)
            {
                existingUser.AppPreferences.Units = units;
                _dbContext.SaveChanges();
            }
        }

        public void UpdateUserUnits(Guid userId, UserPreferenceUnits units)
        {
            UserEntity? existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            UpdateUserUnits(existingUser, units);
        }

        public void UpdateUserUnits(string username, UserPreferenceUnits units)
        {
            UserEntity? existingUser = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            UpdateUserUnits(existingUser, units);
        }



        private async Task UpdateUserUnitsAsync(UserEntity? existingUser, UserPreferenceUnits units, CancellationToken cancellationToken)
        {
            if (existingUser != null)
            {
                existingUser.AppPreferences.Units = units;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateUserUnitsAsync(Guid userId, UserPreferenceUnits units, CancellationToken cancellationToken)
        {
            UserEntity? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);
            await UpdateUserUnitsAsync(existingUser, units, cancellationToken);
        }

        public async Task UpdateUserUnitsAsync(string username, UserPreferenceUnits units, CancellationToken cancellationToken)
        {
            UserEntity? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken: cancellationToken);
            await UpdateUserUnitsAsync(existingUser, units, cancellationToken);
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



        private async Task DeleteUserAsync(UserEntity? userEntity, CancellationToken cancellationToken)
        {
            if (userEntity != null)
            {
                _dbContext.Users.Remove(userEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            UserEntity? userToDelete = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);
            await DeleteUserAsync(userToDelete, cancellationToken);
        }

        public async Task DeleteUserAsync(string username, CancellationToken cancellationToken)
        {
            UserEntity? userToDelete = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken: cancellationToken);
            await DeleteUserAsync(userToDelete, cancellationToken);
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



        public async Task DeleteTrackedFieldAsync(Guid userId, string locationName, CancellationToken cancellationToken)
        {
            UserEntity? user = await _dbContext.Users.Where(i => i.Id == userId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (user != null)
            {
                var trackedField = user.TrackedWeatherFields
                    .Where(n => n.Location == locationName)
                    .FirstOrDefault();
                if (trackedField != null)
                {
                    user.TrackedWeatherFields.Remove(trackedField);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        #endregion



        #region Helpers

        private static User ConvertToUserDTO(UserEntity userEntity)
        {
            var userDTO = new User()
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
                        Location = field.Location,
                        ResolvedLocation = field.ResolvedLocation
                    })
                    .ToList()
            };

            return userDTO;
        }

        #endregion
    }
}
