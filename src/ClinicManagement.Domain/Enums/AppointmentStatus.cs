namespace ClinicManagement.Domain.Enums;

/// <summary>
/// Represents the status of an appointment
/// </summary>
public enum AppointmentStatus
{
    /// <summary>
    /// Appointment is pending approval
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Appointment is approved
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Appointment is completed
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Appointment is cancelled
    /// </summary>
    Cancelled = 3
}
