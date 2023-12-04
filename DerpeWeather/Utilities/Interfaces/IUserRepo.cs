using DerpeWeather.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        Task AddNewUserAsync([NotNull] UserRegistrationDTO user, ISystemPreferenceFetcher systemPreferenceFetcher);

        // READ
        ICollection<UserDTO> GetAllUsers();
        Task<ICollection<UserDTO>> GetAllUsersAsync();
        UserDTO? GetUser(Guid userId);
        UserDTO? GetUser(string username);
        bool IsUserPasswordValid(Guid userId, string hashedPassword);

        // UPDATE
        void UpdateUserPassword(Guid userId, string password);
        void UpdateUserPassword(string username, string password);
        void UpdateUserUsername(Guid userId, string username);
        void UpdateUserUsername(string oldUsername, string username);
        void AddNewTrackedLocation(Guid userId, string locationName);
        void AddNewTrackedLocation(string username, string locationName);

        // DELETE
        void DeleteUser(Guid userId);
        void DeleteUser(string username);
        void DeleteTrackedField(Guid userId, string locationName);
    }
}
