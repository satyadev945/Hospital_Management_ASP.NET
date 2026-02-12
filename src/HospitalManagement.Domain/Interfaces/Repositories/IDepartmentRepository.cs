using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Department entity operations
    /// </summary>
    public interface IDepartmentRepository
    {
        /// <summary>
        /// Retrieves all departments from the database
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of all departments</returns>
        Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a department by its unique identifier
        /// </summary>
        /// <param name="id">Department identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Department entity if found, null otherwise</returns>
        Task<Department> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new department to the database
        /// </summary>
        /// <param name="department">Department entity to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The added department with generated ID</returns>
        Task<Department> AddAsync(Department department, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing department in the database
        /// </summary>
        /// <param name="department">Department entity with updated information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated department entity</returns>
        Task<Department> UpdateAsync(Department department, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a department from the database
        /// </summary>
        /// <param name="id">Department identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a department exists in the database
        /// </summary>
        /// <param name="id">Department identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if department exists, false otherwise</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for departments by name or description
        /// </summary>
        /// <param name="searchTerm">Search term for name or description</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of matching departments</returns>
        Task<IEnumerable<Department>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a department by its name
        /// </summary>
        /// <param name="name">Department name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Department entity if found, null otherwise</returns>
        Task<Department> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a department name already exists
        /// </summary>
        /// <param name="name">Department name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if name exists, false otherwise</returns>
        Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves department with doctor count
        /// </summary>
        /// <param name="id">Department identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Department with doctor count information</returns>
        Task<Department> GetWithDoctorCountAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all departments with their doctor counts
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of departments with doctor counts</returns>
        Task<IEnumerable<Department>> GetAllWithDoctorCountsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves departments that have active doctors
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of departments with active doctors</returns>
        Task<IEnumerable<Department>> GetDepartmentsWithActiveDoctorsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the total count of departments
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Total number of departments</returns>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves department information for patient appointment selection
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of departments with basic information</returns>
        Task<IEnumerable<Department>> GetDepartmentInfoAsync(CancellationToken cancellationToken = default);
    }
}
