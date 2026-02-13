namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents a bill in the clinic management system
/// </summary>
public class Bill
{
    /// <summary>
    /// Bill unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Appointment ID
    /// </summary>
    public int AppointmentId { get; set; }

    /// <summary>
    /// Navigation property to Appointment
    /// </summary>
    public Appointment? Appointment { get; set; }

    /// <summary>
    /// Bill amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Bill description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the bill is paid
    /// </summary>
    public bool IsPaid { get; set; }

    /// <summary>
    /// Date when the bill was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Date when the bill was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Indicates if the bill is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Who created the bill
    /// </summary>
    public string CreatedBy { get; set; } = "System";

    /// <summary>
    /// Who modified the bill
    /// </summary>
    public string? ModifiedBy { get; set; }
}
