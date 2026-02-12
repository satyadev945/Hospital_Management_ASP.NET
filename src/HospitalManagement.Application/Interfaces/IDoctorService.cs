using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

/// <summary>
/// Service interface for Doctor operations
/// </summary>
public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DoctorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DoctorDto> CreateAsync(DoctorCreateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, DoctorUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DoctorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<DoctorDto>> GetByDepartmentAsync(int deptNo, CancellationToken cancellationToken = default);
    Task<(bool IsValid, int DoctorId, string Message)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
