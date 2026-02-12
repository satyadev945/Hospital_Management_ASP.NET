using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services
{
    public interface IStaffService
    {
        /// <summary>
        /// Gets a staff member by ID
        /// </summary>
        Task<StaffDto> GetByIdAsync(int staffId);

        /// <summary>
        /// Gets all staff members
        /// </summary>
        Task<IEnumerable<StaffDto>> GetAllAsync();

        /// <summary>
        /// Gets staff members by designation
        /// </summary>
        Task<IEnumerable<StaffDto>> GetByDesignationAsync(string designation);

        /// <summary>
        /// Creates a new staff member
        /// </summary>
        Task<StaffDto> CreateAsync(StaffCreateDto staffCreateDto);

        /// <summary>
        /// Updates an existing staff member
        /// </summary>
        Task<StaffDto> UpdateAsync(StaffUpdateDto staffUpdateDto);

        /// <summary>
        /// Deletes a staff member by ID
        /// </summary>
        Task<bool> DeleteAsync(int staffId);

        /// <summary>
        /// Gets total count of staff members
        /// </summary>
        Task<int> GetTotalCountAsync();

        /// <summary>
        /// Checks if staff exists by phone number
        /// </summary>
        Task<bool> ExistsByPhoneAsync(string phone);
    }
}
