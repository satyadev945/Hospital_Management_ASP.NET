using System.Threading.Tasks;
using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services
{
    public interface IDashboardService
    {
        /// <summary>
        /// Gets dashboard statistics for admin
        /// </summary>
        Task<DashboardStatsDto> GetDashboardStatsAsync();

        /// <summary>
        /// Gets total count of patients
        /// </summary>
        Task<int> GetTotalPatientsAsync();

        /// <summary>
        /// Gets total count of active doctors
        /// </summary>
        Task<int> GetTotalDoctorsAsync();

        /// <summary>
        /// Gets current month's income from paid bills
        /// </summary>
        Task<decimal> GetMonthlyIncomeAsync();

        /// <summary>
        /// Gets total count of appointments for current month
        /// </summary>
        Task<int> GetMonthlyAppointmentCountAsync();

        /// <summary>
        /// Gets total count of departments
        /// </summary>
        Task<int> GetTotalDepartmentsAsync();

        /// <summary>
        /// Gets total count of staff members
        /// </summary>
        Task<int> GetTotalStaffAsync();
    }
}
