using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Appointment entity operations
    /// </summary>
    public interface IAppointmentRepository
    {
        /// <summary>
        /// Retrieves all appointments from the database
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of all appointments</returns>
        Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an appointment by its unique identifier
        /// </summary>
        /// <param name="id">Appointment identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Appointment entity if found, null otherwise</returns>
        Task<Appointment> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new appointment to the database
        /// </summary>
        /// <param name="appointment">Appointment entity to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The added appointment with generated ID</returns>
        Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing appointment in the database
        /// </summary>
        /// <param name="appointment">Appointment entity with updated information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated appointment entity</returns>
        Task<Appointment> UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an appointment from the database
        /// </summary>
        /// <param name="id">Appointment identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an appointment exists in the database
        /// </summary>
        /// <param name="id">Appointment identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if appointment exists, false otherwise</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for appointments by patient name or doctor name
        /// </summary>
        /// <param name="searchTerm">Search term for patient or doctor name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of matching appointments</returns>
        Task<IEnumerable<Appointment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves appointments by patient ID
        /// </summary>
        /// <param name="patientId">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of appointments for the patient</returns>
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves appointments by doctor ID
        /// </summary>
        /// <param name="doctorId">Doctor identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of appointments for the doctor</returns>
        Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves pending appointments for a doctor
        /// </summary>
        /// <param name="doctorId">Doctor identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of pending appointments</returns>
        Task<IEnumerable<Appointment>> GetPendingAppointmentsByDoctorAsync(int doctorId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves today's appointments for a doctor
        /// </summary>
        /// <param name="doctorId">Doctor identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of today's appointments</returns>
        Task<IEnumerable<Appointment>> GetTodaysAppointmentsByDoctorAsync(int doctorId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves current appointment for a patient (today's appointment)
        /// </summary>
        /// <param name="patientId">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Current appointment if found, null otherwise</returns>
        Task<Appointment> GetCurrentAppointmentByPatientAsync(int patientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves appointments by status
        /// </summary>
        /// <param name="status">Appointment status (1-Approved, 2-Pending, 3-Completed, 4-Rejected)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of appointments with specified status</returns>
        Task<IEnumerable<Appointment>> GetByStatusAsync(int status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves appointments by date
        /// </summary>
        /// <param name="date">Date to filter appointments</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of appointments on the specified date</returns>
        Task<IEnumerable<Appointment>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves appointments within a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of appointments within date range</returns>
        Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Approves a pending appointment
        /// </summary>
        /// <param name="appointmentId">Appointment identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if approval was successful, false otherwise</returns>
        Task<bool> ApproveAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Rejects a pending appointment
        /// </summary>
        /// <param name="appointmentId">Appointment identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if rejection was successful, false otherwise</returns>
        Task<bool> RejectAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Completes an appointment
        /// </summary>
        /// <param name="appointmentId">Appointment identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if completion was successful, false otherwise</returns>
        Task<bool> CompleteAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates prescription information for an appointment
        /// </summary>
        /// <param name="appointmentId">Appointment identifier</param>
        /// <param name="disease">Disease diagnosis</param>
        /// <param name="progress">Patient progress notes</param>
        /// <param name="prescription">Prescription details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdatePrescriptionAsync(int appointmentId, string disease, string progress, string prescription, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates bill information for an appointment
        /// </summary>
        /// <param name="appointmentId">Appointment identifier</param>
        /// <param name="billAmount">Bill amount</param>
        /// <param name="billStatus">Bill status (Paid/Unpaid)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdateBillAsync(int appointmentId, float billAmount, string billStatus, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks bill as paid
        /// </summary>
        /// <param name="appointmentId">Appointment identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> MarkBillAsPaidAsync(int appointmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves appointments with unpaid bills
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of appointments with unpaid bills</returns>
        Task<IEnumerable<Appointment>> GetUnpaidAppointmentsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves appointments with unpaid bills for a specific patient
        /// </summary>
        /// <param name="patientId">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of appointments with unpaid bills</returns>
        Task<IEnumerable<Appointment>> GetUnpaidAppointmentsByPatientAsync(int patientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates doctor notification status
        /// </summary>
        /// <param name="appointmentId">Appointment identifier</param>
        /// <param name="status">Notification status (1-Seen, 2-Unseen)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdateDoctorNotificationAsync(int appointmentId, int status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates patient notification status
        /// </summary>
        /// <param name="appointmentId">Appointment identifier</param>
        /// <param name="status">Notification status (1-Seen, 2-Unseen)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdatePatientNotificationAsync(int appointmentId, int status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves appointments with unseen doctor notifications
        /// </summary>
        /// <param name="doctorId">Doctor identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of appointments with unseen notifications</returns>
        Task<IEnumerable<Appointment>> GetUnseenDoctorNotificationsAsync(int doctorId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves appointments with unseen patient notifications
        /// </summary>
        /// <param name="patientId">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of appointments with unseen notifications</returns>
        Task<IEnumerable<Appointment>> GetUnseenPatientNotificationsAsync(int patientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates feedback status for an appointment
        /// </summary>
        /// <param name="appointmentId">Appointment identifier</param>
        /// <param name="status">Feedback status (1-Given, 2-Pending)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdateFeedbackStatusAsync(int appointmentId, int status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves appointments with pending feedback for a patient
        /// </summary>
        /// <param name="patientId">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Appointment with pending feedback if found, null otherwise</returns>
        Task<Appointment> GetPendingFeedbackByPatientAsync(int patientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves treatment history for a patient
        /// </summary>
        /// <param name="patientId">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of completed appointments (treatment history)</returns>
        Task<IEnumerable<Appointment>> GetTreatmentHistoryByPatientAsync(int patientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves bill history for a patient
        /// </summary>
        /// <param name="patientId">Patient identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of appointments with billing information</returns>
        Task<IEnumerable<Appointment>> GetBillHistoryByPatientAsync(int patientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves available time slots for a doctor
        /// </summary>
        /// <param name="doctorId">Doctor identifier</param>
        /// <param name="date">Date to check availability</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of available time slots</returns>
        Task<IEnumerable<DateTime>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves total appointment count
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Total number of appointments</returns>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves total income from paid appointments
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Total income amount</returns>
        Task<float> GetTotalIncomeAsync(CancellationToken cancellationToken = default);
    }
}
