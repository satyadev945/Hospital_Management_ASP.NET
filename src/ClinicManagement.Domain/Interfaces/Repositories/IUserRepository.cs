using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for User entity operations
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets all users
    /// </summary>
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by email
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new user
    /// </summary>
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user
    /// </summary>
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a user by ID
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user exists by ID
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches users by name or email
    /// </summary>
    Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates user login credentials
    /// </summary>
    Task<User?> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
