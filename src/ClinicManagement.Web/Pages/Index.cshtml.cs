using ClinicManagement.Core.Entities;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        ILogger<IndexModel> logger)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _logger = logger;
    }

    public int TotalPatients { get; set; }
    public int TotalDoctors { get; set; }
    public int TodayAppointments { get; set; }
    public IEnumerable<Patient> RecentPatients { get; set; } = new List<Patient>();
    public IEnumerable<Doctor> RecentDoctors { get; set; } = new List<Doctor>();

    public async Task OnGetAsync()
    {
        try
        {
            // Get all patients and doctors
            var allPatients = await _patientRepository.GetAllAsync();
            var allDoctors = await _doctorRepository.GetAllAsync();

            // Calculate statistics
            TotalPatients = allPatients.Count();
            TotalDoctors = allDoctors.Count();
            TodayAppointments = 0; // Placeholder for appointments feature

            // Get recent patients (ordered by ID descending as a proxy for recent)
            RecentPatients = allPatients
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToList();

            // Get recent doctors
            RecentDoctors = allDoctors
                .OrderByDescending(d => d.Id)
                .Take(5)
                .ToList();

            _logger.LogInformation("Dashboard loaded successfully. Patients: {PatientCount}, Doctors: {DoctorCount}", 
                TotalPatients, TotalDoctors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading dashboard data");
            TotalPatients = 0;
            TotalDoctors = 0;
            TodayAppointments = 0;
        }
    }
}
