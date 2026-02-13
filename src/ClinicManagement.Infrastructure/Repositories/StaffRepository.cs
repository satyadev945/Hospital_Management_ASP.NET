using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Staff entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class StaffRepository : IStaffRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<StaffRepository> _logger;

    public StaffRepository(ClinicDbContext context, ILogger<StaffRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Staff?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving staff with ID: {StaffId}", id);

            var staff = await _context.Staff
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.StaffId == id, cancellationToken);

            if (staff == null)
            {
                _logger.LogWarning("Staff with ID {StaffId} not found", id);
            }

            return staff;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff with ID: {StaffId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Staff>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all staff members");

            var staffMembers = await _context.Staff
                .Include(s => s.Department)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Retrieved {Count} staff members", staffMembers.Count);
            return staffMembers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all staff members");
            throw;
        }
    }

    public async Task<Staff> AddAsync(Staff entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding new staff member: {FirstName} {LastName}", 
                entity.FirstName, entity.LastName);

            await _context.Staff.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added staff member with ID: {StaffId}", entity.StaffId);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding staff member: {FirstName} {LastName}", 
                entity.FirstName, entity.LastName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding staff member: {FirstName} {LastName}", 
                entity.FirstName, entity.LastName);
            throw;
        }
    }

    public async Task UpdateAsync(Staff entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating staff member with ID: {StaffId}", entity.StaffId);

            _context.Staff.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated staff member with ID: {StaffId}", entity.StaffId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating staff member with ID: {StaffId}", 
                entity.StaffId);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating staff member with ID: {StaffId}", 
                entity.StaffId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating staff member with ID: {StaffId}", entity.StaffId);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting staff member with ID: {StaffId}", id);

            var staff = await _context.Staff.FindAsync(new object[] { id }, cancellationToken);
            if (staff == null)
            {
                _logger.LogWarning("Staff member with ID {StaffId} not found for deletion", id);
                throw new KeyNotFoundException($"Staff member with ID {id} not found");
            }

            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted staff member with ID: {StaffId}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting staff member with ID: {StaffId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting staff member with ID: {StaffId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Staff>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving staff for department ID: {DepartmentId}", departmentId);

            var staffMembers = await _context.Staff
                .Where(s => s.DepartmentId == departmentId)
                .Include(s => s.Department)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} staff members in department ID: {DepartmentId}", 
                staffMembers.Count, departmentId);
            return staffMembers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff for department ID: {DepartmentId}", departmentId);
            throw;
        }
    }

    public async Task<IEnumerable<Staff>> GetByRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                _logger.LogWarning("Empty role provided for staff search");
                return Enumerable.Empty<Staff>();
            }

            _logger.LogDebug("Retrieving staff by role: {Role}", role);

            var staffMembers = await _context.Staff
                .Where(s => s.Role == role)
                .Include(s => s.Department)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} staff members with role: {Role}", staffMembers.Count, role);
            return staffMembers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff by role: {Role}", role);
            throw;
        }
    }

    public async Task<IEnumerable<Staff>> GetActiveStaffAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving active staff members");

            var staffMembers = await _context.Staff
                .Where(s => s.IsActive)
                .Include(s => s.Department)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} active staff members", staffMembers.Count);
            return staffMembers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active staff members");
            throw;
        }
    }

    public async Task<IEnumerable<Staff>> GetByShiftAsync(string shift, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(shift))
            {
                _logger.LogWarning("Empty shift provided for staff search");
                return Enumerable.Empty<Staff>();
            }

            _logger.LogDebug("Retrieving staff by shift: {Shift}", shift);

            var staffMembers = await _context.Staff
                .Where(s => s.Shift == shift && s.IsActive)
                .Include(s => s.Department)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} staff members with shift: {Shift}", staffMembers.Count, shift);
            return staffMembers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff by shift: {Shift}", shift);
            throw;
        }
    }

    public async Task<Staff?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Empty email provided for staff search");
                return null;
            }

            _logger.LogDebug("Retrieving staff by email: {Email}", email);

            var staff = await _context.Staff
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Email == email, cancellationToken);

            if (staff == null)
            {
                _logger.LogWarning("Staff with email {Email} not found", email);
            }

            return staff;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff by email: {Email}", email);
            throw;
        }
    }

    public async Task<IEnumerable<Staff>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Empty search term provided for staff search");
                return Enumerable.Empty<Staff>();
            }

            _logger.LogDebug("Searching staff by name with term: {SearchTerm}", searchTerm);

            var staffMembers = await _context.Staff
                .Where(s => s.FirstName.Contains(searchTerm) || s.LastName.Contains(searchTerm))
                .Include(s => s.Department)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} staff members matching search term: {SearchTerm}", 
                staffMembers.Count, searchTerm);
            return staffMembers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching staff by name with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
