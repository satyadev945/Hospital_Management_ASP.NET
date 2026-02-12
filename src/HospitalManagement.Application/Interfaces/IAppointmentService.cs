using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

/// <summary>
/// Service interface for Appointment operations
/// </summary>
public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AppointmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AppointmentDto> CreateAsync(AppointmentCreateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, AppointmentUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetPendingByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetTodaysByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
    Task ApproveAppointmentAsync(int id, CancellationToken cancellationToken = default);
}
