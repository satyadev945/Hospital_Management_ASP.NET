namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents a doctor in the clinic management system
/// </summary>
public class Doctor
{
    /// <summary>
    /// Doctor unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Reference to the User entity
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to User
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Doctor specialization
    /// </summary>
    public string Specialization { get; set; } = string.Empty;

    /// <summary>
    /// Doctor qualification
    /// </summary>
    public string Qualification { get; set; } = string.Empty;

    /// <summary>
    /// Years of experience
    /// </summary>
    public int Experience { get; set; }

    /// <summary>
    /// Doctor biography or description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date when the doctor was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Date when the doctor was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Indicates if the doctor is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Who created the doctor
    /// </summary>
    public string CreatedBy { get; set; } = "System";

    /// <summary>
    /// Who modified the doctor
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Navigation property to appointments
    /// </summary>
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
