namespace ClinicManagement.Application.DTOs;

public class BillUpdateDto
{
    public int BillId { get; set; }
    public int PatientId { get; set; }
    public int? AppointmentId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
}
