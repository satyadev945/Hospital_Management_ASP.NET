using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

public interface IOtherStaffService
{
    Task<IEnumerable<OtherStaffDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OtherStaffDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<OtherStaffDto> CreateAsync(OtherStaffCreateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, OtherStaffUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OtherStaffDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
