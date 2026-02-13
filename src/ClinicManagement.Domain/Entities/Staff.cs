namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents a staff member entity in the clinic management system.
/// </summary>
public class Staff
{
    /// <summary>
    /// Gets or sets the unique identifier for the staff member.
    /// </summary>
    public int StaffID { get; set; }

    /// <summary>
    /// Gets or sets the staff member's full name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff member's phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff member's birth date.
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the staff member's gender.
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff member's address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff member's salary.
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// Gets or sets the staff member's qualification.
    /// </summary>
    public string Qualification { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff member's designation.
    /// </summary>
    public string Designation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the staff record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the staff record was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the staff member is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who created the staff record.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username of the user who last modified the staff record.
    /// </summary>
    public string? ModifiedBy { get; set; }
}
