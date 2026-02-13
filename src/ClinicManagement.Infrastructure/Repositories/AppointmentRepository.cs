using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Appointment entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<AppointmentRepository> _logger;

    public AppointmentRepository(ClinicDbContext context, ILogger<AppointmentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving appointment with ID: {AppointmentId}", id);

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .Include(a => a.Bills)
                .FirstOrDefaultAsync(a => a.AppointmentId == id, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment with ID {AppointmentId} not found", id);
            }

            return appointment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointment with ID: {AppointmentId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all appointments");

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .Include(a => a.Bills)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Retrieved {Count} appointments", appointments.Count);
            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all appointments");
            throw;
        }
    }

    public async Task<Appointment> AddAsync(Appointment entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding new appointment for Patient ID: {PatientId} and Doctor ID: {DoctorId}", 
                entity.PatientId, entity.DoctorId);

            await _context.Appointments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added appointment with ID: {AppointmentId}", 
                entity.AppointmentId);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding appointment for Patient ID: {PatientId}", 
                entity.PatientId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding appointment for Patient ID: {PatientId}", entity.PatientId);
            throw;
        }
    }

    public async Task UpdateAsync(Appointment entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating appointment with ID: {AppointmentId}", entity.AppointmentId);

            _context.Appointments.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated appointment with ID: {AppointmentId}", 
                entity.AppointmentId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating appointment with ID: {AppointmentId}", 
                entity.AppointmentId);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating appointment with ID: {AppointmentId}", 
                entity.AppointmentId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating appointment with ID: {AppointmentId}", 
                entity.AppointmentId);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting appointment with ID: {AppointmentId}", id);

            var appointment = await _context.Appointments.FindAsync(new object[] { id }, cancellationToken);
            if (appointment == null)
            {
                _logger.LogWarning("Appointment with ID {AppointmentId} not found for deletion", id);
                throw new KeyNotFoundException($"Appointment with ID {id} not found");
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted appointment with ID: {AppointmentId}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting appointment with ID: {AppointmentId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting appointment with ID: {AppointmentId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving appointments for patient ID: {PatientId}", patientId);

            var appointments = await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .Include(a => a.Bills)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} appointments for patient ID: {PatientId}", 
                appointments.Count, patientId);
            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments for patient ID: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving appointments for doctor ID: {DoctorId}", doctorId);

            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .Include(a => a.Bills)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} appointments for doctor ID: {DoctorId}", 
                appointments.Count, doctorId);
            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments for doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving appointments for date: {Date}", date);

            var appointments = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == date.Date)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .Include(a => a.Bills)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} appointments for date: {Date}", appointments.Count, date);
            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments for date: {Date}", date);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving appointments between {StartDate} and {EndDate}", startDate, endDate);

            var appointments = await _context.Appointments
                .Where(a => a.AppointmentDate.Date >= startDate.Date && 
                           a.AppointmentDate.Date <= endDate.Date)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .Include(a => a.Bills)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} appointments between {StartDate} and {EndDate}", 
                appointments.Count, startDate, endDate);
            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments between {StartDate} and {EndDate}", 
                startDate, endDate);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                _logger.LogWarning("Empty status provided for appointment search");
                return Enumerable.Empty<Appointment>();
            }

            _logger.LogDebug("Retrieving appointments with status: {Status}", status);

            var appointments = await _context.Appointments
                .Where(a => a.Status == status)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .Include(a => a.Bills)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} appointments with status: {Status}", appointments.Count, status);
            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments with status: {Status}", status);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving upcoming appointments");

            var today = DateTime.Today;
            var appointments = await _context.Appointments
                .Where(a => a.AppointmentDate >= today && a.Status == "Scheduled")
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .Include(a => a.Bills)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} upcoming appointments", appointments.Count);
            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving upcoming appointments");
            throw;
        }
    }
}
