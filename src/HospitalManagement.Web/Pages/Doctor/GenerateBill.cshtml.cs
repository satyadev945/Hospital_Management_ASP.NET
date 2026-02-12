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
    public class GenerateBillModel : PageModel
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<GenerateBillModel> _logger;

        public GenerateBillModel(
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            ILogger<GenerateBillModel> logger)
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
            _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [BindProperty]
        public int AppointmentId { get; set; }

        [BindProperty]
        public decimal? ConsultationFee { get; set; }

        [BindProperty]
        public decimal MedicationCharges { get; set; }

        [BindProperty]
        public decimal LabCharges { get; set; }

        [BindProperty]
        public decimal OtherCharges { get; set; }

        [BindProperty]
        public decimal DiscountPercentage { get; set; }

        [BindProperty]
        public decimal TaxPercentage { get; set; } = 8.5m; // Default tax rate

        [BindProperty]
        [Required(ErrorMessage = "Payment method is required")]
        public string PaymentMethod { get; set; } = string.Empty;

        [BindProperty]
        [StringLength(100)]
        public string? TransactionReference { get; set; }

        [BindProperty]
        [StringLength(500)]
        public string? Notes { get; set; }

        public AppointmentDto? AppointmentDetails { get; set; }
        public DoctorDto? DoctorInfo { get; set; }

        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(
            int? id,
            decimal medicationCharges = 0,
            decimal labCharges = 0,
            decimal otherCharges = 0,
            decimal discountPercentage = 0,
            decimal taxPercentage = 8.5m)
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

                _logger.LogInformation("Loading bill for appointment ID: {AppointmentId}", AppointmentId);

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
                    ErrorMessage = "You don't have permission to generate bill for this appointment.";
                    return RedirectToPage("TodaysAppointments");
                }

                // Check if appointment is completed
                if (AppointmentDetails.AppointmentStatus != 3)
                {
                    _logger.LogWarning("Appointment ID: {AppointmentId} is not completed", AppointmentId);
                    ErrorMessage = "Bill can only be generated for completed appointments.";
                    return RedirectToPage("TodaysAppointments");
                }

                // Load doctor information
                DoctorInfo = await _doctorService.GetByIdAsync(doctorId);

                // Set charges
                ConsultationFee = DoctorInfo?.ChargesPerVisit ?? 0;
                MedicationCharges = medicationCharges;
                LabCharges = labCharges;
                OtherCharges = otherCharges;
                DiscountPercentage = discountPercentage;
                TaxPercentage = taxPercentage;

                // Calculate bill amounts
                CalculateBillAmounts();

                _logger.LogInformation("Bill loaded successfully for appointment ID: {AppointmentId}", AppointmentId);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading bill for appointment ID: {AppointmentId}", id);
                ErrorMessage = "An error occurred while loading the bill. Please try again.";
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

                // Validate payment method
                if (string.IsNullOrWhiteSpace(PaymentMethod))
                {
                    ErrorMessage = "Payment method is required.";

                    // Reload data
                    AppointmentDetails = await _appointmentService.GetByIdAsync(AppointmentId);
                    DoctorInfo = await _doctorService.GetByIdAsync(doctorId);
                    CalculateBillAmounts();

                    return Page();
                }

                _logger.LogInformation("Marking bill as paid for appointment ID: {AppointmentId}", AppointmentId);

                // Load appointment to update bill status
                var appointment = await _appointmentService.GetByIdAsync(AppointmentId);

                if (appointment == null)
                {
                    ErrorMessage = "Appointment not found.";
                    return RedirectToPage("TodaysAppointments");
                }

                // Update appointment with bill status
                var updateDto = new AppointmentUpdateDto
                {
                    AppointID = AppointmentId,
                    DoctorID = appointment.DoctorID,
                    PatientID = appointment.PatientID,
                    Date = appointment.Date,
                    AppointmentStatus = appointment.AppointmentStatus,
                    BillAmount = TotalAmount,
                    BillStatus = "Paid",
                    DoctorNotification = appointment.DoctorNotification,
                    PatientNotification = appointment.PatientNotification,
                    FeedbackStatus = appointment.FeedbackStatus,
                    Disease = appointment.Disease,
                    Progress = appointment.Progress,
                    Prescription = appointment.Prescription
                };

                var result = await _appointmentService.UpdateAsync(updateDto);

                if (result != null)
                {
                    SuccessMessage = $"Bill of ${TotalAmount:F2} has been marked as paid successfully using {PaymentMethod}.";
                    _logger.LogInformation("Bill marked as paid for appointment ID: {AppointmentId}", AppointmentId);

                    return RedirectToPage("TodaysAppointments");
                }
                else
                {
                    ErrorMessage = "Failed to update bill status. Please try again.";
                    _logger.LogWarning("Failed to mark bill as paid for appointment ID: {AppointmentId}", AppointmentId);

                    // Reload data
                    AppointmentDetails = await _appointmentService.GetByIdAsync(AppointmentId);
                    DoctorInfo = await _doctorService.GetByIdAsync(doctorId);
                    CalculateBillAmounts();

                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing bill payment for appointment ID: {AppointmentId}", AppointmentId);
                ErrorMessage = "An error occurred while processing the payment. Please try again.";

                // Reload data
                try
                {
                    var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (int.TryParse(doctorIdClaim, out int doctorId))
                    {
                        AppointmentDetails = await _appointmentService.GetByIdAsync(AppointmentId);
                        DoctorInfo = await _doctorService.GetByIdAsync(doctorId);
                        CalculateBillAmounts();
                    }
                }
                catch (Exception reloadEx)
                {
                    _logger.LogError(reloadEx, "Error reloading data after payment failure");
                }

                return Page();
            }
        }

        private void CalculateBillAmounts()
        {
            // Calculate subtotal
            Subtotal = (ConsultationFee ?? 0) + MedicationCharges + LabCharges + OtherCharges;

            // Calculate discount
            DiscountAmount = Subtotal * (DiscountPercentage / 100m);

            // Calculate amount after discount
            var amountAfterDiscount = Subtotal - DiscountAmount;

            // Calculate tax
            TaxAmount = amountAfterDiscount * (TaxPercentage / 100m);

            // Calculate total
            TotalAmount = amountAfterDiscount + TaxAmount;

            // Round to 2 decimal places
            Subtotal = Math.Round(Subtotal, 2);
            DiscountAmount = Math.Round(DiscountAmount, 2);
            TaxAmount = Math.Round(TaxAmount, 2);
            TotalAmount = Math.Round(TotalAmount, 2);
        }
    }
}
