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
    /// Repository implementation for OtherStaff entity operations
    /// </summary>
    public class OtherStaffRepository : IOtherStaffRepository
    {
        private readonly HospitalDbContext _context;
        private readonly ILogger<OtherStaffRepository> _logger;

        public OtherStaffRepository(HospitalDbContext context, ILogger<OtherStaffRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<OtherStaff>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving all staff members");
                return await _context.OtherStaff
                    .AsNoTracking()
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all staff members");
                throw;
            }
        }

        public async Task<OtherStaff> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving staff member with ID: {StaffId}", id);
                var staff = await _context.OtherStaff
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.StaffID == id && s.IsActive, cancellationToken);

                if (staff == null)
                {
                    _logger.LogWarning("Staff member with ID {StaffId} not found", id);
                }

                return staff;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving staff member with ID: {StaffId}", id);
                throw;
            }
        }

        public async Task<OtherStaff> AddAsync(OtherStaff staff, CancellationToken cancellationToken = default)
        {
            try
            {
                if (staff == null)
                {
                    throw new ArgumentNullException(nameof(staff));
                }

                _logger.LogInformation("Adding new staff member: {StaffName}", staff.Name);

                staff.CreatedDate = DateTime.UtcNow;
                staff.IsActive = true;
                staff.Status = "Active";

                await _context.OtherStaff.AddAsync(staff, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Staff member added successfully with ID: {StaffId}", staff.StaffID);
                return staff;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding staff member: {StaffName}", staff?.Name);
                throw;
            }
        }

        public async Task<OtherStaff> UpdateAsync(OtherStaff staff, CancellationToken cancellationToken = default)
        {
            try
            {
                if (staff == null)
                {
                    throw new ArgumentNullException(nameof(staff));
                }

                _logger.LogInformation("Updating staff member with ID: {StaffId}", staff.StaffID);

                var existingStaff = await _context.OtherStaff
                    .FirstOrDefaultAsync(s => s.StaffID == staff.StaffID, cancellationToken);

                if (existingStaff == null)
                {
                    _logger.LogWarning("Staff member with ID {StaffId} not found for update", staff.StaffID);
                    throw new InvalidOperationException($"Staff member with ID {staff.StaffID} not found");
                }

                existingStaff.Name = staff.Name;
                existingStaff.Phone = staff.Phone;
                existingStaff.Address = staff.Address;
                existingStaff.Designation = staff.Designation;
                existingStaff.Gender = staff.Gender;
                existingStaff.BirthDate = staff.BirthDate;
                existingStaff.HighestQualification = staff.HighestQualification;
                existingStaff.Salary = staff.Salary;
                existingStaff.JoiningDate = staff.JoiningDate;
                existingStaff.Email = staff.Email;
                existingStaff.EmergencyContact = staff.EmergencyContact;
                existingStaff.EmergencyContactName = staff.EmergencyContactName;
                existingStaff.Department = staff.Department;
                existingStaff.Shift = staff.Shift;
                existingStaff.EmploymentType = staff.EmploymentType;
                existingStaff.EmployeeID = staff.EmployeeID;
                existingStaff.NationalID = staff.NationalID;
                existingStaff.BloodGroup = staff.BloodGroup;
                existingStaff.Status = staff.Status;
                existingStaff.TerminationDate = staff.TerminationDate;
                existingStaff.TerminationReason = staff.TerminationReason;
                existingStaff.Notes = staff.Notes;
                existingStaff.ModifiedDate = DateTime.UtcNow;
                existingStaff.ModifiedBy = staff.ModifiedBy;

                _context.OtherStaff.Update(existingStaff);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Staff member updated successfully with ID: {StaffId}", staff.StaffID);
                return existingStaff;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating staff member with ID: {StaffId}", staff?.StaffID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting staff member with ID: {StaffId}", id);

                var staff = await _context.OtherStaff
                    .FirstOrDefaultAsync(s => s.StaffID == id, cancellationToken);

                if (staff == null)
                {
                    _logger.LogWarning("Staff member with ID {StaffId} not found for deletion", id);
                    return false;
                }

                // Soft delete
                staff.IsActive = false;
                staff.Status = "Terminated";
                staff.TerminationDate = DateTime.UtcNow;
                staff.ModifiedDate = DateTime.UtcNow;

                _context.OtherStaff.Update(staff);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Staff member deleted successfully with ID: {StaffId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting staff member with ID: {StaffId}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Checking if staff member exists with ID: {StaffId}", id);
                return await _context.OtherStaff
                    .AsNoTracking()
                    .AnyAsync(s => s.StaffID == id && s.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking staff member existence with ID: {StaffId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<OtherStaff>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetAllAsync(cancellationToken);
                }

                _logger.LogInformation("Searching staff members with term: {SearchTerm}", searchTerm);

                var normalizedSearchTerm = searchTerm.ToLower().Trim();

                return await _context.OtherStaff
                    .AsNoTracking()
                    .Where(s => s.IsActive &&
                        (s.Name.ToLower().Contains(normalizedSearchTerm) ||
                         s.Designation.ToLower().Contains(normalizedSearchTerm)))
                    .OrderBy(s => s.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching staff members with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<IEnumerable<OtherStaff>> GetByDesignationAsync(string designation, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(designation))
                {
                    throw new ArgumentException("Designation cannot be null or empty", nameof(designation));
                }

                _logger.LogInformation("Retrieving staff members by designation: {Designation}", designation);

                return await _context.OtherStaff
                    .AsNoTracking()
                    .Where(s => s.IsActive && s.Designation.ToLower() == designation.ToLower())
                    .OrderBy(s => s.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving staff members by designation: {Designation}", designation);
                throw;
            }
        }

        public async Task<IEnumerable<OtherStaff>> GetByGenderAsync(char gender, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving staff members by gender: {Gender}", gender);

                var genderStr = gender.ToString().ToUpper();

                return await _context.OtherStaff
                    .AsNoTracking()
                    .Where(s => s.IsActive && s.Gender.ToUpper() == genderStr)
                    .OrderBy(s => s.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving staff members by gender: {Gender}", gender);
                throw;
            }
        }

        public async Task<OtherStaff> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    throw new ArgumentException("Phone cannot be null or empty", nameof(phone));
                }

                _logger.LogInformation("Retrieving staff member by phone: {Phone}", phone);

                var staff = await _context.OtherStaff
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.IsActive && s.Phone == phone, cancellationToken);

                if (staff == null)
                {
                    _logger.LogWarning("Staff member with phone {Phone} not found", phone);
                }

                return staff;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving staff member by phone: {Phone}", phone);
                throw;
            }
        }

        public async Task<IEnumerable<OtherStaff>> GetBySalaryRangeAsync(float minSalary, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving staff members with salary above: {MinSalary}", minSalary);

                return await _context.OtherStaff
                    .AsNoTracking()
                    .Where(s => s.IsActive && s.Salary.HasValue && s.Salary.Value >= (decimal)minSalary)
                    .OrderByDescending(s => s.Salary)
                    .ThenBy(s => s.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving staff members with salary above: {MinSalary}", minSalary);
                throw;
            }
        }

        public async Task<IEnumerable<OtherStaff>> GetBySalaryRangeAsync(float minSalary, float maxSalary, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving staff members with salary between {MinSalary} and {MaxSalary}",
                    minSalary, maxSalary);

                return await _context.OtherStaff
                    .AsNoTracking()
                    .Where(s => s.IsActive &&
                        s.Salary.HasValue &&
                        s.Salary.Value >= (decimal)minSalary &&
                        s.Salary.Value <= (decimal)maxSalary)
                    .OrderByDescending(s => s.Salary)
                    .ThenBy(s => s.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving staff members with salary between {MinSalary} and {MaxSalary}",
                    minSalary, maxSalary);
                throw;
            }
        }

        public async Task<bool> UpdateSalaryAsync(int id, float newSalary, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating salary for staff member ID: {StaffId} to {NewSalary}", id, newSalary);

                var staff = await _context.OtherStaff
                    .FirstOrDefaultAsync(s => s.StaffID == id, cancellationToken);

                if (staff == null)
                {
                    _logger.LogWarning("Staff member with ID {StaffId} not found for salary update", id);
                    return false;
                }

                staff.Salary = (decimal)newSalary;
                staff.ModifiedDate = DateTime.UtcNow;

                _context.OtherStaff.Update(staff);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Salary updated successfully for staff member ID: {StaffId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating salary for staff member ID: {StaffId}", id);
                throw;
            }
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving total staff member count");
                return await _context.OtherStaff
                    .AsNoTracking()
                    .CountAsync(s => s.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total staff member count");
                throw;
            }
        }

        public async Task<int> GetCountByDesignationAsync(string designation, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(designation))
                {
                    throw new ArgumentException("Designation cannot be null or empty", nameof(designation));
                }

                _logger.LogInformation("Retrieving count of staff members by designation: {Designation}", designation);

                return await _context.OtherStaff
                    .AsNoTracking()
                    .CountAsync(s => s.IsActive && s.Designation.ToLower() == designation.ToLower(), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving count of staff members by designation: {Designation}", designation);
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetAllDesignationsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving all unique designations");

                return await _context.OtherStaff
                    .AsNoTracking()
                    .Where(s => s.IsActive)
                    .Select(s => s.Designation)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all unique designations");
                throw;
            }
        }

        public async Task<OtherStaff> GetStaffProfileAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving complete staff profile for ID: {StaffId}", id);

                return await _context.OtherStaff
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.StaffID == id && s.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving staff profile for ID: {StaffId}", id);
                throw;
            }
        }
    }
}
