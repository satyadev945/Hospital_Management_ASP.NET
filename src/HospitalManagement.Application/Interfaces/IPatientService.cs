using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

/// <summary>
/// Service interface for Patient operations
/// </summary>
public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PatientDto> CreateAsync(PatientCreateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, PatientUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<(bool IsValid, int PatientId, string Message)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
