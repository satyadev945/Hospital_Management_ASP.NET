namespace ClinicManagement.Application.DTOs;

public class BillDto
{
    public int BillId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int? AppointmentId { get; set; }
    public DateTime BillDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
}
