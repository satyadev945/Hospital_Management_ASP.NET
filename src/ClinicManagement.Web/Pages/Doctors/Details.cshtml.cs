using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Doctors;

public class DetailsModel : PageModel
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IDoctorRepository doctorRepository, ILogger<DetailsModel> logger)
    {
        _doctorRepository = doctorRepository;
        _logger = logger;
    }

    public Doctor Doctor { get; set; } = null!;

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

            Doctor = doctor;
            _logger.LogInformation("Viewing details for doctor: Dr. {FirstName} {LastName} (ID: {Id})", 
                doctor.FirstName, doctor.LastName, doctor.Id);
            
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading doctor details. ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading doctor details.";
            return RedirectToPage("./Index");
        }
    }
}
