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
    /// Repository implementation for Doctor entity operations
    /// </summary>
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalDbContext _context;
        private readonly ILogger<DoctorRepository> _logger;

        public DoctorRepository(HospitalDbContext context, ILogger<DoctorRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving all doctors");
                return await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .Where(d => d.IsActive)
                    .OrderBy(d => d.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all doctors");
                throw;
            }
        }

        public async Task<Doctor> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving doctor with ID: {DoctorId}", id);
                var doctor = await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .FirstOrDefaultAsync(d => d.DoctorID == id && d.IsActive, cancellationToken);

                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found", id);
                }

                return doctor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctor with ID: {DoctorId}", id);
                throw;
            }
        }

        public async Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
        {
            try
            {
                if (doctor == null)
                {
                    throw new ArgumentNullException(nameof(doctor));
                }

                _logger.LogInformation("Adding new doctor: {DoctorName}", doctor.Name);

                doctor.CreatedDate = DateTime.UtcNow;
                doctor.IsActive = true;
                doctor.Status = 1;

                await _context.Doctors.AddAsync(doctor, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Doctor added successfully with ID: {DoctorId}", doctor.DoctorID);
                return doctor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding doctor: {DoctorName}", doctor?.Name);
                throw;
            }
        }

        public async Task<Doctor> UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default)
        {
            try
            {
                if (doctor == null)
                {
                    throw new ArgumentNullException(nameof(doctor));
                }

                _logger.LogInformation("Updating doctor with ID: {DoctorId}", doctor.DoctorID);

                var existingDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorID == doctor.DoctorID, cancellationToken);

                if (existingDoctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found for update", doctor.DoctorID);
                    throw new InvalidOperationException($"Doctor with ID {doctor.DoctorID} not found");
                }

                existingDoctor.Name = doctor.Name;
                existingDoctor.Phone = doctor.Phone;
                existingDoctor.Address = doctor.Address;
                existingDoctor.BirthDate = doctor.BirthDate;
                existingDoctor.Gender = doctor.Gender;
                existingDoctor.DeptNo = doctor.DeptNo;
                existingDoctor.ChargesPerVisit = doctor.ChargesPerVisit;
                existingDoctor.MonthlySalary = doctor.MonthlySalary;
                existingDoctor.Qualification = doctor.Qualification;
                existingDoctor.Specialization = doctor.Specialization;
                existingDoctor.WorkExperience = doctor.WorkExperience;
                existingDoctor.Status = doctor.Status;
                existingDoctor.Email = doctor.Email;
                existingDoctor.ModifiedDate = DateTime.UtcNow;
                existingDoctor.ModifiedBy = doctor.ModifiedBy;

                _context.Doctors.Update(existingDoctor);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Doctor updated successfully with ID: {DoctorId}", doctor.DoctorID);
                return existingDoctor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating doctor with ID: {DoctorId}", doctor?.DoctorID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting doctor with ID: {DoctorId}", id);

                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorID == id, cancellationToken);

                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found for deletion", id);
                    return false;
                }

                // Soft delete
                doctor.IsActive = false;
                doctor.Status = 0;
                doctor.ModifiedDate = DateTime.UtcNow;

                _context.Doctors.Update(doctor);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Doctor deleted successfully with ID: {DoctorId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting doctor with ID: {DoctorId}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Checking if doctor exists with ID: {DoctorId}", id);
                return await _context.Doctors
                    .AsNoTracking()
                    .AnyAsync(d => d.DoctorID == id && d.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking doctor existence with ID: {DoctorId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetAllAsync(cancellationToken);
                }

                _logger.LogInformation("Searching doctors with term: {SearchTerm}", searchTerm);

                var normalizedSearchTerm = searchTerm.ToLower().Trim();

                return await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .Where(d => d.IsActive &&
                        (d.Name.ToLower().Contains(normalizedSearchTerm) ||
                         (d.Specialization != null && d.Specialization.ToLower().Contains(normalizedSearchTerm))))
                    .OrderBy(d => d.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching doctors with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving active doctors");
                return await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .Where(d => d.IsActive && d.Status == 1)
                    .OrderBy(d => d.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active doctors");
                throw;
            }
        }

        public async Task<IEnumerable<Doctor>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving doctors by department ID: {DepartmentId}", departmentId);
                return await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .Where(d => d.IsActive && d.DeptNo == departmentId)
                    .OrderBy(d => d.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctors by department ID: {DepartmentId}", departmentId);
                throw;
            }
        }

        public async Task<IEnumerable<Doctor>> GetByDepartmentNameAsync(string departmentName, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departmentName))
                {
                    throw new ArgumentException("Department name cannot be null or empty", nameof(departmentName));
                }

                _logger.LogInformation("Retrieving doctors by department name: {DepartmentName}", departmentName);

                return await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .Where(d => d.IsActive &&
                        d.Department != null &&
                        d.Department.DeptName.ToLower() == departmentName.ToLower())
                    .OrderBy(d => d.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctors by department name: {DepartmentName}", departmentName);
                throw;
            }
        }

        public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(specialization))
                {
                    throw new ArgumentException("Specialization cannot be null or empty", nameof(specialization));
                }

                _logger.LogInformation("Retrieving doctors by specialization: {Specialization}", specialization);

                return await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .Where(d => d.IsActive &&
                        d.Specialization != null &&
                        d.Specialization.ToLower().Contains(specialization.ToLower()))
                    .OrderBy(d => d.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctors by specialization: {Specialization}", specialization);
                throw;
            }
        }

        public async Task<Doctor> GetDoctorProfileAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving doctor profile for ID: {DoctorId}", id);
                return await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .Include(d => d.Appointments)
                    .Include(d => d.Feedbacks)
                    .FirstOrDefaultAsync(d => d.DoctorID == id && d.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctor profile for ID: {DoctorId}", id);
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
                return await _context.Doctors
                    .AsNoTracking()
                    .AnyAsync(d => d.Email.ToLower() == email.ToLower(), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email existence: {Email}", email);
                throw;
            }
        }

        public async Task<Doctor> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email cannot be null or empty", nameof(email));
                }

                _logger.LogInformation("Retrieving doctor by email: {Email}", email);

                var doctor = await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .FirstOrDefaultAsync(d => d.IsActive && d.Email.ToLower() == email.ToLower(), cancellationToken);

                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with email {Email} not found", email);
                }

                return doctor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctor by email: {Email}", email);
                throw;
            }
        }

        public async Task<IEnumerable<Doctor>> GetByMinimumExperienceAsync(int years, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving doctors with minimum experience: {Years} years", years);
                return await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .Where(d => d.IsActive && d.WorkExperience >= years)
                    .OrderByDescending(d => d.WorkExperience)
                    .ThenBy(d => d.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctors with minimum experience: {Years} years", years);
                throw;
            }
        }

        public async Task<IEnumerable<Doctor>> GetTopRatedDoctorsAsync(int limit, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving top {Limit} rated doctors", limit);
                return await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .Where(d => d.IsActive && d.Status == 1)
                    .OrderByDescending(d => d.ReputeIndex)
                    .ThenBy(d => d.Name)
                    .Take(limit)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top {Limit} rated doctors", limit);
                throw;
            }
        }

        public async Task<IEnumerable<Doctor>> GetByGenderAsync(char gender, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving doctors by gender: {Gender}", gender);

                var genderStr = gender.ToString().ToUpper();

                return await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .Where(d => d.IsActive && d.Gender.ToUpper() == genderStr)
                    .OrderBy(d => d.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctors by gender: {Gender}", gender);
                throw;
            }
        }

        public async Task<bool> UpdateReputeIndexAsync(int id, float reputeIndex, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating repute index for doctor ID: {DoctorId} to {ReputeIndex}", id, reputeIndex);

                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorID == id, cancellationToken);

                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found for repute index update", id);
                    return false;
                }

                doctor.ReputeIndex = (decimal)reputeIndex;
                doctor.ModifiedDate = DateTime.UtcNow;

                _context.Doctors.Update(doctor);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Repute index updated successfully for doctor ID: {DoctorId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating repute index for doctor ID: {DoctorId}", id);
                throw;
            }
        }

        public async Task<bool> IncrementPatientsTreatedAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Incrementing patients treated for doctor ID: {DoctorId}", id);

                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorID == id, cancellationToken);

                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found for patients treated increment", id);
                    return false;
                }

                doctor.PatientsTreated++;
                doctor.ModifiedDate = DateTime.UtcNow;

                _context.Doctors.Update(doctor);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Patients treated incremented successfully for doctor ID: {DoctorId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing patients treated for doctor ID: {DoctorId}", id);
                throw;
            }
        }

        public async Task<int> GetTotalActiveDoctorsCountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving total active doctors count");
                return await _context.Doctors
                    .AsNoTracking()
                    .CountAsync(d => d.IsActive && d.Status == 1, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total active doctors count");
                throw;
            }
        }
    }
}
