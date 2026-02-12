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
    public class BookAppointmentModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<BookAppointmentModel> _logger;

        public BookAppointmentModel(
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IDepartmentService departmentService,
            ILogger<BookAppointmentModel> logger)
        {
            _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<DepartmentViewModel> Departments { get; set; }
        public IEnumerable<DoctorViewModel> Doctors { get; set; }
        public DoctorViewModel SelectedDoctor { get; set; }
        public IEnumerable<AppointmentSlotViewModel> AvailableSlots { get; set; }

        public string SelectedDepartment { get; set; }
        public int SelectedDoctorId { get; set; }
        public DateTime? SelectedDate { get; set; }
        public bool HasActiveAppointment { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string department, int? doctorId, DateTime? appointmentDate)
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

                // Check for active appointments
                HasActiveAppointment = await _appointmentService.HasActivAppointmentAsync(patientId);
                if (HasActiveAppointment)
                {
                    _logger.LogInformation("Patient {PatientId} has active appointment", patientId);
                    return Page();
                }

                // Load departments
                var departmentsDto = await _departmentService.GetDepartmentInfoAsync();
                if (departmentsDto != null)
                {
                    Departments = departmentsDto.Select(MapToDepartmentViewModel).ToList();
                }

                // Handle department selection
                SelectedDepartment = department;
                if (!string.IsNullOrEmpty(department))
                {
                    var doctorsDto = await _doctorService.GetByDepartmentAsync(department);
                    if (doctorsDto != null)
                    {
                        Doctors = doctorsDto.Select(MapToDoctorViewModel).ToList();
                    }
                }
                else if (doctorId.HasValue)
                {
                    // If doctor is preselected, load all active doctors
                    var doctorsDto = await _doctorService.GetAllActiveAsync();
                    if (doctorsDto != null)
                    {
                        Doctors = doctorsDto.Select(MapToDoctorViewModel).ToList();
                    }
                }

                // Handle doctor selection
                if (doctorId.HasValue && doctorId.Value > 0)
                {
                    SelectedDoctorId = doctorId.Value;
                    var doctorDto = await _doctorService.GetByIdAsync(doctorId.Value);
                    if (doctorDto != null)
                    {
                        SelectedDoctor = MapToDoctorViewModel(doctorDto);
                        SelectedDepartment = doctorDto.DepartmentName;
                    }
                }

                // Handle date selection and load available slots
                if (appointmentDate.HasValue && SelectedDoctorId > 0)
                {
                    SelectedDate = appointmentDate.Value;

                    // Validate date
                    if (appointmentDate.Value.Date < DateTime.Now.Date)
                    {
                        ErrorMessage = "Cannot book appointments for past dates.";
                        return Page();
                    }

                    if (appointmentDate.Value.Date > DateTime.Now.AddMonths(1).Date)
                    {
                        ErrorMessage = "Cannot book appointments more than 1 month in advance.";
                        return Page();
                    }

                    // Get available slots
                    var slotsDto = await _appointmentService.GetFreeSlotsAsync(SelectedDoctorId, patientId, appointmentDate.Value);
                    if (slotsDto != null && slotsDto.Any())
                    {
                        AvailableSlots = slotsDto.Select(MapToSlotViewModel).ToList();
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading book appointment page");
                ErrorMessage = "An error occurred while loading the page. Please try again.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(int doctorId, DateTime appointmentDate, int selectedSlot)
        {
            try
            {
                // Get patient ID from claims
                var patientIdClaim = User.FindFirst("PatientID")?.Value;
                if (string.IsNullOrEmpty(patientIdClaim) || !int.TryParse(patientIdClaim, out int patientId))
                {
                    _logger.LogWarning("Patient ID not found in claims");
                    ErrorMessage = "Unable to identify patient. Please log in again.";
                    return await OnGetAsync(null, doctorId, appointmentDate);
                }

                // Validate inputs
                if (doctorId <= 0)
                {
                    ErrorMessage = "Please select a doctor.";
                    return await OnGetAsync(null, doctorId, appointmentDate);
                }

                if (appointmentDate.Date < DateTime.Now.Date)
                {
                    ErrorMessage = "Cannot book appointments for past dates.";
                    return await OnGetAsync(null, doctorId, appointmentDate);
                }

                if (selectedSlot < 0 || selectedSlot > 23)
                {
                    ErrorMessage = "Please select a valid time slot.";
                    return await OnGetAsync(null, doctorId, appointmentDate);
                }

                // Check for active appointments again
                var hasActiveAppointment = await _appointmentService.HasActivAppointmentAsync(patientId);
                if (hasActiveAppointment)
                {
                    ErrorMessage = "You already have an active appointment. Please complete or cancel it before booking a new one.";
                    return await OnGetAsync(null, doctorId, appointmentDate);
                }

                // Book the appointment
                var success = await _appointmentService.BookAppointmentAsync(patientId, doctorId, selectedSlot);

                if (success)
                {
                    _logger.LogInformation("Appointment booked successfully for patient {PatientId} with doctor {DoctorId}", patientId, doctorId);
                    TempData["SuccessMessage"] = "Appointment booked successfully! The doctor will review and approve your appointment.";
                    return RedirectToPage("./CurrentAppointment");
                }
                else
                {
                    _logger.LogWarning("Failed to book appointment for patient {PatientId} with doctor {DoctorId}", patientId, doctorId);
                    ErrorMessage = "Failed to book appointment. The selected time slot may no longer be available.";
                    return await OnGetAsync(null, doctorId, appointmentDate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error booking appointment");
                ErrorMessage = "An error occurred while booking the appointment. Please try again.";
                return await OnGetAsync(null, doctorId, appointmentDate);
            }
        }

        // Manual mapping methods
        private DepartmentViewModel MapToDepartmentViewModel(DepartmentDto dto)
        {
            return new DepartmentViewModel
            {
                DeptNo = dto.DeptNo,
                DeptName = dto.DeptName,
                Description = dto.Description,
                NumberOfDoctors = dto.NumberOfDoctors
            };
        }

        private DoctorViewModel MapToDoctorViewModel(DoctorDto dto)
        {
            return new DoctorViewModel
            {
                DoctorID = dto.DoctorID,
                Name = dto.Name,
                Phone = dto.Phone,
                DepartmentName = dto.DepartmentName,
                ChargesPerVisit = dto.ChargesPerVisit,
                Qualification = dto.Qualification,
                Specialization = dto.Specialization,
                WorkExperience = dto.WorkExperience,
                Email = dto.Email
            };
        }

        private AppointmentSlotViewModel MapToSlotViewModel(AppointmentSlotDto dto)
        {
            return new AppointmentSlotViewModel
            {
                FreeSlot = dto.FreeSlot,
                SlotHour = dto.SlotHour
            };
        }

        // View Models
        public class DepartmentViewModel
        {
            public int DeptNo { get; set; }
            public string DeptName { get; set; }
            public string Description { get; set; }
            public int NumberOfDoctors { get; set; }
        }

        public class DoctorViewModel
        {
            public int DoctorID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string DepartmentName { get; set; }
            public decimal ChargesPerVisit { get; set; }
            public string Qualification { get; set; }
            public string Specialization { get; set; }
            public int? WorkExperience { get; set; }
            public string Email { get; set; }
        }

        public class AppointmentSlotViewModel
        {
            public string FreeSlot { get; set; }
            public int SlotHour { get; set; }
        }
    }
}
