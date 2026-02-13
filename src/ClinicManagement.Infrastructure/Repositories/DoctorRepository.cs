using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Doctor entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class DoctorRepository : IDoctorRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<DoctorRepository> _logger;

    public DoctorRepository(ClinicDbContext context, ILogger<DoctorRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving doctor with ID: {DoctorId}", id);

            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.DoctorId == id, cancellationToken);

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

    public async Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all doctors");

            var doctors = await _context.Doctors
                .Include(d => d.Department)
                .Include(d => d.Appointments)
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Retrieved {Count} doctors", doctors.Count);
            return doctors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all doctors");
            throw;
        }
    }

    public async Task<Doctor> AddAsync(Doctor entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding new doctor: {FirstName} {LastName}", entity.FirstName, entity.LastName);

            await _context.Doctors.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added doctor with ID: {DoctorId}", entity.DoctorId);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding doctor: {FirstName} {LastName}", 
                entity.FirstName, entity.LastName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding doctor: {FirstName} {LastName}", 
                entity.FirstName, entity.LastName);
            throw;
        }
    }

    public async Task UpdateAsync(Doctor entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating doctor with ID: {DoctorId}", entity.DoctorId);

            _context.Doctors.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated doctor with ID: {DoctorId}", entity.DoctorId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating doctor with ID: {DoctorId}", 
                entity.DoctorId);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating doctor with ID: {DoctorId}", 
                entity.DoctorId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating doctor with ID: {DoctorId}", entity.DoctorId);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting doctor with ID: {DoctorId}", id);

            var doctor = await _context.Doctors.FindAsync(new object[] { id }, cancellationToken);
            if (doctor == null)
            {
                _logger.LogWarning("Doctor with ID {DoctorId} not found for deletion", id);
                throw new KeyNotFoundException($"Doctor with ID {id} not found");
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted doctor with ID: {DoctorId}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting doctor with ID: {DoctorId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting doctor with ID: {DoctorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Doctor>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving doctors for department ID: {DepartmentId}", departmentId);

            var doctors = await _context.Doctors
                .Where(d => d.DepartmentId == departmentId)
                .Include(d => d.Department)
                .Include(d => d.Appointments)
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} doctors in department ID: {DepartmentId}", 
                doctors.Count, departmentId);
            return doctors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctors for department ID: {DepartmentId}", departmentId);
            throw;
        }
    }

    public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(specialization))
            {
                _logger.LogWarning("Empty specialization provided for doctor search");
                return Enumerable.Empty<Doctor>();
            }

            _logger.LogDebug("Retrieving doctors by specialization: {Specialization}", specialization);

            var doctors = await _context.Doctors
                .Where(d => d.Specialization == specialization)
                .Include(d => d.Department)
                .Include(d => d.Appointments)
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} doctors with specialization: {Specialization}", 
                doctors.Count, specialization);
            return doctors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctors by specialization: {Specialization}", 
                specialization);
            throw;
        }
    }

    public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving active doctors");

            var doctors = await _context.Doctors
                .Where(d => d.IsActive)
                .Include(d => d.Department)
                .Include(d => d.Appointments)
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} active doctors", doctors.Count);
            return doctors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active doctors");
            throw;
        }
    }

    public async Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync(DateTime date, TimeSpan time, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving available doctors for date: {Date} and time: {Time}", date, time);

            var doctors = await _context.Doctors
                .Where(d => d.IsActive &&
                           d.AvailableFrom <= time &&
                           d.AvailableTo >= time)
                .Include(d => d.Department)
                .Include(d => d.Appointments.Where(a => a.AppointmentDate.Date == date.Date))
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} available doctors for date: {Date} and time: {Time}", 
                doctors.Count, date, time);
            return doctors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available doctors for date: {Date} and time: {Time}", 
                date, time);
            throw;
        }
    }
}
