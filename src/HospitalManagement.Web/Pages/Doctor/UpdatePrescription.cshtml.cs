using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Interfaces.Services;

namespace HospitalManagement.Web.Pages.Doctor
{
    [Authorize(Roles = "Doctor")]
    public class UpdatePrescriptionModel : PageModel
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<UpdatePrescriptionModel> _logger;

        public UpdatePrescriptionModel(
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            ILogger<UpdatePrescriptionModel> logger)
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
            _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [BindProperty]
        public int AppointmentId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Disease diagnosis is required")]
        [StringLength(100, ErrorMessage = "Disease name cannot exceed 100 characters")]
        public string Disease { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Progress notes are required")]
        [StringLength(500, ErrorMessage = "Progress notes cannot exceed 500 characters")]
        public string Progress { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Prescription is required")]
        [StringLength(500, ErrorMessage = "Prescription cannot exceed 500 characters")]
        public string Prescription { get; set; } = string.Empty;

        [BindProperty]
        public bool MarkAsCompleted { get; set; }

        [BindProperty]
        public bool IsBillPaid { get; set; }

        public AppointmentDto? AppointmentDetails { get; set; }
        public bool IsReadOnly { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    ErrorMessage = "Appointment ID is required.";
                    return RedirectToPage("TodaysAppointments");
                }

                AppointmentId = id.Value;

                var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(doctorIdClaim) || !int.TryParse(doctorIdClaim, out int doctorId))
                {
                    _logger.LogWarning("Doctor ID not found in claims");
                    ErrorMessage = "Unable to identify doctor. Please log in again.";
                    return RedirectToPage("/Account/Login");
                }

                _logger.LogInformation("Loading appointment ID: {AppointmentId} for doctor ID: {DoctorId}",
                    AppointmentId, doctorId);

                // Load appointment details
                AppointmentDetails = await _appointmentService.GetByIdAsync(AppointmentId);

                if (AppointmentDetails == null)
                {
                    _logger.LogWarning("Appointment ID: {AppointmentId} not found", AppointmentId);
                    ErrorMessage = "Appointment not found.";
                    return RedirectToPage("TodaysAppointments");
                }

                // Verify that this appointment belongs to the logged-in doctor
                if (AppointmentDetails.DoctorID != doctorId)
                {
                    _logger.LogWarning("Appointment ID: {AppointmentId} does not belong to doctor ID: {DoctorId}",
                        AppointmentId, doctorId);
                    ErrorMessage = "You don't have permission to view this appointment.";
                    return RedirectToPage("TodaysAppointments");
                }

                // Load existing prescription data if available
                Disease = AppointmentDetails.Disease ?? string.Empty;
                Progress = AppointmentDetails.Progress ?? string.Empty;
                Prescription = AppointmentDetails.Prescription ?? string.Empty;

                // Set read-only mode if appointment is completed
                IsReadOnly = AppointmentDetails.AppointmentStatus == 3;

                _logger.LogInformation("Loaded appointment ID: {AppointmentId} successfully", AppointmentId);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading appointment ID: {AppointmentId}", id);
                ErrorMessage = "An error occurred while loading the appointment. Please try again.";
                return RedirectToPage("TodaysAppointments");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(doctorIdClaim) || !int.TryParse(doctorIdClaim, out int doctorId))
                {
                    _logger.LogWarning("Doctor ID not found in claims");
                    ErrorMessage = "Unable to identify doctor. Please log in again.";
                    return RedirectToPage("/Account/Login");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for appointment ID: {AppointmentId}", AppointmentId);

                    // Reload appointment details
                    AppointmentDetails = await _appointmentService.GetByIdAsync(AppointmentId);
                    IsReadOnly = AppointmentDetails?.AppointmentStatus == 3;

                    return Page();
                }

                _logger.LogInformation("Updating prescription for appointment ID: {AppointmentId}", AppointmentId);

                // Update prescription
                var updateResult = await _doctorService.UpdatePrescriptionAsync(
                    AppointmentId, Disease, Progress, Prescription);

                if (!updateResult)
                {
                    ErrorMessage = "Failed to update prescription. Please try again.";
                    _logger.LogWarning("Failed to update prescription for appointment ID: {AppointmentId}", AppointmentId);

                    // Reload appointment details
                    AppointmentDetails = await _appointmentService.GetByIdAsync(AppointmentId);
                    IsReadOnly = AppointmentDetails?.AppointmentStatus == 3;

                    return Page();
                }

                // If marked as completed, complete the appointment
                if (MarkAsCompleted)
                {
                    _logger.LogInformation("Marking appointment ID: {AppointmentId} as completed", AppointmentId);

                    var completeDto = new AppointmentCompleteDto
                    {
                        AppointID = AppointmentId,
                        DoctorID = doctorId,
                        Disease = Disease,
                        Progress = Progress,
                        Prescription = Prescription,
                        IsBillPaid = IsBillPaid
                    };

                    var completeResult = await _doctorService.CompleteAppointmentAsync(completeDto);

                    if (completeResult)
                    {
                        SuccessMessage = "Appointment completed successfully with prescription.";
                        _logger.LogInformation("Appointment ID: {AppointmentId} completed successfully", AppointmentId);
                    }
                    else
                    {
                        SuccessMessage = "Prescription updated, but failed to mark appointment as completed.";
                        _logger.LogWarning("Failed to complete appointment ID: {AppointmentId}", AppointmentId);
                    }
                }
                else
                {
                    SuccessMessage = "Prescription updated successfully.";
                    _logger.LogInformation("Prescription updated successfully for appointment ID: {AppointmentId}",
                        AppointmentId);
                }

                return RedirectToPage("TodaysAppointments");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating prescription for appointment ID: {AppointmentId}", AppointmentId);
                ErrorMessage = "An error occurred while updating the prescription. Please try again.";

                // Reload appointment details
                AppointmentDetails = await _appointmentService.GetByIdAsync(AppointmentId);
                IsReadOnly = AppointmentDetails?.AppointmentStatus == 3;

                return Page();
            }
        }
    }
}
