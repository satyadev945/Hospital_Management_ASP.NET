using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for LoginTable entity operations
    /// </summary>
    public interface ILoginRepository
    {
        /// <summary>
        /// Retrieves all login records from the database
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of all login records</returns>
        Task<IEnumerable<Login>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a login record by its unique identifier
        /// </summary>
        /// <param name="id">Login identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Login entity if found, null otherwise</returns>
        Task<Login> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new login record to the database
        /// </summary>
        /// <param name="login">Login entity to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The added login record with generated ID</returns>
        Task<Login> AddAsync(Login login, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing login record in the database
        /// </summary>
        /// <param name="login">Login entity with updated information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated login entity</returns>
        Task<Login> UpdateAsync(Login login, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a login record from the database
        /// </summary>
        /// <param name="id">Login identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a login record exists in the database
        /// </summary>
        /// <param name="id">Login identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if login record exists, false otherwise</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for login records by email
        /// </summary>
        /// <param name="searchTerm">Search term for email</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of matching login records</returns>
        Task<IEnumerable<Login>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates user login credentials
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Login entity if credentials are valid, null otherwise</returns>
        Task<Login> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a login record by email address
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Login entity if found, null otherwise</returns>
        Task<Login> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an email already exists in the database
        /// </summary>
        /// <param name="email">Email address to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if email exists, false otherwise</returns>
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="id">Login identifier</param>
        /// <param name="newPassword">New password</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdatePasswordAsync(int id, string newPassword, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates user password by email
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="newPassword">New password</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdatePasswordByEmailAsync(string email, string newPassword, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves login records by user type
        /// </summary>
        /// <param name="type">User type (1-Patient, 2-Doctor, 3-Admin)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of login records with specified type</returns>
        Task<IEnumerable<Login>> GetByTypeAsync(int type, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves count of users by type
        /// </summary>
        /// <param name="type">User type (1-Patient, 2-Doctor, 3-Admin)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Count of users with specified type</returns>
        Task<int> GetCountByTypeAsync(int type, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves total count of login records
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Total number of login records</returns>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Changes user email address
        /// </summary>
        /// <param name="id">Login identifier</param>
        /// <param name="newEmail">New email address</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdateEmailAsync(int id, string newEmail, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifies if password matches for a given login ID
        /// </summary>
        /// <param name="id">Login identifier</param>
        /// <param name="password">Password to verify</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if password matches, false otherwise</returns>
        Task<bool> VerifyPasswordAsync(int id, string password, CancellationToken cancellationToken = default);
    }
}
