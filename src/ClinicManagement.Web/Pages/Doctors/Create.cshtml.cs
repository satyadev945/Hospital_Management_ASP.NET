using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Doctors;

public class CreateModel : PageModel
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IDoctorRepository doctorRepository, ILogger<CreateModel> logger)
    {
        _doctorRepository = doctorRepository;
        _logger = logger;
    }

    [BindProperty]
    public DoctorInputModel Doctor { get; set; } = new();

    public void OnGet()
    {
        // Set default value for IsActive
        Doctor.IsActive = true;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var doctor = new Doctor
            {
                FirstName = Doctor.FirstName,
                LastName = Doctor.LastName,
                Specialization = Doctor.Specialization,
                LicenseNumber = Doctor.LicenseNumber,
                Email = Doctor.Email,
                PhoneNumber = Doctor.PhoneNumber,
                IsActive = Doctor.IsActive
            };

            await _doctorRepository.AddAsync(doctor);
            
            _logger.LogInformation("Created new doctor: Dr. {FirstName} {LastName} (ID: {Id})", 
                doctor.FirstName, doctor.LastName, doctor.Id);
            
            TempData["SuccessMessage"] = $"Doctor {doctor.FirstName} {doctor.LastName} has been created successfully.";
            
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating doctor");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the doctor. Please try again.");
            return Page();
        }
    }

    public class DoctorInputModel
    {
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
        public bool IsActive { get; set; } = true;
    }
}
