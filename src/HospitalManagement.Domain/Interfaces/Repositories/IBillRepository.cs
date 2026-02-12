using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Bill entity
/// </summary>
public interface IBillRepository
{
    Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Bill?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default);
    Task UpdateAsync(Bill bill, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<Bill?> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default);
}
