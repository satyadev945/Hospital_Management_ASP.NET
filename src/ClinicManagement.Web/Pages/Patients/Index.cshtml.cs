using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Patients;

public class IndexModel : PageModel
{
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IPatientRepository patientRepository, ILogger<IndexModel> logger)
    {
        _patientRepository = patientRepository;
        _logger = logger;
    }

    public IEnumerable<Patient> Patients { get; set; } = new List<Patient>();
    
    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var allPatients = await _patientRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(SearchString))
            {
                Patients = allPatients.Where(p =>
                    p.FirstName.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    p.LastName.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                    p.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                
                _logger.LogInformation("Searched patients with term: {SearchString}, Found: {Count}", 
                    SearchString, Patients.Count());
            }
            else
            {
                Patients = allPatients;
                _logger.LogInformation("Retrieved all patients. Count: {Count}", Patients.Count());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patients");
            TempData["ErrorMessage"] = "An error occurred while loading patients.";
            Patients = new List<Patient>();
        }
    }
}
