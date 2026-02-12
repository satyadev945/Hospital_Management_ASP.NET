using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using PatientEntity = HospitalManagement.Domain.Entities.Patient;

namespace HospitalManagement.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManagePatientsModel : PageModel
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly ILogger<ManagePatientsModel> _logger;

        public ManagePatientsModel(
            IPatientRepository patientRepository,
            ILoginRepository loginRepository,
            ILogger<ManagePatientsModel> logger)
        {
            _patientRepository = patientRepository;
            _loginRepository = loginRepository;
            _logger = logger;
        }

        public List<PatientEntity> Patients { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public int TotalPatients { get; set; }
        public int ActivePatients { get; set; }
        public int NewPatientsThisMonth { get; set; }
        public int TotalAppointments { get; set; }

        public async Task OnGetAsync(string? searchTerm = null, string? successMessage = null)
        {
            try
            {
                SearchTerm = searchTerm;
                SuccessMessage = successMessage;

                var allPatients = (await _patientRepository.GetAllAsync()).ToList();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    Patients = allPatients
                        .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   p.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   (p.Phone != null && p.Phone.Contains(searchTerm)))
                        .OrderBy(p => p.Name)
                        .ToList();
                }
                else
                {
                    Patients = allPatients.OrderByDescending(p => p.CreatedDate).ToList();
                }

                // Calculate statistics
                TotalPatients = allPatients.Count;
                ActivePatients = allPatients.Count(p => p.IsActive);
                NewPatientsThisMonth = allPatients.Count(p =>
                    p.CreatedDate.Month == DateTime.Now.Month &&
                    p.CreatedDate.Year == DateTime.Now.Year);

                // Count total appointments across all patients
                TotalAppointments = allPatients.Sum(p => p.Appointments.Count);

                _logger.LogInformation("Loaded {Count} patients", Patients.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading patients");
                ErrorMessage = "Error loading patients. Please try again.";
            }
        }

        public async Task<IActionResult> OnPostDeactivateAsync(int patientId)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    ErrorMessage = "Patient not found.";
                    return RedirectToPage();
                }

                // Deactivate patient
                patient.IsActive = false;
                patient.ModifiedDate = DateTime.UtcNow;
                patient.ModifiedBy = User.Identity?.Name ?? "Admin";

                await _patientRepository.UpdateAsync(patient);

                // Also deactivate login
                var login = await _loginRepository.GetByIdAsync(patientId);
                if (login != null)
                {
                    login.IsActive = false;
                    login.ModifiedDate = DateTime.UtcNow;
                    login.ModifiedBy = User.Identity?.Name ?? "Admin";
                    await _loginRepository.UpdateAsync(login);
                }

                _logger.LogInformation("Patient deactivated successfully: ID {PatientId}", patientId);
                SuccessMessage = "Patient deactivated successfully!";

                return RedirectToPage(new { successMessage = SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating patient with ID {PatientId}", patientId);
                ErrorMessage = "Error deactivating patient. Please try again.";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostActivateAsync(int patientId)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    ErrorMessage = "Patient not found.";
                    return RedirectToPage();
                }

                // Activate patient
                patient.IsActive = true;
                patient.ModifiedDate = DateTime.UtcNow;
                patient.ModifiedBy = User.Identity?.Name ?? "Admin";

                await _patientRepository.UpdateAsync(patient);

                // Also activate login
                var login = await _loginRepository.GetByIdAsync(patientId);
                if (login != null)
                {
                    login.IsActive = true;
                    login.ModifiedDate = DateTime.UtcNow;
                    login.ModifiedBy = User.Identity?.Name ?? "Admin";
                    await _loginRepository.UpdateAsync(login);
                }

                _logger.LogInformation("Patient activated successfully: ID {PatientId}", patientId);
                SuccessMessage = "Patient activated successfully!";

                return RedirectToPage(new { successMessage = SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating patient with ID {PatientId}", patientId);
                ErrorMessage = "Error activating patient. Please try again.";
                return RedirectToPage();
            }
        }
    }
}
