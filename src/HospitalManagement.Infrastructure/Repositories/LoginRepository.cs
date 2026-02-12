using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;

namespace HospitalManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Login entity operations
    /// </summary>
    public class LoginRepository : ILoginRepository
    {
        private readonly HospitalDbContext _context;
        private readonly ILogger<LoginRepository> _logger;

        public LoginRepository(HospitalDbContext context, ILogger<LoginRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Login>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving all login records");
                return await _context.Logins
                    .AsNoTracking()
                    .Where(l => l.IsActive)
                    .OrderBy(l => l.Email)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all login records");
                throw;
            }
        }

        public async Task<Login> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving login record with ID: {LoginId}", id);
                var login = await _context.Logins
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l => l.LoginID == id && l.IsActive, cancellationToken);

                if (login == null)
                {
                    _logger.LogWarning("Login record with ID {LoginId} not found", id);
                }

                return login;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving login record with ID: {LoginId}", id);
                throw;
            }
        }

        public async Task<Login> AddAsync(Login login, CancellationToken cancellationToken = default)
        {
            try
            {
                if (login == null)
                {
                    throw new ArgumentNullException(nameof(login));
                }

                _logger.LogInformation("Adding new login record for email: {Email}", login.Email);

                // Check if email already exists
                var existingLogin = await _context.Logins
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l => l.Email.ToLower() == login.Email.ToLower(), cancellationToken);

                if (existingLogin != null)
                {
                    _logger.LogWarning("Email {Email} already exists", login.Email);
                    throw new InvalidOperationException($"Email {login.Email} already exists");
                }

                login.CreatedDate = DateTime.UtcNow;
                login.IsActive = true;
                login.FailedLoginAttempts = 0;

                await _context.Logins.AddAsync(login, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Login record added successfully with ID: {LoginId}", login.LoginID);
                return login;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding login record for email: {Email}", login?.Email);
                throw;
            }
        }

        public async Task<Login> UpdateAsync(Login login, CancellationToken cancellationToken = default)
        {
            try
            {
                if (login == null)
                {
                    throw new ArgumentNullException(nameof(login));
                }

                _logger.LogInformation("Updating login record with ID: {LoginId}", login.LoginID);

                var existingLogin = await _context.Logins
                    .FirstOrDefaultAsync(l => l.LoginID == login.LoginID, cancellationToken);

                if (existingLogin == null)
                {
                    _logger.LogWarning("Login record with ID {LoginId} not found for update", login.LoginID);
                    throw new InvalidOperationException($"Login record with ID {login.LoginID} not found");
                }

                existingLogin.Email = login.Email;
                existingLogin.Password = login.Password;
                existingLogin.Type = login.Type;
                existingLogin.IsActive = login.IsActive;
                existingLogin.FailedLoginAttempts = login.FailedLoginAttempts;
                existingLogin.LockoutEnd = login.LockoutEnd;
                existingLogin.ModifiedDate = DateTime.UtcNow;
                existingLogin.ModifiedBy = login.ModifiedBy;

                _context.Logins.Update(existingLogin);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Login record updated successfully with ID: {LoginId}", login.LoginID);
                return existingLogin;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating login record with ID: {LoginId}", login?.LoginID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting login record with ID: {LoginId}", id);

                var login = await _context.Logins
                    .FirstOrDefaultAsync(l => l.LoginID == id, cancellationToken);

                if (login == null)
                {
                    _logger.LogWarning("Login record with ID {LoginId} not found for deletion", id);
                    return false;
                }

                // Soft delete
                login.IsActive = false;
                login.ModifiedDate = DateTime.UtcNow;

                _context.Logins.Update(login);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Login record deleted successfully with ID: {LoginId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting login record with ID: {LoginId}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Checking if login record exists with ID: {LoginId}", id);
                return await _context.Logins
                    .AsNoTracking()
                    .AnyAsync(l => l.LoginID == id && l.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking login record existence with ID: {LoginId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Login>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetAllAsync(cancellationToken);
                }

                _logger.LogInformation("Searching login records with term: {SearchTerm}", searchTerm);

                var normalizedSearchTerm = searchTerm.ToLower().Trim();

                return await _context.Logins
                    .AsNoTracking()
                    .Where(l => l.IsActive && l.Email.ToLower().Contains(normalizedSearchTerm))
                    .OrderBy(l => l.Email)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching login records with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<Login> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email cannot be null or empty", nameof(email));
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Password cannot be null or empty", nameof(password));
                }

                _logger.LogInformation("Validating login credentials for email: {Email}", email);

                var login = await _context.Logins
                    .FirstOrDefaultAsync(l => l.IsActive && l.Email.ToLower() == email.ToLower(), cancellationToken);

                if (login == null)
                {
                    _logger.LogWarning("Login attempt failed - email not found: {Email}", email);
                    return null;
                }

                // Check if account is locked out
                if (login.IsLockedOut)
                {
                    _logger.LogWarning("Login attempt failed - account locked out: {Email}", email);
                    return null;
                }

                // Validate password (in production, use proper password hashing comparison)
                if (login.Password != password)
                {
                    _logger.LogWarning("Login attempt failed - invalid password for email: {Email}", email);

                    // Increment failed login attempts
                    login.FailedLoginAttempts++;

                    // Lock account after 5 failed attempts
                    if (login.FailedLoginAttempts >= 5)
                    {
                        login.LockoutEnd = DateTime.UtcNow.AddMinutes(30);
                        _logger.LogWarning("Account locked out due to multiple failed attempts: {Email}", email);
                    }

                    _context.Logins.Update(login);
                    await _context.SaveChangesAsync(cancellationToken);

                    return null;
                }

                // Successful login - reset failed attempts and update last login
                login.FailedLoginAttempts = 0;
                login.LockoutEnd = null;
                login.LastLoginDate = DateTime.UtcNow;

                _context.Logins.Update(login);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Login successful for email: {Email}", email);
                return login;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating login credentials for email: {Email}", email);
                throw;
            }
        }

        public async Task<Login> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email cannot be null or empty", nameof(email));
                }

                _logger.LogInformation("Retrieving login record by email: {Email}", email);

                var login = await _context.Logins
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l => l.IsActive && l.Email.ToLower() == email.ToLower(), cancellationToken);

                if (login == null)
                {
                    _logger.LogWarning("Login record with email {Email} not found", email);
                }

                return login;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving login record by email: {Email}", email);
                throw;
            }
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email cannot be null or empty", nameof(email));
                }

                _logger.LogDebug("Checking if email exists: {Email}", email);
                return await _context.Logins
                    .AsNoTracking()
                    .AnyAsync(l => l.Email.ToLower() == email.ToLower(), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email existence: {Email}", email);
                throw;
            }
        }

        public async Task<bool> UpdatePasswordAsync(int id, string newPassword, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    throw new ArgumentException("Password cannot be null or empty", nameof(newPassword));
                }

                _logger.LogInformation("Updating password for login ID: {LoginId}", id);

                var login = await _context.Logins
                    .FirstOrDefaultAsync(l => l.LoginID == id, cancellationToken);

                if (login == null)
                {
                    _logger.LogWarning("Login record with ID {LoginId} not found for password update", id);
                    return false;
                }

                // In production, hash the password before storing
                login.Password = newPassword;
                login.ModifiedDate = DateTime.UtcNow;

                _context.Logins.Update(login);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Password updated successfully for login ID: {LoginId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating password for login ID: {LoginId}", id);
                throw;
            }
        }

        public async Task<bool> UpdatePasswordByEmailAsync(string email, string newPassword, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email cannot be null or empty", nameof(email));
                }

                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    throw new ArgumentException("Password cannot be null or empty", nameof(newPassword));
                }

                _logger.LogInformation("Updating password for email: {Email}", email);

                var login = await _context.Logins
                    .FirstOrDefaultAsync(l => l.Email.ToLower() == email.ToLower(), cancellationToken);

                if (login == null)
                {
                    _logger.LogWarning("Login record with email {Email} not found for password update", email);
                    return false;
                }

                // In production, hash the password before storing
                login.Password = newPassword;
                login.ModifiedDate = DateTime.UtcNow;

                _context.Logins.Update(login);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Password updated successfully for email: {Email}", email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating password for email: {Email}", email);
                throw;
            }
        }

        public async Task<IEnumerable<Login>> GetByTypeAsync(int type, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving login records by type: {Type}", type);
                return await _context.Logins
                    .AsNoTracking()
                    .Where(l => l.IsActive && l.Type == type)
                    .OrderBy(l => l.Email)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving login records by type: {Type}", type);
                throw;
            }
        }

        public async Task<int> GetCountByTypeAsync(int type, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving count of login records by type: {Type}", type);
                return await _context.Logins
                    .AsNoTracking()
                    .CountAsync(l => l.IsActive && l.Type == type, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving count of login records by type: {Type}", type);
                throw;
            }
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving total login record count");
                return await _context.Logins
                    .AsNoTracking()
                    .CountAsync(l => l.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total login record count");
                throw;
            }
        }

        public async Task<bool> UpdateEmailAsync(int id, string newEmail, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newEmail))
                {
                    throw new ArgumentException("Email cannot be null or empty", nameof(newEmail));
                }

                _logger.LogInformation("Updating email for login ID: {LoginId} to {NewEmail}", id, newEmail);

                // Check if new email already exists
                var emailExists = await _context.Logins
                    .AsNoTracking()
                    .AnyAsync(l => l.Email.ToLower() == newEmail.ToLower() && l.LoginID != id, cancellationToken);

                if (emailExists)
                {
                    _logger.LogWarning("Email {NewEmail} already exists", newEmail);
                    throw new InvalidOperationException($"Email {newEmail} already exists");
                }

                var login = await _context.Logins
                    .FirstOrDefaultAsync(l => l.LoginID == id, cancellationToken);

                if (login == null)
                {
                    _logger.LogWarning("Login record with ID {LoginId} not found for email update", id);
                    return false;
                }

                login.Email = newEmail;
                login.ModifiedDate = DateTime.UtcNow;

                _context.Logins.Update(login);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Email updated successfully for login ID: {LoginId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating email for login ID: {LoginId}", id);
                throw;
            }
        }

        public async Task<bool> VerifyPasswordAsync(int id, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Password cannot be null or empty", nameof(password));
                }

                _logger.LogDebug("Verifying password for login ID: {LoginId}", id);

                var login = await _context.Logins
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l => l.LoginID == id, cancellationToken);

                if (login == null)
                {
                    _logger.LogWarning("Login record with ID {LoginId} not found for password verification", id);
                    return false;
                }

                // In production, use proper password hashing comparison
                var isValid = login.Password == password;

                _logger.LogDebug("Password verification result for login ID {LoginId}: {IsValid}", id, isValid);
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying password for login ID: {LoginId}", id);
                throw;
            }
        }
    }
}
