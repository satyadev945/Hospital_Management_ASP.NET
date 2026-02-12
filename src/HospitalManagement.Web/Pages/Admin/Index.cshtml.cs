using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;

namespace HospitalManagement.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IOtherStaffRepository _staffRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IOtherStaffRepository staffRepository,
            IDepartmentRepository departmentRepository,
            IAppointmentRepository appointmentRepository,
            ILogger<IndexModel> logger)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _staffRepository = staffRepository;
            _departmentRepository = departmentRepository;
            _appointmentRepository = appointmentRepository;
            _logger = logger;
        }

        public int TotalPatients { get; set; }
        public int NewPatientsThisMonth { get; set; }
        public int TotalDoctors { get; set; }
        public int ActiveDoctors { get; set; }
        public int TotalStaff { get; set; }
        public int TotalDepartments { get; set; }
        public int ActiveDepartments { get; set; }
        public int TodayAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public decimal TotalRevenue { get; set; }

        public List<Appointment> RecentAppointments { get; set; } = new();
        public List<Department> Departments { get; set; } = new();

        public string? SuccessMessage { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // Get all data
                var allPatients = (await _patientRepository.GetAllAsync()).ToList();
                var allDoctors = (await _doctorRepository.GetAllAsync()).ToList();
                var allStaff = (await _staffRepository.GetAllAsync()).ToList();
                var allDepartments = (await _departmentRepository.GetAllAsync()).ToList();
                var allAppointments = (await _appointmentRepository.GetAllAsync()).ToList();

                // Calculate statistics
                TotalPatients = allPatients.Count;
                NewPatientsThisMonth = allPatients.Count(p =>
                    p.CreatedDate.Month == DateTime.Now.Month &&
                    p.CreatedDate.Year == DateTime.Now.Year);

                TotalDoctors = allDoctors.Count;
                ActiveDoctors = allDoctors.Count(d => d.Status == 1 && d.IsActive);

                TotalStaff = allStaff.Count;

                TotalDepartments = allDepartments.Count;
                ActiveDepartments = allDepartments.Count(d => d.IsActive);

                // Appointment statistics
                var today = DateTime.Today;
                TodayAppointments = allAppointments.Count(a =>
                    a.Date.HasValue && a.Date.Value.Date == today);

                PendingAppointments = allAppointments.Count(a => a.AppointmentStatus == 2);
                CompletedAppointments = allAppointments.Count(a =>
                    a.AppointmentStatus == 3 &&
                    a.Date.HasValue &&
                    a.Date.Value.Date == today);

                // Calculate total revenue (sum of completed appointments with paid bills)
                TotalRevenue = allAppointments
                    .Where(a => a.AppointmentStatus == 3 && a.BillStatus == "Paid" && a.BillAmount.HasValue)
                    .Sum(a => a.BillAmount.Value);

                // Get recent appointments (last 10)
                RecentAppointments = allAppointments
                    .OrderByDescending(a => a.Date)
                    .Take(10)
                    .ToList();

                // Get departments
                Departments = allDepartments.OrderBy(d => d.DeptName).ToList();

                _logger.LogInformation("Admin dashboard loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin dashboard");
            }
        }
    }
}
