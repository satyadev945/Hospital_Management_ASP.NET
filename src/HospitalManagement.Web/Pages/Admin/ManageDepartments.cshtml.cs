using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;

namespace HospitalManagement.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageDepartmentsModel : PageModel
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<ManageDepartmentsModel> _logger;

        public ManageDepartmentsModel(
            IDepartmentRepository departmentRepository,
            ILogger<ManageDepartmentsModel> logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        public List<Department> Departments { get; set; } = new();
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public int TotalDepartments { get; set; }
        public int ActiveDepartments { get; set; }
        public int TotalDoctors { get; set; }

        public async Task OnGetAsync(string? successMessage = null)
        {
            try
            {
                SuccessMessage = successMessage;

                Departments = (await _departmentRepository.GetAllAsync())
                    .OrderBy(d => d.DeptName)
                    .ToList();

                TotalDepartments = Departments.Count;
                ActiveDepartments = Departments.Count(d => d.IsActive);
                TotalDoctors = Departments.Sum(d => d.Doctors.Count);

                _logger.LogInformation("Loaded {Count} departments", Departments.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading departments");
                ErrorMessage = "Error loading departments. Please try again.";
            }
        }

        public async Task<IActionResult> OnPostAddAsync(string DeptName, string? Description, bool IsActive = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DeptName))
                {
                    ErrorMessage = "Department name is required.";
                    await OnGetAsync();
                    return Page();
                }

                var department = new Department
                {
                    DeptName = DeptName.Trim(),
                    Description = Description?.Trim(),
                    IsActive = IsActive,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = User.Identity?.Name ?? "Admin"
                };

                await _departmentRepository.AddAsync(department);

                _logger.LogInformation("Department added successfully: {DeptName}", DeptName);
                SuccessMessage = "Department added successfully!";

                return RedirectToPage(new { successMessage = SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding department");
                ErrorMessage = "Error adding department. Please try again.";
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostUpdateAsync(int DeptNo, string DeptName, string? Description, bool IsActive)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(DeptNo);
                if (department == null)
                {
                    ErrorMessage = "Department not found.";
                    return RedirectToPage();
                }

                if (string.IsNullOrWhiteSpace(DeptName))
                {
                    ErrorMessage = "Department name is required.";
                    await OnGetAsync();
                    return Page();
                }

                department.DeptName = DeptName.Trim();
                department.Description = Description?.Trim();
                department.IsActive = IsActive;
                department.ModifiedDate = DateTime.UtcNow;
                department.ModifiedBy = User.Identity?.Name ?? "Admin";

                await _departmentRepository.UpdateAsync(department);

                _logger.LogInformation("Department updated successfully: {DeptNo}", DeptNo);
                SuccessMessage = "Department updated successfully!";

                return RedirectToPage(new { successMessage = SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department");
                ErrorMessage = "Error updating department. Please try again.";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int deptNo)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(deptNo);
                if (department == null)
                {
                    ErrorMessage = "Department not found.";
                    return RedirectToPage();
                }

                // Check if department has doctors
                if (department.Doctors.Any())
                {
                    ErrorMessage = "Cannot delete department with assigned doctors. Please reassign or remove doctors first.";
                    return RedirectToPage();
                }

                var success = await _departmentRepository.DeleteAsync(deptNo);

                if (success)
                {
                    _logger.LogInformation("Department deleted successfully: {DeptNo}", deptNo);
                    SuccessMessage = "Department deleted successfully!";
                }
                else
                {
                    ErrorMessage = "Failed to delete department.";
                }

                return RedirectToPage(new { successMessage = SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department with ID {DeptNo}", deptNo);
                ErrorMessage = "Error deleting department. Please try again.";
                return RedirectToPage();
            }
        }
    }
}
