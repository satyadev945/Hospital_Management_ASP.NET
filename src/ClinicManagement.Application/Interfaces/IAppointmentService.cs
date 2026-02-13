using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AppointmentDto?> GetByIdAsync(int appointmentId, CancellationToken cancellationToken = default);
    Task<AppointmentDto> CreateAsync(AppointmentCreateDto appointmentCreateDto, CancellationToken cancellationToken = default);
    Task<AppointmentDto> UpdateAsync(AppointmentUpdateDto appointmentUpdateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int appointmentId, CancellationToken cancellationToken = default);
    
    // Entity-specific methods
    Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetTodayAppointmentsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(int daysAhead, CancellationToken cancellationToken = default);
    Task<bool> UpdateStatusAsync(int appointmentId, string status, CancellationToken cancellationToken = default);
}
