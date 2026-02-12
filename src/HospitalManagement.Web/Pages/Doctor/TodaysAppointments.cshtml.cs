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
    public class TodaysAppointmentsModel : PageModel
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger<TodaysAppointmentsModel> _logger;

        public TodaysAppointmentsModel(
            IDoctorService doctorService,
            ILogger<TodaysAppointmentsModel> logger)
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<TodaysAppointmentViewModel> TodaysAppointments { get; set; } = new List<TodaysAppointmentViewModel>();

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

                _logger.LogInformation("Loading today's appointments for doctor ID: {DoctorId}", doctorId);

                // Load today's appointments
                var todaysAppointmentsDto = await _doctorService.GetTodaysAppointmentsAsync(doctorId);

                // Manual mapping from DTO to ViewModel
                TodaysAppointments = MapToViewModelList(todaysAppointmentsDto);

                _logger.LogInformation("Loaded {Count} today's appointments for doctor ID: {DoctorId}",
                    TodaysAppointments.Count, doctorId);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading today's appointments");
                ErrorMessage = "An error occurred while loading today's appointments. Please try again.";
                return Page();
            }
        }

        private List<TodaysAppointmentViewModel> MapToViewModelList(IEnumerable<TodaysAppointmentDto> dtos)
        {
            if (dtos == null)
            {
                return new List<TodaysAppointmentViewModel>();
            }

            var viewModels = new List<TodaysAppointmentViewModel>();

            foreach (var dto in dtos)
            {
                // Parse date
                DateTime appointmentDate;
                if (!DateTime.TryParse(dto.Date.ToString(), out appointmentDate))
                {
                    appointmentDate = DateTime.MinValue;
                }

                viewModels.Add(new TodaysAppointmentViewModel
                {
                    AppointID = dto.AppointID,
                    PatientName = dto.PatientName ?? "Unknown Patient",
                    Date = appointmentDate,
                    BillAmount = dto.BillAmount ?? "0",
                    BillStatus = dto.BillStatus ?? "Unpaid",
                    Disease = dto.Disease ?? string.Empty,
                    Progress = dto.Progress ?? string.Empty,
                    Prescription = dto.Prescription ?? string.Empty,
                    AppointmentStatus = dto.AppointmentStatus
                });
            }

            // Sort by date/time ascending
            return viewModels.OrderBy(vm => vm.Date).ToList();
        }

        // ViewModel for manual mapping
        public class TodaysAppointmentViewModel
        {
            public int AppointID { get; set; }
            public string PatientName { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public string BillAmount { get; set; } = string.Empty;
            public string BillStatus { get; set; } = string.Empty;
            public string Disease { get; set; } = string.Empty;
            public string Progress { get; set; } = string.Empty;
            public string Prescription { get; set; } = string.Empty;
            public int AppointmentStatus { get; set; }
        }
    }
}
