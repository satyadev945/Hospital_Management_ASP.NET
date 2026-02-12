using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.Interfaces.Services;
using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Web.Pages.Patient
{
    [Authorize(Roles = "Patient")]
    public class TreatmentHistoryModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<TreatmentHistoryModel> _logger;

        public TreatmentHistoryModel(
            IPatientService patientService,
            ILogger<TreatmentHistoryModel> logger)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<TreatmentHistoryViewModel> TreatmentHistory { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Get patient ID from claims
                var patientIdClaim = User.FindFirst("PatientID")?.Value;
                if (string.IsNullOrEmpty(patientIdClaim) || !int.TryParse(patientIdClaim, out int patientId))
                {
                    _logger.LogWarning("Patient ID not found in claims");
                    ErrorMessage = "Unable to identify patient. Please log in again.";
                    return Page();
                }

                // Load treatment history
                var treatmentHistoryDto = await _patientService.GetTreatmentHistoryAsync(patientId);

                if (treatmentHistoryDto != null && treatmentHistoryDto.Any())
                {
                    TreatmentHistory = treatmentHistoryDto.Select(MapToTreatmentHistoryViewModel).ToList();
                    _logger.LogInformation("Loaded {Count} treatment records for patient {PatientId}",
                        TreatmentHistory.Count(), patientId);
                }
                else
                {
                    TreatmentHistory = new List<TreatmentHistoryViewModel>();
                    _logger.LogInformation("No treatment history found for patient {PatientId}", patientId);
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading treatment history");
                ErrorMessage = "An error occurred while loading your treatment history. Please try again.";
                TreatmentHistory = new List<TreatmentHistoryViewModel>();
                return Page();
            }
        }

        // Manual mapping method
        private TreatmentHistoryViewModel MapToTreatmentHistoryViewModel(TreatmentHistoryDto dto)
        {
            return new TreatmentHistoryViewModel
            {
                Date = dto.Date,
                DoctorName = dto.DoctorName,
                DiagnosedDisease = dto.DiagnosedDisease,
                ProgressMade = dto.ProgressMade,
                Prescription = dto.Prescription
            };
        }

        // View Model
        public class TreatmentHistoryViewModel
        {
            public string Date { get; set; }
            public string DoctorName { get; set; }
            public string DiagnosedDisease { get; set; }
            public string ProgressMade { get; set; }
            public string Prescription { get; set; }
        }
    }
}
