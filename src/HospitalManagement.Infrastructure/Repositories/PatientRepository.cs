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
    /// Repository implementation for Patient entity operations
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly HospitalDbContext _context;
        private readonly ILogger<PatientRepository> _logger;

        public PatientRepository(HospitalDbContext context, ILogger<PatientRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving all patients");
                return await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patients");
                throw;
            }
        }

        public async Task<Patient> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving patient with ID: {PatientId}", id);
                var patient = await _context.Patients
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PatientID == id && p.IsActive, cancellationToken);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found", id);
                }

                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with ID: {PatientId}", id);
                throw;
            }
        }

        public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            try
            {
                if (patient == null)
                {
                    throw new ArgumentNullException(nameof(patient));
                }

                _logger.LogInformation("Adding new patient: {PatientName}", patient.Name);

                patient.CreatedDate = DateTime.UtcNow;
                patient.IsActive = true;

                await _context.Patients.AddAsync(patient, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Patient added successfully with ID: {PatientId}", patient.PatientID);
                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding patient: {PatientName}", patient?.Name);
                throw;
            }
        }

        public async Task<Patient> UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            try
            {
                if (patient == null)
                {
                    throw new ArgumentNullException(nameof(patient));
                }

                _logger.LogInformation("Updating patient with ID: {PatientId}", patient.PatientID);

                var existingPatient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientID == patient.PatientID, cancellationToken);

                if (existingPatient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found for update", patient.PatientID);
                    throw new InvalidOperationException($"Patient with ID {patient.PatientID} not found");
                }

                existingPatient.Name = patient.Name;
                existingPatient.Phone = patient.Phone;
                existingPatient.Address = patient.Address;
                existingPatient.BirthDate = patient.BirthDate;
                existingPatient.Gender = patient.Gender;
                existingPatient.Email = patient.Email;
                existingPatient.ModifiedDate = DateTime.UtcNow;
                existingPatient.ModifiedBy = patient.ModifiedBy;

                _context.Patients.Update(existingPatient);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Patient updated successfully with ID: {PatientId}", patient.PatientID);
                return existingPatient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient with ID: {PatientId}", patient?.PatientID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting patient with ID: {PatientId}", id);

                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientID == id, cancellationToken);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found for deletion", id);
                    return false;
                }

                // Soft delete
                patient.IsActive = false;
                patient.ModifiedDate = DateTime.UtcNow;

                _context.Patients.Update(patient);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Patient deleted successfully with ID: {PatientId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting patient with ID: {PatientId}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Checking if patient exists with ID: {PatientId}", id);
                return await _context.Patients
                    .AsNoTracking()
                    .AnyAsync(p => p.PatientID == id && p.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking patient existence with ID: {PatientId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetAllAsync(cancellationToken);
                }

                _logger.LogInformation("Searching patients with term: {SearchTerm}", searchTerm);

                var normalizedSearchTerm = searchTerm.ToLower().Trim();

                return await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.IsActive &&
                        (p.Name.ToLower().Contains(normalizedSearchTerm) ||
                         (p.Phone != null && p.Phone.Contains(normalizedSearchTerm))))
                    .OrderBy(p => p.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching patients with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<Patient> GetPatientWithAgeAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving patient with age for ID: {PatientId}", id);
                return await GetByIdAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with age for ID: {PatientId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Patient>> GetByGenderAsync(char gender, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving patients by gender: {Gender}", gender);

                var genderStr = gender.ToString().ToUpper();

                return await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.IsActive && p.Gender.ToUpper() == genderStr)
                    .OrderBy(p => p.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patients by gender: {Gender}", gender);
                throw;
            }
        }

        public async Task<IEnumerable<Patient>> GetByAgeRangeAsync(int minAge, int maxAge, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving patients by age range: {MinAge} to {MaxAge}", minAge, maxAge);

                var today = DateTime.UtcNow;
                var maxBirthDate = today.AddYears(-minAge);
                var minBirthDate = today.AddYears(-maxAge - 1);

                return await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.IsActive &&
                        p.BirthDate >= minBirthDate &&
                        p.BirthDate <= maxBirthDate)
                    .OrderBy(p => p.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patients by age range: {MinAge} to {MaxAge}", minAge, maxAge);
                throw;
            }
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving total patient count");
                return await _context.Patients
                    .AsNoTracking()
                    .CountAsync(p => p.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total patient count");
                throw;
            }
        }

        public async Task<IEnumerable<Patient>> GetPatientsWithPendingAppointmentsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving patients with pending appointments");

                return await _context.Patients
                    .AsNoTracking()
                    .Include(p => p.Appointments)
                    .Where(p => p.IsActive &&
                        p.Appointments.Any(a => a.AppointmentStatus == 2 && a.IsActive))
                    .OrderBy(p => p.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patients with pending appointments");
                throw;
            }
        }

        public async Task<Patient> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email cannot be null or empty", nameof(email));
                }

                _logger.LogInformation("Retrieving patient by email: {Email}", email);

                var patient = await _context.Patients
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.IsActive && p.Email.ToLower() == email.ToLower(), cancellationToken);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with email {Email} not found", email);
                }

                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient by email: {Email}", email);
                throw;
            }
        }

        public async Task<Patient> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    throw new ArgumentException("Phone cannot be null or empty", nameof(phone));
                }

                _logger.LogInformation("Retrieving patient by phone: {Phone}", phone);

                var patient = await _context.Patients
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.IsActive && p.Phone == phone, cancellationToken);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with phone {Phone} not found", phone);
                }

                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient by phone: {Phone}", phone);
                throw;
            }
        }
    }
}
