using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for OtherStaff entity operations
    /// </summary>
    public interface IOtherStaffRepository
    {
        /// <summary>
        /// Retrieves all staff members from the database
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of all staff members</returns>
        Task<IEnumerable<OtherStaff>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a staff member by their unique identifier
        /// </summary>
        /// <param name="id">Staff identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>OtherStaff entity if found, null otherwise</returns>
        Task<OtherStaff> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new staff member to the database
        /// </summary>
        /// <param name="staff">OtherStaff entity to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The added staff member with generated ID</returns>
        Task<OtherStaff> AddAsync(OtherStaff staff, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing staff member in the database
        /// </summary>
        /// <param name="staff">OtherStaff entity with updated information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated staff member entity</returns>
        Task<OtherStaff> UpdateAsync(OtherStaff staff, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a staff member from the database
        /// </summary>
        /// <param name="id">Staff identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a staff member exists in the database
        /// </summary>
        /// <param name="id">Staff identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if staff member exists, false otherwise</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for staff members by name or designation
        /// </summary>
        /// <param name="searchTerm">Search term for name or designation</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of matching staff members</returns>
        Task<IEnumerable<OtherStaff>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves staff members by designation
        /// </summary>
        /// <param name="designation">Staff designation</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of staff members with the specified designation</returns>
        Task<IEnumerable<OtherStaff>> GetByDesignationAsync(string designation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves staff members by gender
        /// </summary>
        /// <param name="gender">Gender filter (M/F)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of staff members matching gender</returns>
        Task<IEnumerable<OtherStaff>> GetByGenderAsync(char gender, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves staff members by phone number
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>OtherStaff entity if found, null otherwise</returns>
        Task<OtherStaff> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves staff members with salary greater than specified amount
        /// </summary>
        /// <param name="minSalary">Minimum salary</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of staff members with salary above minimum</returns>
        Task<IEnumerable<OtherStaff>> GetBySalaryRangeAsync(float minSalary, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves staff members within a salary range
        /// </summary>
        /// <param name="minSalary">Minimum salary</param>
        /// <param name="maxSalary">Maximum salary</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of staff members within salary range</returns>
        Task<IEnumerable<OtherStaff>> GetBySalaryRangeAsync(float minSalary, float maxSalary, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates staff member salary
        /// </summary>
        /// <param name="id">Staff identifier</param>
        /// <param name="newSalary">New salary amount</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdateSalaryAsync(int id, float newSalary, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves total count of staff members
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Total number of staff members</returns>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves count of staff members by designation
        /// </summary>
        /// <param name="designation">Staff designation</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Count of staff members with specified designation</returns>
        Task<int> GetCountByDesignationAsync(string designation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all unique designations
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of unique designations</returns>
        Task<IEnumerable<string>> GetAllDesignationsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves staff member profile with complete information
        /// </summary>
        /// <param name="id">Staff identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>OtherStaff with complete profile information</returns>
        Task<OtherStaff> GetStaffProfileAsync(int id, CancellationToken cancellationToken = default);
    }
}
