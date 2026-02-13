namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents a department entity in the clinic management system.
/// </summary>
public class Department
{
    /// <summary>
    /// Gets or sets the unique identifier for the department.
    /// </summary>
    public int DeptNo { get; set; }

    /// <summary>
    /// Gets or sets the department name.
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the department description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date when the department record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the department record was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the department is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who created the department record.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username of the user who last modified the department record.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the collection of doctors in this department.
    /// </summary>
    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
