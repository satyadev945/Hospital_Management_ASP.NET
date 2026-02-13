using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Department entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class DepartmentRepository : IDepartmentRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(ClinicDbContext context, ILogger<DepartmentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving department with ID: {DepartmentId}", id);

            var department = await _context.Departments
                .Include(d => d.Doctors)
                .Include(d => d.Staff)
                .FirstOrDefaultAsync(d => d.DepartmentId == id, cancellationToken);

            if (department == null)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found", id);
            }

            return department;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department with ID: {DepartmentId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all departments");

            var departments = await _context.Departments
                .Include(d => d.Doctors)
                .Include(d => d.Staff)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Retrieved {Count} departments", departments.Count);
            return departments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all departments");
            throw;
        }
    }

    public async Task<Department> AddAsync(Department entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding new department: {DepartmentName}", entity.DepartmentName);

            await _context.Departments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added department with ID: {DepartmentId}", entity.DepartmentId);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding department: {DepartmentName}", 
                entity.DepartmentName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding department: {DepartmentName}", entity.DepartmentName);
            throw;
        }
    }

    public async Task UpdateAsync(Department entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating department with ID: {DepartmentId}", entity.DepartmentId);

            _context.Departments.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated department with ID: {DepartmentId}", 
                entity.DepartmentId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating department with ID: {DepartmentId}", 
                entity.DepartmentId);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating department with ID: {DepartmentId}", 
                entity.DepartmentId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department with ID: {DepartmentId}", entity.DepartmentId);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting department with ID: {DepartmentId}", id);

            var department = await _context.Departments.FindAsync(new object[] { id }, cancellationToken);
            if (department == null)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found for deletion", id);
                throw new KeyNotFoundException($"Department with ID {id} not found");
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted department with ID: {DepartmentId}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting department with ID: {DepartmentId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department with ID: {DepartmentId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Department>> GetActiveDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving active departments");

            var departments = await _context.Departments
                .Where(d => d.IsActive)
                .Include(d => d.Doctors)
                .Include(d => d.Staff)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} active departments", departments.Count);
            return departments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active departments");
            throw;
        }
    }

    public async Task<Department?> GetByNameAsync(string departmentName, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(departmentName))
            {
                _logger.LogWarning("Empty department name provided for search");
                return null;
            }

            _logger.LogDebug("Retrieving department by name: {DepartmentName}", departmentName);

            var department = await _context.Departments
                .Include(d => d.Doctors)
                .Include(d => d.Staff)
                .FirstOrDefaultAsync(d => d.DepartmentName == departmentName, cancellationToken);

            if (department == null)
            {
                _logger.LogWarning("Department with name {DepartmentName} not found", departmentName);
            }

            return department;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department by name: {DepartmentName}", departmentName);
            throw;
        }
    }

    public async Task<IEnumerable<Department>> GetDepartmentsWithDoctorsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving departments with doctors");

            var departments = await _context.Departments
                .Include(d => d.Doctors.Where(doc => doc.IsActive))
                .Include(d => d.Staff)
                .Where(d => d.Doctors.Any(doc => doc.IsActive))
                .OrderBy(d => d.DepartmentName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} departments with doctors", departments.Count);
            return departments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving departments with doctors");
            throw;
        }
    }
}
