namespace ClinicManagement.Application.DTOs;

public class StaffDto
{
    public int StaffId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime JoiningDate { get; set; }
    public decimal Salary { get; set; }
    public string Shift { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
