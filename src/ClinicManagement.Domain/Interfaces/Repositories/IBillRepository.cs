using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Bill entity operations
/// </summary>
public interface IBillRepository
{
    /// <summary>
    /// Gets all bills
    /// </summary>
    Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a bill by ID
    /// </summary>
    Task<Bill?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets bills by appointment ID
    /// </summary>
    Task<IEnumerable<Bill>> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new bill
    /// </summary>
    Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing bill
    /// </summary>
    Task UpdateAsync(Bill bill, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a bill by ID
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a bill exists by ID
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
