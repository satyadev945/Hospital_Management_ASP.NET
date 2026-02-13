using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Appointment entity
/// </summary>
public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicManagementDbContext _context;
    private readonly ILogger<AppointmentRepository> _logger;

    public AppointmentRepository(ClinicManagementDbContext context, ILogger<AppointmentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all appointments");
            return await _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p!.User)
                .Include(a => a.Doctor).ThenInclude(d => d!.User)
                .Where(a => a.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all appointments");
            throw;
        }
    }

    public async Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting appointment by ID: {AppointmentId}", id);
            return await _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p!.User)
                .Include(a => a.Doctor).ThenInclude(d => d!.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting appointment by ID: {AppointmentId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting appointments by patient ID: {PatientId}", patientId);
            return await _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p!.User)
                .Include(a => a.Doctor).ThenInclude(d => d!.User)
                .Where(a => a.PatientId == patientId && a.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting appointments by patient ID: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting appointments by doctor ID: {DoctorId}", doctorId);
            return await _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p!.User)
                .Include(a => a.Doctor).ThenInclude(d => d!.User)
                .Where(a => a.DoctorId == doctorId && a.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting appointments by doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting appointments by status: {Status}", status);
            return await _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p!.User)
                .Include(a => a.Doctor).ThenInclude(d => d!.User)
                .Where(a => a.Status == status && a.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting appointments by status: {Status}", status);
            throw;
        }
    }

    public async Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new appointment");
            appointment.CreatedDate = DateTime.UtcNow;
            appointment.IsActive = true;

            await _context.Appointments.AddAsync(appointment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Appointment added successfully: {AppointmentId}", appointment.Id);
            return appointment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding appointment");
            throw;
        }
    }

    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating appointment: {AppointmentId}", appointment.Id);
            appointment.ModifiedDate = DateTime.UtcNow;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Appointment updated successfully: {AppointmentId}", appointment.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating appointment: {AppointmentId}", appointment.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting appointment: {AppointmentId}", id);
            var appointment = await _context.Appointments.FindAsync(new object[] { id }, cancellationToken);

            if (appointment != null)
            {
                appointment.IsActive = false;
                appointment.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Appointment deleted successfully: {AppointmentId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting appointment: {AppointmentId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Appointments
                .AnyAsync(a => a.Id == id && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking appointment existence: {AppointmentId}", id);
            throw;
        }
    }
}
