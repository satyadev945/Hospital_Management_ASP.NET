using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

public interface IBillService
{
    Task<IEnumerable<BillDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BillDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BillDto> CreateAsync(BillCreateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, BillUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BillDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
}
