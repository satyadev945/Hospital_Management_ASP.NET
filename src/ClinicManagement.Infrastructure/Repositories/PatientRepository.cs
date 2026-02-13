using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Patient entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class PatientRepository : IPatientRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<PatientRepository> _logger;

    public PatientRepository(ClinicDbContext context, ILogger<PatientRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving patient with ID: {PatientId}", id);

            var patient = await _context.Patients
                .Include(p => p.Appointments)
                .Include(p => p.Bills)
                .FirstOrDefaultAsync(p => p.PatientId == id, cancellationToken);

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

    public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all patients");

            var patients = await _context.Patients
                .Include(p => p.Appointments)
                .Include(p => p.Bills)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Retrieved {Count} patients", patients.Count);
            return patients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all patients");
            throw;
        }
    }

    public async Task<Patient> AddAsync(Patient entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding new patient: {FirstName} {LastName}", entity.FirstName, entity.LastName);

            await _context.Patients.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added patient with ID: {PatientId}", entity.PatientId);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding patient: {FirstName} {LastName}", 
                entity.FirstName, entity.LastName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding patient: {FirstName} {LastName}", 
                entity.FirstName, entity.LastName);
            throw;
        }
    }

    public async Task UpdateAsync(Patient entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating patient with ID: {PatientId}", entity.PatientId);

            _context.Patients.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated patient with ID: {PatientId}", entity.PatientId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating patient with ID: {PatientId}", 
                entity.PatientId);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating patient with ID: {PatientId}", 
                entity.PatientId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient with ID: {PatientId}", entity.PatientId);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting patient with ID: {PatientId}", id);

            var patient = await _context.Patients.FindAsync(new object[] { id }, cancellationToken);
            if (patient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found for deletion", id);
                throw new KeyNotFoundException($"Patient with ID {id} not found");
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted patient with ID: {PatientId}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting patient with ID: {PatientId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient with ID: {PatientId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Patient>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Empty search term provided for patient search");
                return Enumerable.Empty<Patient>();
            }

            _logger.LogDebug("Searching patients by name with term: {SearchTerm}", searchTerm);

            var patients = await _context.Patients
                .Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm))
                .Include(p => p.Appointments)
                .Include(p => p.Bills)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} patients matching search term: {SearchTerm}", 
                patients.Count, searchTerm);
            return patients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching patients by name with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<Patient>> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                _logger.LogWarning("Empty phone number provided for patient search");
                return Enumerable.Empty<Patient>();
            }

            _logger.LogDebug("Retrieving patients by phone: {Phone}", phone);

            var patients = await _context.Patients
                .Where(p => p.Phone == phone)
                .Include(p => p.Appointments)
                .Include(p => p.Bills)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} patients with phone: {Phone}", patients.Count, phone);
            return patients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patients by phone: {Phone}", phone);
            throw;
        }
    }

    public async Task<IEnumerable<Patient>> GetPatientsWithUpcomingAppointmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving patients with upcoming appointments");

            var today = DateTime.Today;
            var patients = await _context.Patients
                .Include(p => p.Appointments.Where(a => a.AppointmentDate >= today))
                .Where(p => p.Appointments.Any(a => a.AppointmentDate >= today))
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} patients with upcoming appointments", patients.Count);
            return patients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patients with upcoming appointments");
            throw;
        }
    }
}
