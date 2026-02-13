using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Doctor entity operations
/// </summary>
public interface IDoctorRepository
{
    /// <summary>
    /// Gets all doctors
    /// </summary>
    Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a doctor by ID
    /// </summary>
    Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a doctor by user ID
    /// </summary>
    Task<Doctor?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new doctor
    /// </summary>
    Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing doctor
    /// </summary>
    Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a doctor by ID
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a doctor exists by ID
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches doctors by name or specialization
    /// </summary>
    Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
