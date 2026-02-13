using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DepartmentDto?> GetByIdAsync(int departmentId, CancellationToken cancellationToken = default);
    Task<DepartmentDto> CreateAsync(DepartmentCreateDto departmentCreateDto, CancellationToken cancellationToken = default);
    Task<DepartmentDto> UpdateAsync(DepartmentUpdateDto departmentUpdateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int departmentId, CancellationToken cancellationToken = default);
    
    // Entity-specific methods
    Task<DepartmentDto?> GetByNameAsync(string departmentName, CancellationToken cancellationToken = default);
    Task<IEnumerable<DepartmentDto>> GetByLocationAsync(string location, CancellationToken cancellationToken = default);
    Task<int> GetDoctorCountAsync(int departmentId, CancellationToken cancellationToken = default);
    Task<int> GetStaffCountAsync(int departmentId, CancellationToken cancellationToken = default);
}
