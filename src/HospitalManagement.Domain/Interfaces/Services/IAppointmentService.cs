using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services
{
    public interface IAppointmentService
    {
        /// <summary>
        /// Gets an appointment by ID
        /// </summary>
        Task<AppointmentDto> GetByIdAsync(int appointId);

        /// <summary>
        /// Gets all appointments
        /// </summary>
        Task<IEnumerable<AppointmentDto>> GetAllAsync();

        /// <summary>
        /// Gets appointments by patient ID
        /// </summary>
        Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId);

        /// <summary>
        /// Gets appointments by doctor ID
        /// </summary>
        Task<IEnumerable<AppointmentDto>> GetByDoctorIdAsync(int doctorId);

        /// <summary>
        /// Creates a new appointment
        /// </summary>
        Task<AppointmentDto> CreateAsync(AppointmentCreateDto appointmentCreateDto);

        /// <summary>
        /// Updates an existing appointment
        /// </summary>
        Task<AppointmentDto> UpdateAsync(AppointmentUpdateDto appointmentUpdateDto);

        /// <summary>
        /// Deletes an appointment by ID
        /// </summary>
        Task<bool> DeleteAsync(int appointId);

        /// <summary>
        /// Gets free appointment slots for a doctor on a specific date
        /// </summary>
        Task<IEnumerable<AppointmentSlotDto>> GetFreeSlotsAsync(int doctorId, int patientId, DateTime date);

        /// <summary>
        /// Books an appointment for a patient
        /// </summary>
        Task<bool> BookAppointmentAsync(int patientId, int doctorId, int slotHour);

        /// <summary>
        /// Gets appointments by status
        /// </summary>
        Task<IEnumerable<AppointmentDto>> GetByStatusAsync(int status);

        /// <summary>
        /// Gets appointments for a specific date range
        /// </summary>
        Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets current month's appointments
        /// </summary>
        Task<IEnumerable<AppointmentDto>> GetCurrentMonthAppointmentsAsync();

        /// <summary>
        /// Updates appointment status
        /// </summary>
        Task<bool> UpdateStatusAsync(int appointId, int status);

        /// <summary>
        /// Updates appointment notifications
        /// </summary>
        Task<bool> UpdateNotificationsAsync(int appointId, int? doctorNotification, int? patientNotification);

        /// <summary>
        /// Checks if patient has pending or approved appointments
        /// </summary>
        Task<bool> HasActivAppointmentAsync(int patientId);
    }
}
