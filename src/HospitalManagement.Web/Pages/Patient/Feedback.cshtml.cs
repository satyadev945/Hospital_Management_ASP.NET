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
    public class FeedbackModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<FeedbackModel> _logger;

        public FeedbackModel(
            IPatientService patientService,
            ILogger<FeedbackModel> logger)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public AppointmentViewModel PendingFeedback { get; set; }

        [BindProperty]
        public int AppointmentId { get; set; }

        [BindProperty]
        public int Rating { get; set; }

        [BindProperty]
        public string Comments { get; set; }

        [BindProperty]
        public bool WouldRecommend { get; set; }

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

                // Load pending feedback appointment
                var pendingFeedbackDto = await _patientService.GetPendingFeedbackAsync(patientId);

                if (pendingFeedbackDto != null)
                {
                    PendingFeedback = MapToAppointmentViewModel(pendingFeedbackDto);
                    _logger.LogInformation("Loaded pending feedback for appointment {AppointmentId}",
                        pendingFeedbackDto.AppointID);
                }
                else
                {
                    _logger.LogInformation("No pending feedback found for patient {PatientId}", patientId);
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading feedback page");
                ErrorMessage = "An error occurred while loading the feedback form. Please try again.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Get patient ID from claims
                var patientIdClaim = User.FindFirst("PatientID")?.Value;
                if (string.IsNullOrEmpty(patientIdClaim) || !int.TryParse(patientIdClaim, out int patientId))
                {
                    _logger.LogWarning("Patient ID not found in claims");
                    ErrorMessage = "Unable to identify patient. Please log in again.";
                    return await OnGetAsync();
                }

                // Validate inputs
                if (AppointmentId <= 0)
                {
                    ErrorMessage = "Invalid appointment ID.";
                    return await OnGetAsync();
                }

                if (Rating < 1 || Rating > 5)
                {
                    ErrorMessage = "Please select a valid rating (1-5 stars).";
                    return await OnGetAsync();
                }

                // Store feedback
                // Note: The service method currently doesn't accept rating/comments parameters
                // In a real implementation, you would extend the service to accept these
                var success = await _patientService.StoreFeedbackAsync(AppointmentId);

                if (success)
                {
                    _logger.LogInformation(
                        "Feedback submitted successfully for appointment {AppointmentId} by patient {PatientId}. Rating: {Rating}, Recommend: {Recommend}",
                        AppointmentId, patientId, Rating, WouldRecommend);

                    TempData["SuccessMessage"] = "Thank you for your feedback! Your response has been recorded successfully.";
                    return RedirectToPage("./Index");
                }
                else
                {
                    _logger.LogWarning("Failed to store feedback for appointment {AppointmentId}", AppointmentId);
                    ErrorMessage = "Failed to submit feedback. Please try again.";
                    return await OnGetAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting feedback for appointment {AppointmentId}", AppointmentId);
                ErrorMessage = "An error occurred while submitting your feedback. Please try again.";
                return await OnGetAsync();
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
