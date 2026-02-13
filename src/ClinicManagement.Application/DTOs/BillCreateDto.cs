namespace ClinicManagement.Application.DTOs;

public class BillCreateDto
{
    public int PatientId { get; set; }
    public int? AppointmentId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
