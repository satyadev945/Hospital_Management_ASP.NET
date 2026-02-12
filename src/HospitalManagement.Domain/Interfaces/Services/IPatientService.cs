using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services
{
    public interface IPatientService
    {
        /// <summary>
        /// Gets a patient by ID
        /// </summary>
        Task<PatientDto> GetByIdAsync(int patientId);

        /// <summary>
        /// Gets all patients
        /// </summary>
        Task<IEnumerable<PatientDto>> GetAllAsync();

        /// <summary>
        /// Creates a new patient
        /// </summary>
        Task<PatientDto> CreateAsync(PatientCreateDto patientCreateDto);

        /// <summary>
        /// Updates an existing patient
        /// </summary>
        Task<PatientDto> UpdateAsync(PatientUpdateDto patientUpdateDto);

        /// <summary>
        /// Deletes a patient by ID
        /// </summary>
        Task<bool> DeleteAsync(int patientId);

        /// <summary>
        /// Gets patient's bill history
        /// </summary>
        Task<IEnumerable<BillHistoryDto>> GetBillHistoryAsync(int patientId);

        /// <summary>
        /// Gets patient's treatment history
        /// </summary>
        Task<IEnumerable<TreatmentHistoryDto>> GetTreatmentHistoryAsync(int patientId);

        /// <summary>
        /// Gets patient's current appointment
        /// </summary>
        Task<AppointmentDto> GetCurrentAppointmentAsync(int patientId);

        /// <summary>
        /// Gets patient notifications
        /// </summary>
        Task<NotificationDto> GetNotificationsAsync(int patientId);

        /// <summary>
        /// Gets pending feedback for patient
        /// </summary>
        Task<AppointmentDto> GetPendingFeedbackAsync(int patientId);

        /// <summary>
        /// Stores feedback for an appointment
        /// </summary>
        Task<bool> StoreFeedbackAsync(int appointmentId);

        /// <summary>
        /// Checks if patient exists by email
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email);
    }
}
