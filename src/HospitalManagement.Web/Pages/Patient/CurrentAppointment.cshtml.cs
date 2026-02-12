using System;
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
    public class CurrentAppointmentModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<CurrentAppointmentModel> _logger;

        public CurrentAppointmentModel(
            IPatientService patientService,
            ILogger<CurrentAppointmentModel> logger)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public AppointmentViewModel Appointment { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

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

                // Load current appointment
                var appointmentDto = await _patientService.GetCurrentAppointmentAsync(patientId);

                if (appointmentDto != null)
                {
                    Appointment = MapToAppointmentViewModel(appointmentDto);
                    _logger.LogInformation("Loaded current appointment {AppointmentId} for patient {PatientId}",
                        appointmentDto.AppointID, patientId);
                }
                else
                {
                    _logger.LogInformation("No current appointment found for patient {PatientId}", patientId);
                }

                // Check for success message in TempData
                if (TempData.ContainsKey("SuccessMessage"))
                {
                    SuccessMessage = TempData["SuccessMessage"]?.ToString();
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading current appointment");
                ErrorMessage = "An error occurred while loading your appointment. Please try again.";
                return Page();
            }
        }

        // Manual mapping method
        private AppointmentViewModel MapToAppointmentViewModel(AppointmentDto dto)
        {
            return new AppointmentViewModel
            {
                AppointID = dto.AppointID,
                DoctorID = dto.DoctorID,
                DoctorName = dto.DoctorName,
                PatientID = dto.PatientID,
                PatientName = dto.PatientName,
                Date = dto.Date,
                AppointmentStatus = dto.AppointmentStatus,
                AppointmentStatusText = dto.AppointmentStatusText,
                BillAmount = dto.BillAmount,
                BillStatus = dto.BillStatus,
                DoctorNotification = dto.DoctorNotification,
                PatientNotification = dto.PatientNotification,
                FeedbackStatus = dto.FeedbackStatus,
                Disease = dto.Disease,
                Progress = dto.Progress,
                Prescription = dto.Prescription
            };
        }

        // View Model
        public class AppointmentViewModel
        {
            public int AppointID { get; set; }
            public int? DoctorID { get; set; }
            public string DoctorName { get; set; }
            public int? PatientID { get; set; }
            public string PatientName { get; set; }
            public DateTime? Date { get; set; }
            public int? AppointmentStatus { get; set; }
            public string AppointmentStatusText { get; set; }
            public decimal? BillAmount { get; set; }
            public string BillStatus { get; set; }
            public int? DoctorNotification { get; set; }
            public int? PatientNotification { get; set; }
            public int? FeedbackStatus { get; set; }
            public string Disease { get; set; }
            public string Progress { get; set; }
            public string Prescription { get; set; }
        }
    }
}
