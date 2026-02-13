using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Doctors;

public class EditModel : PageModel
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IDoctorRepository doctorRepository, ILogger<EditModel> logger)
    {
        _doctorRepository = doctorRepository;
        _logger = logger;
    }

    [BindProperty]
    public DoctorInputModel Doctor { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found with ID: {Id}", id);
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToPage("./Index");
            }

            Doctor = new DoctorInputModel
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialization = doctor.Specialization,
                LicenseNumber = doctor.LicenseNumber,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                IsActive = doctor.IsActive
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading doctor for edit. ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the doctor.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var doctor = await _doctorRepository.GetByIdAsync(Doctor.Id);
            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found for update. ID: {Id}", Doctor.Id);
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToPage("./Index");
            }

            doctor.FirstName = Doctor.FirstName;
            doctor.LastName = Doctor.LastName;
            doctor.Specialization = Doctor.Specialization;
            doctor.LicenseNumber = Doctor.LicenseNumber;
            doctor.Email = Doctor.Email;
            doctor.PhoneNumber = Doctor.PhoneNumber;
            doctor.IsActive = Doctor.IsActive;

            await _doctorRepository.UpdateAsync(doctor);
            
            _logger.LogInformation("Updated doctor: Dr. {FirstName} {LastName} (ID: {Id})", 
                doctor.FirstName, doctor.LastName, doctor.Id);
            
            TempData["SuccessMessage"] = $"Doctor {doctor.FirstName} {doctor.LastName} has been updated successfully.";
            
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating doctor. ID: {Id}", Doctor.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the doctor. Please try again.");
            return Page();
        }
    }

    public class DoctorInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(100, ErrorMessage = "Specialization cannot exceed 100 characters")]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "License number is required")]
        [StringLength(50, ErrorMessage = "License number cannot exceed 50 characters")]
        [Display(Name = "License Number")]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
