using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using DoctorEntity = HospitalManagement.Domain.Entities.Doctor;

namespace HospitalManagement.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageDoctorsModel : PageModel
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly ILogger<ManageDoctorsModel> _logger;

        public ManageDoctorsModel(
            IDoctorRepository doctorRepository,
            IDepartmentRepository departmentRepository,
            ILoginRepository loginRepository,
            ILogger<ManageDoctorsModel> logger)
        {
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _loginRepository = loginRepository;
            _logger = logger;
        }

        public List<DoctorEntity> Doctors { get; set; } = new();
        public List<Department> Departments { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync(string? searchTerm = null)
        {
            try
            {
                SearchTerm = searchTerm;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var allDoctors = await _doctorRepository.GetAllAsync();
                    Doctors = allDoctors
                        .Where(d => d.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   d.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   (d.Specialization != null && d.Specialization.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        .OrderBy(d => d.Name)
                        .ToList();
                }
                else
                {
                    Doctors = (await _doctorRepository.GetAllAsync()).OrderBy(d => d.Name).ToList();
                }

                Departments = (await _departmentRepository.GetAllAsync()).OrderBy(d => d.DeptName).ToList();

                _logger.LogInformation("Loaded {Count} doctors", Doctors.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading doctors");
                ErrorMessage = "Error loading doctors. Please try again.";
            }
        }

        public async Task<IActionResult> OnPostAddAsync(
            string Name, string Email, string Phone, string Gender, DateTime BirthDate,
            int DeptNo, string Qualification, string Specialization, int WorkExperience,
            decimal ChargesPerVisit, decimal? MonthlySalary, string? Address,
            string Password, string ConfirmPassword)
        {
            try
            {
                // Validate passwords match
                if (Password != ConfirmPassword)
                {
                    ErrorMessage = "Passwords do not match.";
                    await OnGetAsync();
                    return Page();
                }

                // Check if email already exists
                var emailExists = await _loginRepository.EmailExistsAsync(Email);
                if (emailExists)
                {
                    ErrorMessage = "A user with this email already exists.";
                    await OnGetAsync();
                    return Page();
                }

                // Create login record
                var login = new Login
                {
                    Email = Email,
                    Password = Password, // In production, hash this
                    Type = 2, // Doctor type
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = User.Identity?.Name ?? "Admin"
                };

                var createdLogin = await _loginRepository.AddAsync(login);

                // Create doctor record
                var doctor = new DoctorEntity
                {
                    DoctorID = createdLogin.LoginID,
                    Name = Name,
                    Email = Email,
                    Password = Password, // In production, hash this
                    Phone = Phone,
                    Gender = Gender,
                    BirthDate = BirthDate,
                    Address = Address,
                    DeptNo = DeptNo,
                    Qualification = Qualification,
                    Specialization = Specialization,
                    WorkExperience = WorkExperience,
                    ChargesPerVisit = ChargesPerVisit,
                    MonthlySalary = MonthlySalary,
                    Status = 1,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = User.Identity?.Name ?? "Admin"
                };

                await _doctorRepository.AddAsync(doctor);

                _logger.LogInformation("Doctor added successfully: {Email}", Email);
                SuccessMessage = "Doctor added successfully!";

                return RedirectToPage(new { successMessage = SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding doctor");
                ErrorMessage = "Error adding doctor. Please try again.";
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int doctorId)
        {
            try
            {
                var doctor = await _doctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                {
                    ErrorMessage = "Doctor not found.";
                    return RedirectToPage();
                }

                // Soft delete
                doctor.IsActive = false;
                doctor.Status = 0;
                doctor.ModifiedDate = DateTime.UtcNow;
                doctor.ModifiedBy = User.Identity?.Name ?? "Admin";

                await _doctorRepository.UpdateAsync(doctor);

                // Also deactivate login
                var login = await _loginRepository.GetByIdAsync(doctorId);
                if (login != null)
                {
                    login.IsActive = false;
                    login.ModifiedDate = DateTime.UtcNow;
                    login.ModifiedBy = User.Identity?.Name ?? "Admin";
                    await _loginRepository.UpdateAsync(login);
                }

                _logger.LogInformation("Doctor deleted successfully: ID {DoctorId}", doctorId);
                SuccessMessage = "Doctor deleted successfully!";

                return RedirectToPage(new { successMessage = SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting doctor with ID {DoctorId}", doctorId);
                ErrorMessage = "Error deleting doctor. Please try again.";
                return RedirectToPage();
            }
        }
    }
}
