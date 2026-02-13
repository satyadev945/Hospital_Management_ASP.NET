using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Patients;

public class DetailsModel : PageModel
{
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IPatientRepository patientRepository, ILogger<DetailsModel> logger)
    {
        _patientRepository = patientRepository;
        _logger = logger;
    }

    public Patient Patient { get; set; } = null!;
    public int Age => DateTime.Today.Year - Patient.DateOfBirth.Year - 
                     (DateTime.Today.DayOfYear < Patient.DateOfBirth.DayOfYear ? 1 : 0);

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

            Patient = patient;
            _logger.LogInformation("Viewing details for patient: {FirstName} {LastName} (ID: {Id})", 
                patient.FirstName, patient.LastName, patient.Id);
            
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading patient details. ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading patient details.";
            return RedirectToPage("./Index");
        }
    }
}
