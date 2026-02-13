using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Doctor entity
/// </summary>
public class DoctorRepository : IDoctorRepository
{
    private readonly ClinicManagementDbContext _context;
    private readonly ILogger<DoctorRepository> _logger;

    public DoctorRepository(ClinicManagementDbContext context, ILogger<DoctorRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all doctors");
            return await _context.Doctors
                .Include(d => d.User)
                .Where(d => d.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all doctors");
            throw;
        }
    }

    public async Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting doctor by ID: {DoctorId}", id);
            return await _context.Doctors
                .Include(d => d.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting doctor by ID: {DoctorId}", id);
            throw;
        }
    }

    public async Task<Doctor?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting doctor by user ID: {UserId}", userId);
            return await _context.Doctors
                .Include(d => d.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == userId && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting doctor by user ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new doctor for user ID: {UserId}", doctor.UserId);
            doctor.CreatedDate = DateTime.UtcNow;
            doctor.IsActive = true;

            await _context.Doctors.AddAsync(doctor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Doctor added successfully: {DoctorId}", doctor.Id);
            return doctor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding doctor for user ID: {UserId}", doctor.UserId);
            throw;
        }
    }

    public async Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating doctor: {DoctorId}", doctor.Id);
            doctor.ModifiedDate = DateTime.UtcNow;

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Doctor updated successfully: {DoctorId}", doctor.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating doctor: {DoctorId}", doctor.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting doctor: {DoctorId}", id);
            var doctor = await _context.Doctors.FindAsync(new object[] { id }, cancellationToken);

            if (doctor != null)
            {
                doctor.IsActive = false;
                doctor.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Doctor deleted successfully: {DoctorId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting doctor: {DoctorId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Doctors
                .AnyAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking doctor existence: {DoctorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching doctors with term: {SearchTerm}", searchTerm);
            return await _context.Doctors
                .Include(d => d.User)
                .Where(d => d.IsActive &&
                    ((d.User != null && d.User.Name.Contains(searchTerm)) ||
                     d.Specialization.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching doctors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
