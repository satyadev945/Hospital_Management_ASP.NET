using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Appointment entity operations
/// </summary>
public interface IAppointmentRepository
{
    /// <summary>
    /// Gets all appointments
    /// </summary>
    Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an appointment by ID
    /// </summary>
    Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets appointments by patient ID
    /// </summary>
    Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets appointments by doctor ID
    /// </summary>
    Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets appointments by status
    /// </summary>
    Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new appointment
    /// </summary>
    Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing appointment
    /// </summary>
    Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an appointment by ID
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an appointment exists by ID
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
