namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data transfer object for Department
/// </summary>
public class DepartmentDto
{
    public int DeptNo { get; set; }
    public string DeptName { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class DepartmentCreateDto
{
    public string DeptName { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class DepartmentUpdateDto
{
    public string DeptName { get; set; } = string.Empty;
    public string? Description { get; set; }
}
