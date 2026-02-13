using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Patient entity
/// </summary>
public class PatientRepository : IPatientRepository
{
    private readonly ClinicManagementDbContext _context;
    private readonly ILogger<PatientRepository> _logger;

    public PatientRepository(ClinicManagementDbContext context, ILogger<PatientRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all patients");
            return await _context.Patients
                .Include(p => p.User)
                .Where(p => p.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all patients");
            throw;
        }
    }

    public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting patient by ID: {PatientId}", id);
            return await _context.Patients
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting patient by ID: {PatientId}", id);
            throw;
        }
    }

    public async Task<Patient?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting patient by user ID: {UserId}", userId);
            return await _context.Patients
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting patient by user ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new patient for user ID: {UserId}", patient.UserId);
            patient.CreatedDate = DateTime.UtcNow;
            patient.IsActive = true;

            await _context.Patients.AddAsync(patient, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Patient added successfully: {PatientId}", patient.Id);
            return patient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding patient for user ID: {UserId}", patient.UserId);
            throw;
        }
    }

    public async Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating patient: {PatientId}", patient.Id);
            patient.ModifiedDate = DateTime.UtcNow;

            _context.Patients.Update(patient);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Patient updated successfully: {PatientId}", patient.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient: {PatientId}", patient.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting patient: {PatientId}", id);
            var patient = await _context.Patients.FindAsync(new object[] { id }, cancellationToken);

            if (patient != null)
            {
                patient.IsActive = false;
                patient.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Patient deleted successfully: {PatientId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient: {PatientId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Patients
                .AnyAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking patient existence: {PatientId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching patients with term: {SearchTerm}", searchTerm);
            return await _context.Patients
                .Include(p => p.User)
                .Where(p => p.IsActive && p.User != null && p.User.Name.Contains(searchTerm))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching patients with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
