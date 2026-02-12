using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for OtherStaff entity
/// </summary>
public interface IOtherStaffRepository
{
    Task<IEnumerable<OtherStaff>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OtherStaff?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<OtherStaff> AddAsync(OtherStaff staff, CancellationToken cancellationToken = default);
    Task UpdateAsync(OtherStaff staff, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OtherStaff>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
