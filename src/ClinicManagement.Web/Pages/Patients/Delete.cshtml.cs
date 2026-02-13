using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Patients;

public class DeleteModel : PageModel
{
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IPatientRepository patientRepository, ILogger<DeleteModel> logger)
    {
        _patientRepository = patientRepository;
        _logger = logger;
    }

    [BindProperty]
    public Patient Patient { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found for deletion. ID: {Id}", id);
                TempData["ErrorMessage"] = "Patient not found.";
                return RedirectToPage("./Index");
            }

            Patient = patient;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading patient for deletion. ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the patient.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var patient = await _patientRepository.GetByIdAsync(Patient.Id);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found for deletion. ID: {Id}", Patient.Id);
                TempData["ErrorMessage"] = "Patient not found.";
                return RedirectToPage("./Index");
            }

            await _patientRepository.DeleteAsync(Patient.Id);
            
            _logger.LogInformation("Deleted patient: {FirstName} {LastName} (ID: {Id})", 
                patient.FirstName, patient.LastName, patient.Id);
            
            TempData["SuccessMessage"] = $"Patient {patient.FirstName} {patient.LastName} has been deleted successfully.";
            
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient. ID: {Id}", Patient.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the patient. Please try again.";
            return RedirectToPage("./Index");
        }
    }
}
