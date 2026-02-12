using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Doctor entity operations
    /// </summary>
    public interface IDoctorRepository
    {
        /// <summary>
        /// Retrieves all doctors from the database
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of all doctors</returns>
        Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a doctor by their unique identifier
        /// </summary>
        /// <param name="id">Doctor identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Doctor entity if found, null otherwise</returns>
        Task<Doctor> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new doctor to the database
        /// </summary>
        /// <param name="doctor">Doctor entity to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The added doctor with generated ID</returns>
        Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing doctor in the database
        /// </summary>
        /// <param name="doctor">Doctor entity with updated information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated doctor entity</returns>
        Task<Doctor> UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a doctor from the database (soft delete by updating status)
        /// </summary>
        /// <param name="id">Doctor identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a doctor exists in the database
        /// </summary>
        /// <param name="id">Doctor identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if doctor exists, false otherwise</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for doctors by name or specialization
        /// </summary>
        /// <param name="searchTerm">Search term for name or specialization</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of matching doctors</returns>
        Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves active doctors (status = 1)
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of active doctors</returns>
        Task<IEnumerable<Doctor>> GetActiveDoctorsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves doctors by department
        /// </summary>
        /// <param name="departmentId">Department identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of doctors in the specified department</returns>
        Task<IEnumerable<Doctor>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves doctors by department name
        /// </summary>
        /// <param name="departmentName">Department name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of doctors in the specified department</returns>
        Task<IEnumerable<Doctor>> GetByDepartmentNameAsync(string departmentName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves doctors by specialization
        /// </summary>
        /// <param name="specialization">Specialization</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of doctors with the specified specialization</returns>
        Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves doctor profile with complete information including reputation index
        /// </summary>
        /// <param name="id">Doctor identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Doctor with complete profile information</returns>
        Task<Doctor> GetDoctorProfileAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if doctor email already exists
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if email exists, false otherwise</returns>
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves doctor by email address
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Doctor entity if found, null otherwise</returns>
        Task<Doctor> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves doctors with experience greater than specified years
        /// </summary>
        /// <param name="years">Minimum years of experience</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of experienced doctors</returns>
        Task<IEnumerable<Doctor>> GetByMinimumExperienceAsync(int years, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves doctors ordered by reputation index
        /// </summary>
        /// <param name="limit">Maximum number of doctors to return</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of top-rated doctors</returns>
        Task<IEnumerable<Doctor>> GetTopRatedDoctorsAsync(int limit, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves doctors by gender
        /// </summary>
        /// <param name="gender">Gender filter (M/F)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of doctors matching gender</returns>
        Task<IEnumerable<Doctor>> GetByGenderAsync(char gender, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates doctor reputation index
        /// </summary>
        /// <param name="id">Doctor identifier</param>
        /// <param name="reputeIndex">New reputation index value</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdateReputeIndexAsync(int id, float reputeIndex, CancellationToken cancellationToken = default);

        /// <summary>
        /// Increments patients treated count for a doctor
        /// </summary>
        /// <param name="id">Doctor identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> IncrementPatientsTreatedAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves total count of active doctors
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Total number of active doctors</returns>
        Task<int> GetTotalActiveDoctorsCountAsync(CancellationToken cancellationToken = default);
    }
}
