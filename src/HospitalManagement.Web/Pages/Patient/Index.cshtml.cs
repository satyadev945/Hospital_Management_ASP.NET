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
    public class IndexModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IPatientService patientService,
            ILogger<IndexModel> logger)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // View Models
        public PatientViewModel PatientInfo { get; set; }
        public AppointmentViewModel CurrentAppointment { get; set; }
        public NotificationViewModel Notification { get; set; }
        public string PatientName { get; set; }
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

                // Load patient information
                var patientDto = await _patientService.GetByIdAsync(patientId);
                if (patientDto == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found", patientId);
                    ErrorMessage = "Patient information not found.";
                    return Page();
                }

                // Map to view model
                PatientInfo = MapToPatientViewModel(patientDto);
                PatientName = patientDto.Name;

                // Load current appointment
                try
                {
                    var currentAppointmentDto = await _patientService.GetCurrentAppointmentAsync(patientId);
                    if (currentAppointmentDto != null)
                    {
                        CurrentAppointment = MapToAppointmentViewModel(currentAppointmentDto);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading current appointment for patient {PatientId}", patientId);
                    // Continue loading other data
                }

                // Load notifications
                try
                {
                    var notificationDto = await _patientService.GetNotificationsAsync(patientId);
                    if (notificationDto != null)
                    {
                        Notification = MapToNotificationViewModel(notificationDto);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading notifications for patient {PatientId}", patientId);
                    // Continue loading other data
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
                _logger.LogError(ex, "Error loading patient dashboard");
                ErrorMessage = "An error occurred while loading the dashboard. Please try again.";
                return Page();
            }
        }

        // Manual mapping methods - no AutoMapper in Web layer
        private PatientViewModel MapToPatientViewModel(PatientDto dto)
        {
            return new PatientViewModel
            {
                PatientID = dto.PatientID,
                Name = dto.Name,
                Phone = dto.Phone,
                Address = dto.Address,
                BirthDate = dto.BirthDate,
                Age = dto.Age,
                Gender = dto.Gender,
                Email = dto.Email
            };
        }

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

        // View Models for the page
        public class PatientViewModel
        {
            public int PatientID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public DateTime BirthDate { get; set; }
            public int Age { get; set; }
            public char Gender { get; set; }
            public string Email { get; set; }
        }

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

        public class NotificationViewModel
        {
            public int NotificationType { get; set; }
            public string DoctorName { get; set; }
            public string Timings { get; set; }
            public string Message { get; set; }
        }
    }
}
