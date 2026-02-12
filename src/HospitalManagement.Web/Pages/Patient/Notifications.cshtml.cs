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
    public class NotificationsModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<NotificationsModel> _logger;

        public NotificationsModel(
            IPatientService patientService,
            ILogger<NotificationsModel> logger)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public NotificationViewModel Notification { get; set; }
        public int? DoctorId { get; set; }
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

                // Load notifications
                var notificationDto = await _patientService.GetNotificationsAsync(patientId);

                if (notificationDto != null && notificationDto.NotificationType > 0)
                {
                    Notification = MapToNotificationViewModel(notificationDto);
                    _logger.LogInformation("Loaded notification type {NotificationType} for patient {PatientId}",
                        notificationDto.NotificationType, patientId);

                    // Try to get doctor ID from current appointment if available
                    try
                    {
                        var currentAppointment = await _patientService.GetCurrentAppointmentAsync(patientId);
                        if (currentAppointment != null && currentAppointment.DoctorID.HasValue)
                        {
                            DoctorId = currentAppointment.DoctorID.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not load doctor ID for notification");
                        // Continue without doctor ID
                    }
                }
                else
                {
                    _logger.LogInformation("No notifications found for patient {PatientId}", patientId);
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading notifications");
                ErrorMessage = "An error occurred while loading your notifications. Please try again.";
                return Page();
            }
        }

        // Manual mapping method
        private NotificationViewModel MapToNotificationViewModel(NotificationDto dto)
        {
            return new NotificationViewModel
            {
                NotificationType = dto.NotificationType,
                DoctorName = dto.DoctorName,
                Timings = dto.Timings,
                Message = dto.Message
            };
        }

        // View Model
        public class NotificationViewModel
        {
            public int NotificationType { get; set; }
            public string DoctorName { get; set; }
            public string Timings { get; set; }
            public string Message { get; set; }
        }
    }
}
