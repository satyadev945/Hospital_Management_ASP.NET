namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents a doctor entity in the clinic management system.
/// </summary>
public class Doctor
{
    /// <summary>
    /// Gets or sets the unique identifier for the doctor.
    /// </summary>
    public int DoctorID { get; set; }

    /// <summary>
    /// Gets or sets the doctor's full name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor's phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor's birth date.
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the doctor's gender.
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor's address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the department number to which the doctor belongs.
    /// </summary>
    public int DeptNo { get; set; }

    /// <summary>
    /// Gets or sets the doctor's years of experience.
    /// </summary>
    public int Experience { get; set; }

    /// <summary>
    /// Gets or sets the doctor's salary.
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// Gets or sets the charges per visit for the doctor.
    /// </summary>
    public decimal ChargesPerVisit { get; set; }

    /// <summary>
    /// Gets or sets the doctor's specialization.
    /// </summary>
    public string Specialization { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor's qualification.
    /// </summary>
    public string Qualification { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the doctor is active.
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// Gets or sets the date when the doctor record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the doctor record was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the doctor is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who created the doctor record.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username of the user who last modified the doctor record.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the department to which the doctor belongs.
    /// </summary>
    public virtual Department? Department { get; set; }

    /// <summary>
    /// Gets or sets the collection of appointments for this doctor.
    /// </summary>
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
