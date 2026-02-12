using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Patient entity operations
    /// </summary>
    public interface IPatientRepository
    {
        /// <summary>
        /// Retrieves all patients from the database
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of all patients</returns>
        Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a patient by their unique identifier
        /// </summary>
        /// <param name="id">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Patient entity if found, null otherwise</returns>
        Task<Patient> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new patient to the database
        /// </summary>
        /// <param name="patient">Patient entity to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The added patient with generated ID</returns>
        Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing patient in the database
        /// </summary>
        /// <param name="patient">Patient entity with updated information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated patient entity</returns>
        Task<Patient> UpdateAsync(Patient patient, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a patient from the database
        /// </summary>
        /// <param name="id">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a patient exists in the database
        /// </summary>
        /// <param name="id">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if patient exists, false otherwise</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for patients by name or phone number
        /// </summary>
        /// <param name="searchTerm">Search term for name or phone</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of matching patients</returns>
        Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves patient information with calculated age
        /// </summary>
        /// <param name="id">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Patient with age information</returns>
        Task<Patient> GetPatientWithAgeAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves patients by gender
        /// </summary>
        /// <param name="gender">Gender filter (M/F)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of patients matching gender</returns>
        Task<IEnumerable<Patient>> GetByGenderAsync(char gender, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves patients within a specific age range
        /// </summary>
        /// <param name="minAge">Minimum age</param>
        /// <param name="maxAge">Maximum age</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of patients within age range</returns>
        Task<IEnumerable<Patient>> GetByAgeRangeAsync(int minAge, int maxAge, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the total count of patients
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Total number of patients</returns>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves patients with pending appointments
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of patients with pending appointments</returns>
        Task<IEnumerable<Patient>> GetPatientsWithPendingAppointmentsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves patient by email address
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Patient entity if found, null otherwise</returns>
        Task<Patient> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves patient by phone number
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Patient entity if found, null otherwise</returns>
        Task<Patient> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    }
}
