using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;

namespace HospitalManagement.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageStaffModel : PageModel
    {
        private readonly IOtherStaffRepository _staffRepository;
        private readonly ILogger<ManageStaffModel> _logger;

        public ManageStaffModel(
            IOtherStaffRepository staffRepository,
            ILogger<ManageStaffModel> logger)
        {
            _staffRepository = staffRepository;
            _logger = logger;
        }

        public List<OtherStaff> Staff { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync(string? searchTerm = null, string? successMessage = null)
        {
            try
            {
                SearchTerm = searchTerm;
                SuccessMessage = successMessage;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var allStaff = await _staffRepository.GetAllAsync();
                    Staff = allStaff
                        .Where(s => s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   (s.Email != null && s.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                                   s.Designation.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .OrderBy(s => s.Name)
                        .ToList();
                }
                else
                {
                    Staff = (await _staffRepository.GetAllAsync()).OrderBy(s => s.Name).ToList();
                }

                _logger.LogInformation("Loaded {Count} staff members", Staff.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading staff");
                ErrorMessage = "Error loading staff. Please try again.";
            }
        }

        public async Task<IActionResult> OnPostAddAsync(
            string Name, string? Email, string Phone, string Gender, DateTime? BirthDate,
            string? EmployeeID, string Designation, string? Department, string? Shift,
            string? EmploymentType, DateTime? JoiningDate, string? HighestQualification,
            decimal? Salary, string? Address, string? EmergencyContactName, string? EmergencyContact)
        {
            try
            {
                var staff = new OtherStaff
                {
                    Name = Name,
                    Email = Email,
                    Phone = Phone,
                    Gender = Gender,
                    BirthDate = BirthDate,
                    EmployeeID = EmployeeID,
                    Designation = Designation,
                    Department = Department,
                    Shift = Shift,
                    EmploymentType = EmploymentType,
                    JoiningDate = JoiningDate,
                    HighestQualification = HighestQualification,
                    Salary = Salary,
                    Address = Address,
                    EmergencyContactName = EmergencyContactName,
                    EmergencyContact = EmergencyContact,
                    Status = "Active",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = User.Identity?.Name ?? "Admin"
                };

                await _staffRepository.AddAsync(staff);

                _logger.LogInformation("Staff member added successfully: {Name}", Name);
                SuccessMessage = "Staff member added successfully!";

                return RedirectToPage(new { successMessage = SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding staff member");
                ErrorMessage = "Error adding staff member. Please try again.";
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int staffId)
        {
            try
            {
                var staff = await _staffRepository.GetByIdAsync(staffId);
                if (staff == null)
                {
                    ErrorMessage = "Staff member not found.";
                    return RedirectToPage();
                }

                // Soft delete
                staff.IsActive = false;
                staff.Status = "Terminated";
                staff.ModifiedDate = DateTime.UtcNow;
                staff.ModifiedBy = User.Identity?.Name ?? "Admin";

                await _staffRepository.UpdateAsync(staff);

                _logger.LogInformation("Staff member deleted successfully: ID {StaffId}", staffId);
                SuccessMessage = "Staff member deleted successfully!";

                return RedirectToPage(new { successMessage = SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting staff member with ID {StaffId}", staffId);
                ErrorMessage = "Error deleting staff member. Please try again.";
                return RedirectToPage();
            }
        }
    }
}
