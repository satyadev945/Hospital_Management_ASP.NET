namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents a patient in the clinic management system
/// </summary>
public class Patient
{
    /// <summary>
    /// Patient unique identifier
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
    /// Patient medical history
    /// </summary>
    public string? MedicalHistory { get; set; }

    /// <summary>
    /// Date when the patient was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Date when the patient was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Indicates if the patient is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Who created the patient
    /// </summary>
    public string CreatedBy { get; set; } = "System";

    /// <summary>
    /// Who modified the patient
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Navigation property to appointments
    /// </summary>
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
