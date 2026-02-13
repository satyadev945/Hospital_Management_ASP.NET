namespace ClinicManagement.Application.DTOs;

public class DepartmentUpdateDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int? HeadOfDepartmentId { get; set; }
}
