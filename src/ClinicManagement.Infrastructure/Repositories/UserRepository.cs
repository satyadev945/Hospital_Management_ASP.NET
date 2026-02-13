using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for User entity
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ClinicManagementDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ClinicManagementDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all users");
            return await _context.Users
                .Where(u => u.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            throw;
        }
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting user by ID: {UserId}", id);
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by ID: {UserId}", id);
            throw;
        }
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting user by email: {Email}", email);
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by email: {Email}", email);
            throw;
        }
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new user: {Email}", user.Email);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User added successfully: {UserId}", user.Id);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding user: {Email}", user.Email);
            throw;
        }
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user: {UserId}", user.Id);
            user.ModifiedDate = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User updated successfully: {UserId}", user.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {UserId}", user.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user: {UserId}", id);
            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);

            if (user != null)
            {
                user.IsActive = false;
                user.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("User deleted successfully: {UserId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {UserId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Users
                .AnyAsync(u => u.Id == id && u.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking user existence: {UserId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users with term: {SearchTerm}", searchTerm);
            return await _context.Users
                .Where(u => u.IsActive && (u.Name.Contains(searchTerm) || u.Email.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<User?> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating login for email: {Email}", email);
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password && u.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating login for email: {Email}", email);
            throw;
        }
    }
}
