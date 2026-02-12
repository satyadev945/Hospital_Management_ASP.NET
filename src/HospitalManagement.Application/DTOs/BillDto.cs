namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data transfer object for Bill
/// </summary>
public class BillDto
{
    public int BillID { get; set; }
    public int PatientID { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int AppointmentID { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime BillDate { get; set; }
    public DateTime? PaymentDate { get; set; }
}

public class BillCreateDto
{
    public int PatientID { get; set; }
    public int AppointmentID { get; set; }
    public decimal Amount { get; set; }
}

public class BillUpdateDto
{
    public bool IsPaid { get; set; }
    public DateTime? PaymentDate { get; set; }
}
