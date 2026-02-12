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
    public class BillHistoryModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<BillHistoryModel> _logger;

        public BillHistoryModel(
            IPatientService patientService,
            ILogger<BillHistoryModel> logger)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<BillHistoryViewModel> BillHistory { get; set; }
        public int TotalBills { get; set; }
        public decimal TotalAmount { get; set; }
        public int UnpaidCount { get; set; }
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

                // Load bill history
                var billHistoryDto = await _patientService.GetBillHistoryAsync(patientId);

                if (billHistoryDto != null && billHistoryDto.Any())
                {
                    BillHistory = billHistoryDto.Select(MapToBillHistoryViewModel).ToList();

                    // Calculate statistics
                    TotalBills = BillHistory.Count();
                    TotalAmount = BillHistory.Sum(b => b.BillAmount);
                    UnpaidCount = BillHistory.Count(b => b.BillStatus == "Unpaid");

                    _logger.LogInformation("Loaded {Count} bills for patient {PatientId}", TotalBills, patientId);
                }
                else
                {
                    BillHistory = new List<BillHistoryViewModel>();
                    _logger.LogInformation("No bill history found for patient {PatientId}", patientId);
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading bill history");
                ErrorMessage = "An error occurred while loading your bill history. Please try again.";
                BillHistory = new List<BillHistoryViewModel>();
                return Page();
            }
        }

        // Manual mapping method
        private BillHistoryViewModel MapToBillHistoryViewModel(BillHistoryDto dto)
        {
            return new BillHistoryViewModel
            {
                Date = dto.Date,
                DoctorName = dto.DoctorName,
                BillAmount = dto.BillAmount,
                BillStatus = dto.BillStatus
            };
        }

        // View Model
        public class BillHistoryViewModel
        {
            public string Date { get; set; }
            public string DoctorName { get; set; }
            public decimal BillAmount { get; set; }
            public string BillStatus { get; set; }
        }
    }
}
