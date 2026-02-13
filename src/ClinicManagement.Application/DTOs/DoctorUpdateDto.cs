namespace ClinicManagement.Application.DTOs;

public class DoctorUpdateDto
{
    public int DoctorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string Availability { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
}
