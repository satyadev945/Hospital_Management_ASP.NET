using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Patients;

public class EditModel : PageModel
{
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IPatientRepository patientRepository, ILogger<EditModel> logger)
    {
        _patientRepository = patientRepository;
        _logger = logger;
    }

    [BindProperty]
    public PatientInputModel Patient { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found with ID: {Id}", id);
                TempData["ErrorMessage"] = "Patient not found.";
                return RedirectToPage("./Index");
            }

            Patient = new PatientInputModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                Address = patient.Address,
                MedicalHistory = patient.MedicalHistory
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading patient for edit. ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the patient.";
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
            var patient = await _patientRepository.GetByIdAsync(Patient.Id);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found for update. ID: {Id}", Patient.Id);
                TempData["ErrorMessage"] = "Patient not found.";
                return RedirectToPage("./Index");
            }

            patient.FirstName = Patient.FirstName;
            patient.LastName = Patient.LastName;
            patient.DateOfBirth = Patient.DateOfBirth;
            patient.Gender = Patient.Gender;
            patient.Email = Patient.Email;
            patient.PhoneNumber = Patient.PhoneNumber;
            patient.Address = Patient.Address;
            patient.MedicalHistory = Patient.MedicalHistory ?? string.Empty;

            await _patientRepository.UpdateAsync(patient);
            
            _logger.LogInformation("Updated patient: {FirstName} {LastName} (ID: {Id})", 
                patient.FirstName, patient.LastName, patient.Id);
            
            TempData["SuccessMessage"] = $"Patient {patient.FirstName} {patient.LastName} has been updated successfully.";
            
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient. ID: {Id}", Patient.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the patient. Please try again.");
            return Page();
        }
    }

    public class PatientInputModel
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
