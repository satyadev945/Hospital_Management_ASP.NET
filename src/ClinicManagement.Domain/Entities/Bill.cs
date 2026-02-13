namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents a bill entity in the clinic management system.
/// </summary>
public class Bill
{
    /// <summary>
    /// Gets or sets the unique identifier for the bill.
    /// </summary>
    public int BillID { get; set; }

    /// <summary>
    /// Gets or sets the appointment identifier associated with this bill.
    /// </summary>
    public int AppointmentID { get; set; }

    /// <summary>
    /// Gets or sets the total amount for the bill.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the bill has been paid.
    /// </summary>
    public bool IsPaid { get; set; }

    /// <summary>
    /// Gets or sets the date when the bill was issued.
    /// </summary>
    public DateTime BillDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the bill record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the bill record was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the bill is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who created the bill record.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username of the user who last modified the bill record.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the appointment associated with this bill.
    /// </summary>
    public virtual Appointment? Appointment { get; set; }
}
