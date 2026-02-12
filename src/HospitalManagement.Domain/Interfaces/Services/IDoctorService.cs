using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services
{
    public interface IDoctorService
    {
        /// <summary>
        /// Gets a doctor by ID
        /// </summary>
        Task<DoctorDto> GetByIdAsync(int doctorId);

        /// <summary>
        /// Gets all doctors
        /// </summary>
        Task<IEnumerable<DoctorDto>> GetAllAsync();

        /// <summary>
        /// Gets all active doctors (status = 1)
        /// </summary>
        Task<IEnumerable<DoctorDto>> GetAllActiveAsync();

        /// <summary>
        /// Gets doctors by department
        /// </summary>
        Task<IEnumerable<DoctorDto>> GetByDepartmentAsync(string departmentName);

        /// <summary>
        /// Creates a new doctor
        /// </summary>
        Task<DoctorDto> CreateAsync(DoctorCreateDto doctorCreateDto);

        /// <summary>
        /// Updates an existing doctor
        /// </summary>
        Task<DoctorDto> UpdateAsync(DoctorUpdateDto doctorUpdateDto);

        /// <summary>
        /// Soft deletes a doctor by setting status to 0
        /// </summary>
        Task<bool> DeleteAsync(int doctorId);

        /// <summary>
        /// Gets pending appointments for a doctor
        /// </summary>
        Task<IEnumerable<PendingAppointmentDto>> GetPendingAppointmentsAsync(int doctorId);

        /// <summary>
        /// Gets today's appointments for a doctor
        /// </summary>
        Task<IEnumerable<TodaysAppointmentDto>> GetTodaysAppointmentsAsync(int doctorId);

        /// <summary>
        /// Gets patient history for a doctor
        /// </summary>
        Task<IEnumerable<TreatmentHistoryDto>> GetPatientHistoryAsync(int doctorId);

        /// <summary>
        /// Approves an appointment
        /// </summary>
        Task<bool> ApproveAppointmentAsync(int appointmentId);

        /// <summary>
        /// Rejects an appointment
        /// </summary>
        Task<bool> RejectAppointmentAsync(int appointmentId);

        /// <summary>
        /// Completes an appointment with prescription
        /// </summary>
        Task<bool> CompleteAppointmentAsync(AppointmentCompleteDto appointmentCompleteDto);

        /// <summary>
        /// Updates prescription for an appointment
        /// </summary>
        Task<bool> UpdatePrescriptionAsync(int appointmentId, string disease, string progress, string prescription);

        /// <summary>
        /// Checks if doctor exists by email
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// Gets doctor's charges per visit
        /// </summary>
        Task<decimal> GetChargesAsync(int doctorId);
    }
}
