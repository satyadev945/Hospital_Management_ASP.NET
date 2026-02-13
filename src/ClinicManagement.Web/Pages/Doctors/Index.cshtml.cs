using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Doctors;

public class IndexModel : PageModel
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IDoctorRepository doctorRepository, ILogger<IndexModel> logger)
    {
        _doctorRepository = doctorRepository;
        _logger = logger;
    }

    public IEnumerable<Doctor> Doctors { get; set; } = new List<Doctor>();
    
    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var allDoctors = await _doctorRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(SearchString))
            {
                Doctors = allDoctors.Where(d =>
                    d.FirstName.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    d.LastName.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    d.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    d.Specialization.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                
                _logger.LogInformation("Searched doctors with term: {SearchString}, Found: {Count}", 
                    SearchString, Doctors.Count());
            }
            else
            {
                Doctors = allDoctors;
                _logger.LogInformation("Retrieved all doctors. Count: {Count}", Doctors.Count());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctors");
            TempData["ErrorMessage"] = "An error occurred while loading doctors.";
            Doctors = new List<Doctor>();
        }
    }
}
