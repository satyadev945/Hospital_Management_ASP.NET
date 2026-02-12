namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents a bill for an appointment
/// </summary>
public class Bill
{
    public int BillID { get; set; }
    public int PatientID { get; set; }
    public int AppointmentID { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime BillDate { get; set; } = DateTime.UtcNow;
    public DateTime? PaymentDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public virtual Patient? Patient { get; set; }
    public virtual Appointment? Appointment { get; set; }
}
