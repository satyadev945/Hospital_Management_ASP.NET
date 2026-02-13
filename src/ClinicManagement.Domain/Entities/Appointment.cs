namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents an appointment entity in the clinic management system.
/// </summary>
public class Appointment
{
    /// <summary>
    /// Gets or sets the unique identifier for the appointment.
    /// </summary>
    public int AppointmentID { get; set; }

    /// <summary>
    /// Gets or sets the patient identifier associated with this appointment.
    /// </summary>
    public int PatientID { get; set; }

    /// <summary>
    /// Gets or sets the doctor identifier associated with this appointment.
    /// </summary>
    public int DoctorID { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the appointment.
    /// </summary>
    public DateTime AppointmentDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the appointment.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the disease or condition being treated.
    /// </summary>
    public string? Disease { get; set; }

    /// <summary>
    /// Gets or sets the progress notes for the appointment.
    /// </summary>
    public string? Progress { get; set; }

    /// <summary>
    /// Gets or sets the prescription details for the appointment.
    /// </summary>
    public string? Prescription { get; set; }

    /// <summary>
    /// Gets or sets the date when the appointment record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the appointment record was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the appointment is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who created the appointment record.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username of the user who last modified the appointment record.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the patient associated with this appointment.
    /// </summary>
    public virtual Patient? Patient { get; set; }

    /// <summary>
    /// Gets or sets the doctor associated with this appointment.
    /// </summary>
    public virtual Doctor? Doctor { get; set; }

    /// <summary>
    /// Gets or sets the collection of bills for this appointment.
    /// </summary>
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
