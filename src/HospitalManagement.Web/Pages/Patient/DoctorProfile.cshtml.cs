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
    public class DoctorProfileModel : PageModel
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger<DoctorProfileModel> _logger;

        public DoctorProfileModel(
            IDoctorService doctorService,
            ILogger<DoctorProfileModel> logger)
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public DoctorViewModel Doctor { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid doctor ID: {DoctorId}", id);
                    ErrorMessage = "Invalid doctor ID.";
                    return Page();
                }

                var doctorDto = await _doctorService.GetByIdAsync(id);

                if (doctorDto == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found", id);
                    ErrorMessage = "Doctor not found.";
                    return Page();
                }

                // Check if doctor is active
                if (doctorDto.Status != 1)
                {
                    _logger.LogWarning("Attempted to view inactive doctor profile: {DoctorId}", id);
                    ErrorMessage = "This doctor is currently not available.";
                    return Page();
                }

                Doctor = MapToDoctorViewModel(doctorDto);
                _logger.LogInformation("Loaded profile for doctor {DoctorId}", id);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading doctor profile for ID {DoctorId}", id);
                ErrorMessage = "An error occurred while loading the doctor profile. Please try again.";
                return Page();
            }
        }

        // Manual mapping method
        private DoctorViewModel MapToDoctorViewModel(DoctorDto dto)
        {
            return new DoctorViewModel
            {
                DoctorID = dto.DoctorID,
                Name = dto.Name,
                Phone = dto.Phone,
                Address = dto.Address,
                BirthDate = dto.BirthDate,
                Age = dto.Age,
                Gender = dto.Gender,
                DeptNo = dto.DeptNo,
                DepartmentName = dto.DepartmentName,
                ChargesPerVisit = dto.ChargesPerVisit,
                MonthlySalary = dto.MonthlySalary,
                ReputeIndex = dto.ReputeIndex,
                PatientsTreated = dto.PatientsTreated,
                Qualification = dto.Qualification,
                Specialization = dto.Specialization,
                WorkExperience = dto.WorkExperience,
                Status = dto.Status,
                Email = dto.Email
            };
        }

        // View Model
        public class DoctorViewModel
        {
            public int DoctorID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public DateTime BirthDate { get; set; }
            public int Age { get; set; }
            public char Gender { get; set; }
            public int DeptNo { get; set; }
            public string DepartmentName { get; set; }
            public decimal ChargesPerVisit { get; set; }
            public decimal? MonthlySalary { get; set; }
            public decimal? ReputeIndex { get; set; }
            public int PatientsTreated { get; set; }
            public string Qualification { get; set; }
            public string Specialization { get; set; }
            public int? WorkExperience { get; set; }
            public int Status { get; set; }
            public string Email { get; set; }
        }
    }
}
