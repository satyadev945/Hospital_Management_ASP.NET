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
    public class IndexModel : PageModel
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            ILogger<IndexModel> logger)
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
            _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public DoctorDto? DoctorInfo { get; set; }
        public List<AppointmentDto> RecentAppointments { get; set; } = new List<AppointmentDto>();
        public int PendingAppointmentsCount { get; set; }
        public int TodaysAppointmentsCount { get; set; }

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

                _logger.LogInformation("Loading dashboard for doctor ID: {DoctorId}", doctorId);

                // Load doctor information
                DoctorInfo = await LoadDoctorInformationAsync(doctorId);
                if (DoctorInfo == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found", doctorId);
                    ErrorMessage = "Doctor information not found.";
                    return RedirectToPage("/Account/Login");
                }

                // Load pending appointments count
                PendingAppointmentsCount = await LoadPendingAppointmentsCountAsync(doctorId);

                // Load today's appointments count
                TodaysAppointmentsCount = await LoadTodaysAppointmentsCountAsync(doctorId);

                // Load recent appointments
                RecentAppointments = await LoadRecentAppointmentsAsync(doctorId);

                _logger.LogInformation("Dashboard loaded successfully for doctor ID: {DoctorId}", doctorId);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading doctor dashboard");
                ErrorMessage = "An error occurred while loading the dashboard. Please try again.";
                return Page();
            }
        }

        private async Task<DoctorDto?> LoadDoctorInformationAsync(int doctorId)
        {
            try
            {
                return await _doctorService.GetByIdAsync(doctorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading doctor information for ID: {DoctorId}", doctorId);
                return null;
            }
        }

        private async Task<int> LoadPendingAppointmentsCountAsync(int doctorId)
        {
            try
            {
                var pendingAppointments = await _doctorService.GetPendingAppointmentsAsync(doctorId);
                return pendingAppointments?.Count() ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading pending appointments count for doctor ID: {DoctorId}", doctorId);
                return 0;
            }
        }

        private async Task<int> LoadTodaysAppointmentsCountAsync(int doctorId)
        {
            try
            {
                var todaysAppointments = await _doctorService.GetTodaysAppointmentsAsync(doctorId);
                return todaysAppointments?.Count() ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading today's appointments count for doctor ID: {DoctorId}", doctorId);
                return 0;
            }
        }

        private async Task<List<AppointmentDto>> LoadRecentAppointmentsAsync(int doctorId)
        {
            try
            {
                var allAppointments = await _appointmentService.GetByDoctorIdAsync(doctorId);

                if (allAppointments == null || !allAppointments.Any())
                {
                    return new List<AppointmentDto>();
                }

                // Convert to list and sort by date descending, take top 10
                var recentAppointments = allAppointments
                    .OrderByDescending(a => a.Date ?? DateTime.MinValue)
                    .Take(10)
                    .ToList();

                return recentAppointments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading recent appointments for doctor ID: {DoctorId}", doctorId);
                return new List<AppointmentDto>();
            }
        }
    }
}
