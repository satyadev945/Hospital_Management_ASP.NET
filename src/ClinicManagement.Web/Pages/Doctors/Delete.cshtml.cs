using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Doctors;

public class DeleteModel : PageModel
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IDoctorRepository doctorRepository, ILogger<DeleteModel> logger)
    {
        _doctorRepository = doctorRepository;
        _logger = logger;
    }

    [BindProperty]
    public Doctor Doctor { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found for deletion. ID: {Id}", id);
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToPage("./Index");
            }

            Doctor = doctor;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading doctor for deletion. ID: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the doctor.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var doctor = await _doctorRepository.GetByIdAsync(Doctor.Id);
            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found for deletion. ID: {Id}", Doctor.Id);
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToPage("./Index");
            }

            await _doctorRepository.DeleteAsync(Doctor.Id);
            
            _logger.LogInformation("Deleted doctor: Dr. {FirstName} {LastName} (ID: {Id})", 
                doctor.FirstName, doctor.LastName, doctor.Id);
            
            TempData["SuccessMessage"] = $"Doctor {doctor.FirstName} {doctor.LastName} has been deleted successfully.";
            
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting doctor. ID: {Id}", Doctor.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the doctor. Please try again.";
            return RedirectToPage("./Index");
        }
    }
}
