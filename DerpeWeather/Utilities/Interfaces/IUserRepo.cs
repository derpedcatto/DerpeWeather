using DerpeWeather.DAL.DTO;
using DerpeWeather.MVVM.Models;
using DerpeWeather.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace DerpeWeather.Utilities.Interfaces
{
    /// <summary>
    /// DAL Repo with CRUD functions to work with database.
    /// </summary>
    public interface IUserRepo
    {
        // CREATE
        void AddNewUser([NotNull] UserRegistrationDTO user, ISystemPreferenceFetcher systemPreferenceFetcher);
        Task AddNewUserAsync([NotNull] UserRegistrationDTO user, ISystemPreferenceFetcher systemPreferenceFetcher, CancellationToken cancellationToken);

        // READ
        ICollection<User> GetAllUsers();
        Task<ICollection<User>> GetAllUsersAsync(CancellationToken cancellationToken);
        User? GetUser(Guid userId);
        User? GetUser(string username);
        Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<User?> GetUserAsync(string username, CancellationToken cancellationToken);
        bool IsUserPasswordValid(Guid userId, string hashedPassword);
        Task<bool> IsUserPasswordValidAsync(Guid userId, string hashedPassword, CancellationToken cancellationToken);

        // UPDATE
        void UpdateUserPassword(Guid userId, string password);
        void UpdateUserPassword(string username, string password);
        Task UpdateUserPasswordAsync(Guid userId, string password, CancellationToken cancellationToken);
        Task UpdateUserPasswordAsync(string username, string password, CancellationToken cancellationToken);
        void UpdateUserUsername(Guid userId, string username);
        void UpdateUserUsername(string oldUsername, string username);
        Task UpdateUserUsernameAsync(Guid userId, string username, CancellationToken cancellationToken);
        Task UpdateUserUsernameAsync(string oldUsername, string username, CancellationToken cancellationToken);
        void AddNewTrackedLocation(Guid userId, string locationName, string resolvedLocationName);
        void AddNewTrackedLocation(string username, string locationName, string resolvedLocationName);
        Task AddNewTrackedLocationAsync(Guid userId, string locationName, string resolvedLocationName, CancellationToken cancellationToken);
        Task AddNewTrackedLocationAsync(string username, string locationName, string resolvedLocationName, CancellationToken cancellationToken);
        void UpdateUserTheme(Guid userId, UserPreferenceTheme theme);
        void UpdateUserTheme(string username, UserPreferenceTheme theme);
        Task UpdateUserThemeAsync(Guid userId, UserPreferenceTheme theme, CancellationToken cancellationToken);
        Task UpdateUserThemeAsync(string username, UserPreferenceTheme theme, CancellationToken cancellationToken);
        void UpdateUserUnits(Guid userId, UserPreferenceUnits units);
        void UpdateUserUnits(string username, UserPreferenceUnits units);
        Task UpdateUserUnitsAsync(Guid userId, UserPreferenceUnits units, CancellationToken cancellationToken);
        Task UpdateUserUnitsAsync(string username, UserPreferenceUnits units, CancellationToken cancellationToken);

        // DELETE
        void DeleteUser(Guid userId);
        void DeleteUser(string username);
        Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
        Task DeleteUserAsync(string username, CancellationToken cancellationToken);

        void DeleteTrackedField(Guid userId, string locationName);
        Task DeleteTrackedFieldAsync(Guid userId, string locationName, CancellationToken cancellationToken);
    }
}