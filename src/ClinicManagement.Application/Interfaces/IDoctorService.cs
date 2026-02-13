using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DoctorDto?> GetByIdAsync(int doctorId, CancellationToken cancellationToken = default);
    Task<DoctorDto> CreateAsync(DoctorCreateDto doctorCreateDto, CancellationToken cancellationToken = default);
    Task<DoctorDto> UpdateAsync(DoctorUpdateDto doctorUpdateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int doctorId, CancellationToken cancellationToken = default);
    
    // Entity-specific methods
    Task<IEnumerable<DoctorDto>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DoctorDto>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default);
    Task<IEnumerable<DoctorDto>> GetAvailableDoctorsAsync(CancellationToken cancellationToken = default);
    Task<DoctorDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
