using DerpeWeather.DAL.DTO;
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
        ICollection<UserDTO> GetAllUsers();
        Task<ICollection<UserDTO>> GetAllUsersAsync(CancellationToken cancellationToken);
        UserDTO? GetUser(Guid userId);
        UserDTO? GetUser(string username);
        Task<UserDTO?> GetUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserDTO?> GetUserAsync(string username, CancellationToken cancellationToken);
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
        void AddNewTrackedLocation(Guid userId, string locationName);
        void AddNewTrackedLocation(string username, string locationName);
        Task AddNewTrackedLocationAsync(Guid userId, string locationName, CancellationToken cancellationToken);
        Task AddNewTrackedLocationAsync(string username, string locationName, CancellationToken cancellationToken);

        // DELETE
        void DeleteUser(Guid userId);
        void DeleteUser(string username);
        Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
        Task DeleteUserAsync(string username, CancellationToken cancellationToken);

        void DeleteTrackedField(Guid userId, string locationName);
        Task DeleteTrackedFieldAsync(Guid userId, string locationName, CancellationToken cancellationToken);
    }
}