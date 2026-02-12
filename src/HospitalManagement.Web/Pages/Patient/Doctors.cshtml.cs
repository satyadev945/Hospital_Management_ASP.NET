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
    public class DoctorsModel : PageModel
    {
        private readonly IDoctorService _doctorService;
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DoctorsModel> _logger;

        public DoctorsModel(
            IDoctorService doctorService,
            IDepartmentService departmentService,
            ILogger<DoctorsModel> logger)
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<DoctorViewModel> Doctors { get; set; }
        public IEnumerable<DepartmentViewModel> Departments { get; set; }
        public string SelectedDepartment { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string department)
        {
            try
            {
                SelectedDepartment = department;

                // Load all departments for filter
                var departmentsDto = await _departmentService.GetDepartmentInfoAsync();
                if (departmentsDto != null)
                {
                    Departments = departmentsDto.Select(MapToDepartmentViewModel).ToList();
                }

                // Load doctors based on department filter
                IEnumerable<DoctorDto> doctorsDto;

                if (!string.IsNullOrEmpty(department))
                {
                    _logger.LogInformation("Loading doctors for department: {Department}", department);
                    doctorsDto = await _doctorService.GetByDepartmentAsync(department);
                }
                else
                {
                    _logger.LogInformation("Loading all active doctors");
                    doctorsDto = await _doctorService.GetAllActiveAsync();
                }

                if (doctorsDto != null && doctorsDto.Any())
                {
                    Doctors = doctorsDto.Select(MapToDoctorViewModel).ToList();
                    _logger.LogInformation("Loaded {Count} doctors", Doctors.Count());
                }
                else
                {
                    Doctors = new List<DoctorViewModel>();
                    _logger.LogInformation("No doctors found");
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading doctors");
                ErrorMessage = "An error occurred while loading doctors. Please try again.";
                Doctors = new List<DoctorViewModel>();
                Departments = new List<DepartmentViewModel>();
                return Page();
            }
        }

        // Manual mapping methods
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

        // View Models
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

        public class DepartmentViewModel
        {
            public int DeptNo { get; set; }
            public string DeptName { get; set; }
            public string Description { get; set; }
            public int NumberOfDoctors { get; set; }
        }
    }
}
