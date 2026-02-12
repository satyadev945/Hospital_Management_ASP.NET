using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Domain.Interfaces.Services;

namespace HospitalManagement.Application.Services
{
    /// <summary>
    /// Service implementation for dashboard statistics and analytics
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IOtherStaffRepository _staffRepository;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IDepartmentRepository departmentRepository,
            IOtherStaffRepository staffRepository,
            ILogger<DashboardService> logger)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving dashboard statistics");

                var totalPatients = await GetTotalPatientsAsync();
                var totalDoctors = await GetTotalDoctorsAsync();
                var monthlyIncome = await GetMonthlyIncomeAsync();
                var monthlyAppointments = await GetMonthlyAppointmentCountAsync();
                var totalDepartments = await GetTotalDepartmentsAsync();
                var totalStaff = await GetTotalStaffAsync();

                var dashboardStats = new DashboardStatsDto
                {
                    TotalPatients = totalPatients,
                    TotalDoctors = totalDoctors,
                    MonthlyIncome = monthlyIncome,
                    TotalAppointments = monthlyAppointments,
                    TotalDepartments = totalDepartments,
                    TotalStaff = totalStaff
                };

                _logger.LogInformation("Successfully retrieved dashboard statistics - Patients: {Patients}, Doctors: {Doctors}, Income: {Income}",
                    totalPatients, totalDoctors, monthlyIncome);

                return dashboardStats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard statistics");
                throw;
            }
        }

        public async Task<int> GetTotalPatientsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving total patient count");

                var count = await _patientRepository.GetTotalCountAsync(CancellationToken.None);

                _logger.LogInformation("Successfully retrieved total patient count: {Count}", count);

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total patient count");
                throw;
            }
        }

        public async Task<int> GetTotalDoctorsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving total active doctor count");

                var count = await _doctorRepository.GetTotalActiveDoctorsCountAsync(CancellationToken.None);

                _logger.LogInformation("Successfully retrieved total active doctor count: {Count}", count);

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total active doctor count");
                throw;
            }
        }

        public async Task<decimal> GetMonthlyIncomeAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving monthly income from paid bills");

                // Get current month's date range
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                // Get appointments in current month
                var appointments = await _appointmentRepository.GetByDateRangeAsync(
                    startDate, endDate, CancellationToken.None);

                decimal totalIncome = 0;

                foreach (var appointment in appointments)
                {
                    // Only count paid bills
                    if (appointment.BillStatus != null &&
                        appointment.BillStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase))
                    {
                        totalIncome += appointment.BillAmount ?? 0;
                    }
                }

                _logger.LogInformation("Successfully retrieved monthly income: {Income}", totalIncome);

                return totalIncome;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving monthly income");
                throw;
            }
        }

        public async Task<int> GetMonthlyAppointmentCountAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving monthly appointment count");

                // Get current month's date range
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                // Get appointments in current month
                var appointments = await _appointmentRepository.GetByDateRangeAsync(
                    startDate, endDate, CancellationToken.None);

                var count = 0;
                foreach (var _ in appointments)
                {
                    count++;
                }

                _logger.LogInformation("Successfully retrieved monthly appointment count: {Count}", count);

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving monthly appointment count");
                throw;
            }
        }

        public async Task<int> GetTotalDepartmentsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving total department count");

                var count = await _departmentRepository.GetTotalCountAsync(CancellationToken.None);

                _logger.LogInformation("Successfully retrieved total department count: {Count}", count);

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total department count");
                throw;
            }
        }

        public async Task<int> GetTotalStaffAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving total staff count");

                var count = await _staffRepository.GetTotalCountAsync(CancellationToken.None);

                _logger.LogInformation("Successfully retrieved total staff count: {Count}", count);

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total staff count");
                throw;
            }
        }
    }
}
