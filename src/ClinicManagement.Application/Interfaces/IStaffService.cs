using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.Interfaces;

public interface IStaffService
{
    Task<IEnumerable<StaffDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<StaffDto?> GetByIdAsync(int staffId, CancellationToken cancellationToken = default);
    Task<StaffDto> CreateAsync(StaffCreateDto staffCreateDto, CancellationToken cancellationToken = default);
    Task<StaffDto> UpdateAsync(StaffUpdateDto staffUpdateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int staffId, CancellationToken cancellationToken = default);
    
    // Entity-specific methods
    Task<IEnumerable<StaffDto>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<StaffDto>> GetByPositionAsync(string position, CancellationToken cancellationToken = default);
    Task<IEnumerable<StaffDto>> GetByShiftAsync(string shift, CancellationToken cancellationToken = default);
    Task<StaffDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<StaffDto>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
}
