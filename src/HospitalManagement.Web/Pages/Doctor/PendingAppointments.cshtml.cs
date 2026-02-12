using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PendingAppointmentsModel : PageModel
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger<PendingAppointmentsModel> _logger;

        public PendingAppointmentsModel(
            IDoctorService doctorService,
            ILogger<PendingAppointmentsModel> logger)
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<PendingAppointmentViewModel> PendingAppointments { get; set; } = new List<PendingAppointmentViewModel>();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
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

                _logger.LogInformation("Loading pending appointments for doctor ID: {DoctorId}", doctorId);

                // Load pending appointments
                var pendingAppointmentsDto = await _doctorService.GetPendingAppointmentsAsync(doctorId);

                // Manual mapping from DTO to ViewModel
                PendingAppointments = MapToViewModelList(pendingAppointmentsDto);

                _logger.LogInformation("Loaded {Count} pending appointments for doctor ID: {DoctorId}",
                    PendingAppointments.Count, doctorId);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading pending appointments");
                ErrorMessage = "An error occurred while loading pending appointments. Please try again.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostApproveAsync(int appointmentId)
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

                _logger.LogInformation("Approving appointment ID: {AppointmentId} by doctor ID: {DoctorId}",
                    appointmentId, doctorId);

                var result = await _doctorService.ApproveAppointmentAsync(appointmentId);

                if (result)
                {
                    SuccessMessage = $"Appointment #{appointmentId} has been approved successfully.";
                    _logger.LogInformation("Appointment ID: {AppointmentId} approved successfully", appointmentId);
                }
                else
                {
                    ErrorMessage = $"Failed to approve appointment #{appointmentId}. Please try again.";
                    _logger.LogWarning("Failed to approve appointment ID: {AppointmentId}", appointmentId);
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving appointment ID: {AppointmentId}", appointmentId);
                ErrorMessage = "An error occurred while approving the appointment. Please try again.";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostRejectAsync(int appointmentId)
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

                _logger.LogInformation("Rejecting appointment ID: {AppointmentId} by doctor ID: {DoctorId}",
                    appointmentId, doctorId);

                var result = await _doctorService.RejectAppointmentAsync(appointmentId);

                if (result)
                {
                    SuccessMessage = $"Appointment #{appointmentId} has been rejected.";
                    _logger.LogInformation("Appointment ID: {AppointmentId} rejected successfully", appointmentId);
                }
                else
                {
                    ErrorMessage = $"Failed to reject appointment #{appointmentId}. Please try again.";
                    _logger.LogWarning("Failed to reject appointment ID: {AppointmentId}", appointmentId);
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting appointment ID: {AppointmentId}", appointmentId);
                ErrorMessage = "An error occurred while rejecting the appointment. Please try again.";
                return RedirectToPage();
            }
        }

        private List<PendingAppointmentViewModel> MapToViewModelList(IEnumerable<PendingAppointmentDto> dtos)
        {
            if (dtos == null)
            {
                return new List<PendingAppointmentViewModel>();
            }

            var viewModels = new List<PendingAppointmentViewModel>();

            foreach (var dto in dtos)
            {
                viewModels.Add(new PendingAppointmentViewModel
                {
                    AppointID = dto.AppointID,
                    PatientName = dto.PatientName ?? "Unknown Patient",
                    Date = dto.Date,
                    AppointmentStatus = dto.AppointmentStatus
                });
            }

            // Sort by date ascending
            return viewModels.OrderBy(vm => vm.Date).ToList();
        }

        // ViewModel for manual mapping
        public class PendingAppointmentViewModel
        {
            public int AppointID { get; set; }
            public string PatientName { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public int AppointmentStatus { get; set; }
        }
    }
}
