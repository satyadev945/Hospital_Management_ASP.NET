using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Patients;

public class CreateModel : PageModel
{
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IPatientRepository patientRepository, ILogger<CreateModel> logger)
    {
        _patientRepository = patientRepository;
        _logger = logger;
    }

    [BindProperty]
    public PatientInputModel Patient { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var patient = new Patient
            {
                FirstName = Patient.FirstName,
                LastName = Patient.LastName,
                DateOfBirth = Patient.DateOfBirth,
                Gender = Patient.Gender,
                Email = Patient.Email,
                PhoneNumber = Patient.PhoneNumber,
                Address = Patient.Address,
                MedicalHistory = Patient.MedicalHistory ?? string.Empty
            };

            await _patientRepository.AddAsync(patient);
            
            _logger.LogInformation("Created new patient: {FirstName} {LastName} (ID: {Id})", 
                patient.FirstName, patient.LastName, patient.Id);
            
            TempData["SuccessMessage"] = $"Patient {patient.FirstName} {patient.LastName} has been created successfully.";
            
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the patient. Please try again.");
            return Page();
        }
    }

    public class PatientInputModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Medical history cannot exceed 2000 characters")]
        [Display(Name = "Medical History")]
        public string? MedicalHistory { get; set; }
    }
}
