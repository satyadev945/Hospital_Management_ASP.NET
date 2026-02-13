using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents an appointment in the clinic management system
/// </summary>
public class Appointment
{
    /// <summary>
    /// Appointment unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Patient ID
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Navigation property to Patient
    /// </summary>
    public Patient? Patient { get; set; }

    /// <summary>
    /// Doctor ID
    /// </summary>
    public int DoctorId { get; set; }

    /// <summary>
    /// Navigation property to Doctor
    /// </summary>
    public Doctor? Doctor { get; set; }

    /// <summary>
    /// Appointment date and time
    /// </summary>
    public DateTime AppointmentDate { get; set; }

    /// <summary>
    /// Appointment reason or description
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Appointment status
    /// </summary>
    public AppointmentStatus Status { get; set; }

    /// <summary>
    /// Doctor notes
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Date when the appointment was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Date when the appointment was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Indicates if the appointment is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Who created the appointment
    /// </summary>
    public string CreatedBy { get; set; } = "System";

    /// <summary>
    /// Who modified the appointment
    /// </summary>
    public string? ModifiedBy { get; set; }
}
