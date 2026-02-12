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
    public class PatientHistoryModel : PageModel
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger<PatientHistoryModel> _logger;

        public PatientHistoryModel(
            IDoctorService doctorService,
            ILogger<PatientHistoryModel> logger)
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<TreatmentHistoryViewModel> PatientHistory { get; set; } = new List<TreatmentHistoryViewModel>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }

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

                _logger.LogInformation("Loading patient history for doctor ID: {DoctorId}", doctorId);

                // Load patient treatment history
                var treatmentHistoryDto = await _doctorService.GetPatientHistoryAsync(doctorId);

                // Manual mapping from DTO to ViewModel
                PatientHistory = MapToViewModelList(treatmentHistoryDto);

                // Apply filters if provided
                PatientHistory = ApplyFilters(PatientHistory);

                _logger.LogInformation("Loaded {Count} patient history records for doctor ID: {DoctorId}",
                    PatientHistory.Count, doctorId);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading patient history");
                ErrorMessage = "An error occurred while loading patient history. Please try again.";
                return Page();
            }
        }

        private List<TreatmentHistoryViewModel> MapToViewModelList(IEnumerable<TreatmentHistoryDto> dtos)
        {
            if (dtos == null)
            {
                return new List<TreatmentHistoryViewModel>();
            }

            var viewModels = new List<TreatmentHistoryViewModel>();

            foreach (var dto in dtos)
            {
                viewModels.Add(new TreatmentHistoryViewModel
                {
                    Date = dto.Date ?? "N/A",
                    DoctorName = dto.DoctorName ?? "Unknown Doctor",
                    DiagnosedDisease = dto.DiagnosedDisease ?? string.Empty,
                    ProgressMade = dto.ProgressMade ?? string.Empty,
                    Prescription = dto.Prescription ?? string.Empty
                });
            }

            return viewModels;
        }

        private List<TreatmentHistoryViewModel> ApplyFilters(List<TreatmentHistoryViewModel> records)
        {
            var filteredRecords = records;

            // Apply search term filter
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                filteredRecords = filteredRecords.Where(r =>
                    r.DoctorName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(r.DiagnosedDisease) &&
                     r.DiagnosedDisease.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(r.Prescription) &&
                     r.Prescription.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // Apply date range filters
            if (FromDate.HasValue || ToDate.HasValue)
            {
                filteredRecords = filteredRecords.Where(r =>
                {
                    if (DateTime.TryParse(r.Date, out DateTime recordDate))
                    {
                        bool matchesFrom = !FromDate.HasValue || recordDate.Date >= FromDate.Value.Date;
                        bool matchesTo = !ToDate.HasValue || recordDate.Date <= ToDate.Value.Date;
                        return matchesFrom && matchesTo;
                    }
                    return false;
                }).ToList();
            }

            // Sort by date descending (most recent first)
            filteredRecords = filteredRecords.OrderByDescending(r =>
            {
                if (DateTime.TryParse(r.Date, out DateTime recordDate))
                {
                    return recordDate;
                }
                return DateTime.MinValue;
            }).ToList();

            return filteredRecords;
        }

        // ViewModel for manual mapping
        public class TreatmentHistoryViewModel
        {
            public string Date { get; set; } = string.Empty;
            public string DoctorName { get; set; } = string.Empty;
            public string DiagnosedDisease { get; set; } = string.Empty;
            public string ProgressMade { get; set; } = string.Empty;
            public string Prescription { get; set; } = string.Empty;
        }
    }
}
