using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for User business logic operations
/// </summary>
public interface IUserService
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
    /// Creates a new user
    /// </summary>
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user
    /// </summary>
    Task UpdateAsync(int id, User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a user by ID
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches users by name or email
    /// </summary>
    Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates user login credentials
    /// </summary>
    Task<(bool isValid, User? user)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
