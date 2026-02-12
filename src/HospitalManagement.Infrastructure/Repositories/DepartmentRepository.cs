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
    /// Repository implementation for Department entity operations
    /// </summary>
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HospitalDbContext _context;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(HospitalDbContext context, ILogger<DepartmentRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving all departments");
                return await _context.Departments
                    .AsNoTracking()
                    .Where(d => d.IsActive)
                    .OrderBy(d => d.DeptName)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all departments");
                throw;
            }
        }

        public async Task<Department> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving department with ID: {DepartmentId}", id);
                var department = await _context.Departments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.DeptNo == id && d.IsActive, cancellationToken);

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

        public async Task<Department> AddAsync(Department department, CancellationToken cancellationToken = default)
        {
            try
            {
                if (department == null)
                {
                    throw new ArgumentNullException(nameof(department));
                }

                _logger.LogInformation("Adding new department: {DepartmentName}", department.DeptName);

                department.CreatedDate = DateTime.UtcNow;
                department.IsActive = true;

                await _context.Departments.AddAsync(department, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Department added successfully with ID: {DepartmentId}", department.DeptNo);
                return department;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding department: {DepartmentName}", department?.DeptName);
                throw;
            }
        }

        public async Task<Department> UpdateAsync(Department department, CancellationToken cancellationToken = default)
        {
            try
            {
                if (department == null)
                {
                    throw new ArgumentNullException(nameof(department));
                }

                _logger.LogInformation("Updating department with ID: {DepartmentId}", department.DeptNo);

                var existingDepartment = await _context.Departments
                    .FirstOrDefaultAsync(d => d.DeptNo == department.DeptNo, cancellationToken);

                if (existingDepartment == null)
                {
                    _logger.LogWarning("Department with ID {DepartmentId} not found for update", department.DeptNo);
                    throw new InvalidOperationException($"Department with ID {department.DeptNo} not found");
                }

                existingDepartment.DeptName = department.DeptName;
                existingDepartment.Description = department.Description;
                existingDepartment.ModifiedDate = DateTime.UtcNow;
                existingDepartment.ModifiedBy = department.ModifiedBy;

                _context.Departments.Update(existingDepartment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Department updated successfully with ID: {DepartmentId}", department.DeptNo);
                return existingDepartment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department with ID: {DepartmentId}", department?.DeptNo);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting department with ID: {DepartmentId}", id);

                var department = await _context.Departments
                    .FirstOrDefaultAsync(d => d.DeptNo == id, cancellationToken);

                if (department == null)
                {
                    _logger.LogWarning("Department with ID {DepartmentId} not found for deletion", id);
                    return false;
                }

                // Soft delete
                department.IsActive = false;
                department.ModifiedDate = DateTime.UtcNow;

                _context.Departments.Update(department);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Department deleted successfully with ID: {DepartmentId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department with ID: {DepartmentId}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Checking if department exists with ID: {DepartmentId}", id);
                return await _context.Departments
                    .AsNoTracking()
                    .AnyAsync(d => d.DeptNo == id && d.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking department existence with ID: {DepartmentId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Department>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetAllAsync(cancellationToken);
                }

                _logger.LogInformation("Searching departments with term: {SearchTerm}", searchTerm);

                var normalizedSearchTerm = searchTerm.ToLower().Trim();

                return await _context.Departments
                    .AsNoTracking()
                    .Where(d => d.IsActive &&
                        (d.DeptName.ToLower().Contains(normalizedSearchTerm) ||
                         (d.Description != null && d.Description.ToLower().Contains(normalizedSearchTerm))))
                    .OrderBy(d => d.DeptName)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching departments with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<Department> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Department name cannot be null or empty", nameof(name));
                }

                _logger.LogInformation("Retrieving department by name: {DepartmentName}", name);

                var department = await _context.Departments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.IsActive && d.DeptName.ToLower() == name.ToLower(), cancellationToken);

                if (department == null)
                {
                    _logger.LogWarning("Department with name {DepartmentName} not found", name);
                }

                return department;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department by name: {DepartmentName}", name);
                throw;
            }
        }

        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Department name cannot be null or empty", nameof(name));
                }

                _logger.LogDebug("Checking if department name exists: {DepartmentName}", name);
                return await _context.Departments
                    .AsNoTracking()
                    .AnyAsync(d => d.DeptName.ToLower() == name.ToLower(), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking department name existence: {DepartmentName}", name);
                throw;
            }
        }

        public async Task<Department> GetWithDoctorCountAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving department with doctor count for ID: {DepartmentId}", id);
                return await _context.Departments
                    .AsNoTracking()
                    .Include(d => d.Doctors.Where(doc => doc.IsActive))
                    .FirstOrDefaultAsync(d => d.DeptNo == id && d.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department with doctor count for ID: {DepartmentId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Department>> GetAllWithDoctorCountsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving all departments with doctor counts");
                return await _context.Departments
                    .AsNoTracking()
                    .Include(d => d.Doctors.Where(doc => doc.IsActive))
                    .Where(d => d.IsActive)
                    .OrderBy(d => d.DeptName)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all departments with doctor counts");
                throw;
            }
        }

        public async Task<IEnumerable<Department>> GetDepartmentsWithActiveDoctorsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving departments with active doctors");
                return await _context.Departments
                    .AsNoTracking()
                    .Include(d => d.Doctors)
                    .Where(d => d.IsActive &&
                        d.Doctors.Any(doc => doc.IsActive && doc.Status == 1))
                    .OrderBy(d => d.DeptName)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving departments with active doctors");
                throw;
            }
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving total department count");
                return await _context.Departments
                    .AsNoTracking()
                    .CountAsync(d => d.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total department count");
                throw;
            }
        }

        public async Task<IEnumerable<Department>> GetDepartmentInfoAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving department information for appointment selection");
                return await _context.Departments
                    .AsNoTracking()
                    .Where(d => d.IsActive)
                    .Select(d => new Department
                    {
                        DeptNo = d.DeptNo,
                        DeptName = d.DeptName,
                        Description = d.Description
                    })
                    .OrderBy(d => d.DeptName)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department information");
                throw;
            }
        }
    }
}
