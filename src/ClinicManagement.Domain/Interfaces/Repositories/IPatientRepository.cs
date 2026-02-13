using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Patient entity operations
/// </summary>
public interface IPatientRepository
{
    /// <summary>
    /// Gets all patients
    /// </summary>
    Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a patient by ID
    /// </summary>
    Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a patient by user ID
    /// </summary>
    Task<Patient?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new patient
    /// </summary>
    Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing patient
    /// </summary>
    Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a patient by ID
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a patient exists by ID
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches patients by name
    /// </summary>
    Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
