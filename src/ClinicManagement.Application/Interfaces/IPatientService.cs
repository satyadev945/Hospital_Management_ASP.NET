using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PatientDto?> GetByIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<PatientDto> CreateAsync(PatientCreateDto patientCreateDto, CancellationToken cancellationToken = default);
    Task<PatientDto> UpdateAsync(PatientUpdateDto patientUpdateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int patientId, CancellationToken cancellationToken = default);
    
    // Entity-specific methods
    Task<IEnumerable<PatientDto>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<PatientDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientDto>> GetByBloodGroupAsync(string bloodGroup, CancellationToken cancellationToken = default);
    Task<IEnumerable<PatientDto>> GetRecentPatientsAsync(int count, CancellationToken cancellationToken = default);
}
