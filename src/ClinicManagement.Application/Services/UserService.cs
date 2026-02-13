using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Exceptions;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Services;

/// <summary>
/// Service implementation for User business logic
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all users from service");
            return await _userRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UserService.GetAllAsync");
            throw;
        }
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting user by ID from service: {UserId}", id);
            return await _userRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UserService.GetByIdAsync for ID: {UserId}", id);
            throw;
        }
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating user in service: {Email}", user.Email);

            var existingUser = await _userRepository.GetByEmailAsync(user.Email, cancellationToken);
            if (existingUser != null)
            {
                throw new ValidationException($"User with email {user.Email} already exists");
            }

            return await _userRepository.AddAsync(user, cancellationToken);
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UserService.CreateAsync for email: {Email}", user.Email);
            throw;
        }
    }

    public async Task UpdateAsync(int id, User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user in service: {UserId}", id);

            var existingUser = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (existingUser == null)
            {
                throw new EntityNotFoundException("User", id);
            }

            user.Id = id;
            await _userRepository.UpdateAsync(user, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UserService.UpdateAsync for ID: {UserId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user in service: {UserId}", id);

            var existingUser = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (existingUser == null)
            {
                throw new EntityNotFoundException("User", id);
            }

            await _userRepository.DeleteAsync(id, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UserService.DeleteAsync for ID: {UserId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users in service with term: {SearchTerm}", searchTerm);
            return await _userRepository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UserService.SearchAsync with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<(bool isValid, User? user)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating login in service for email: {Email}", email);
            var user = await _userRepository.ValidateLoginAsync(email, password, cancellationToken);
            return (user != null, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UserService.ValidateLoginAsync for email: {Email}", email);
            throw;
        }
    }
}
