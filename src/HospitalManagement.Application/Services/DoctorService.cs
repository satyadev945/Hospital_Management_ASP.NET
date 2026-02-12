using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Domain.Interfaces.Services;

namespace HospitalManagement.Application.Services
{
    /// <summary>
    /// Service implementation for doctor-related operations
    /// </summary>
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DoctorService> _logger;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            ILogger<DoctorService> logger)
        {
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DoctorDto> GetByIdAsync(int doctorId)
        {
            try
            {
                _logger.LogInformation("Retrieving doctor with ID: {DoctorId}", doctorId);

                var doctor = await _doctorRepository.GetByIdAsync(doctorId, CancellationToken.None);

                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found", doctorId);
                    return null;
                }

                var doctorDto = _mapper.Map<DoctorDto>(doctor);
                _logger.LogInformation("Successfully retrieved doctor with ID: {DoctorId}", doctorId);

                return doctorDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctor with ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all doctors");

                var doctors = await _doctorRepository.GetAllAsync(CancellationToken.None);
                var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);

                _logger.LogInformation("Successfully retrieved {Count} doctors", ((List<DoctorDto>)doctorDtos).Count);

                return doctorDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all doctors");
                throw;
            }
        }

        public async Task<IEnumerable<DoctorDto>> GetAllActiveAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all active doctors");

                var doctors = await _doctorRepository.GetActiveDoctorsAsync(CancellationToken.None);
                var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);

                _logger.LogInformation("Successfully retrieved {Count} active doctors", ((List<DoctorDto>)doctorDtos).Count);

                return doctorDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all active doctors");
                throw;
            }
        }

        public async Task<IEnumerable<DoctorDto>> GetByDepartmentAsync(string departmentName)
        {
            try
            {
                _logger.LogInformation("Retrieving doctors for department: {DepartmentName}", departmentName);

                var doctors = await _doctorRepository.GetByDepartmentNameAsync(departmentName, CancellationToken.None);
                var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);

                _logger.LogInformation("Successfully retrieved {Count} doctors for department: {DepartmentName}",
                    ((List<DoctorDto>)doctorDtos).Count, departmentName);

                return doctorDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctors for department: {DepartmentName}", departmentName);
                throw;
            }
        }

        public async Task<DoctorDto> CreateAsync(DoctorCreateDto doctorCreateDto)
        {
            try
            {
                _logger.LogInformation("Creating new doctor with email: {Email}", doctorCreateDto.Email);

                // Check if email already exists
                var existingDoctor = await _doctorRepository.GetByEmailAsync(doctorCreateDto.Email, CancellationToken.None);
                if (existingDoctor != null)
                {
                    _logger.LogWarning("Doctor with email {Email} already exists", doctorCreateDto.Email);
                    throw new InvalidOperationException($"Doctor with email {doctorCreateDto.Email} already exists");
                }

                var doctor = _mapper.Map<Doctor>(doctorCreateDto);
                doctor.CreatedDate = DateTime.UtcNow;
                doctor.IsActive = true;
                doctor.PatientsTreated = 0;
                doctor.ReputeIndex = 0.0m;

                var createdDoctor = await _doctorRepository.AddAsync(doctor, CancellationToken.None);
                var doctorDto = _mapper.Map<DoctorDto>(createdDoctor);

                _logger.LogInformation("Successfully created doctor with ID: {DoctorId}", createdDoctor.DoctorID);

                return doctorDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating doctor with email: {Email}", doctorCreateDto.Email);
                throw;
            }
        }

        public async Task<DoctorDto> UpdateAsync(DoctorUpdateDto doctorUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating doctor with ID: {DoctorId}", doctorUpdateDto.DoctorID);

                var existingDoctor = await _doctorRepository.GetByIdAsync(doctorUpdateDto.DoctorID, CancellationToken.None);
                if (existingDoctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found", doctorUpdateDto.DoctorID);
                    throw new InvalidOperationException($"Doctor with ID {doctorUpdateDto.DoctorID} not found");
                }

                _mapper.Map(doctorUpdateDto, existingDoctor);
                existingDoctor.ModifiedDate = DateTime.UtcNow;

                var updatedDoctor = await _doctorRepository.UpdateAsync(existingDoctor, CancellationToken.None);
                var doctorDto = _mapper.Map<DoctorDto>(updatedDoctor);

                _logger.LogInformation("Successfully updated doctor with ID: {DoctorId}", doctorUpdateDto.DoctorId);

                return doctorDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating doctor with ID: {DoctorId}", doctorUpdateDto.DoctorID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int doctorId)
        {
            try
            {
                _logger.LogInformation("Soft deleting doctor with ID: {DoctorId}", doctorId);

                var doctor = await _doctorRepository.GetByIdAsync(doctorId, CancellationToken.None);
                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found", doctorId);
                    return false;
                }

                var result = await _doctorRepository.DeleteAsync(doctorId, CancellationToken.None);

                if (result)
                {
                    _logger.LogInformation("Successfully soft deleted doctor with ID: {DoctorId}", doctorId);
                }
                else
                {
                    _logger.LogWarning("Failed to soft delete doctor with ID: {DoctorId}", doctorId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting doctor with ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<IEnumerable<PendingAppointmentDto>> GetPendingAppointmentsAsync(int doctorId)
        {
            try
            {
                _logger.LogInformation("Retrieving pending appointments for doctor ID: {DoctorId}", doctorId);

                var appointments = await _appointmentRepository.GetPendingAppointmentsByDoctorAsync(doctorId, CancellationToken.None);
                var pendingAppointmentDtos = _mapper.Map<IEnumerable<PendingAppointmentDto>>(appointments);

                _logger.LogInformation("Successfully retrieved {Count} pending appointments for doctor ID: {DoctorId}",
                    ((List<PendingAppointmentDto>)pendingAppointmentDtos).Count, doctorId);

                return pendingAppointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending appointments for doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<IEnumerable<TodaysAppointmentDto>> GetTodaysAppointmentsAsync(int doctorId)
        {
            try
            {
                _logger.LogInformation("Retrieving today's appointments for doctor ID: {DoctorId}", doctorId);

                var appointments = await _appointmentRepository.GetTodaysAppointmentsByDoctorAsync(doctorId, CancellationToken.None);
                var todaysAppointmentDtos = _mapper.Map<IEnumerable<TodaysAppointmentDto>>(appointments);

                _logger.LogInformation("Successfully retrieved {Count} today's appointments for doctor ID: {DoctorId}",
                    ((List<TodaysAppointmentDto>)todaysAppointmentDtos).Count, doctorId);

                return todaysAppointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving today's appointments for doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<IEnumerable<TreatmentHistoryDto>> GetPatientHistoryAsync(int doctorId)
        {
            try
            {
                _logger.LogInformation("Retrieving patient history for doctor ID: {DoctorId}", doctorId);

                var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId, CancellationToken.None);

                // Filter completed appointments
                var completedAppointments = new List<Appointment>();
                foreach (var appointment in appointments)
                {
                    if (appointment.AppointmentStatus == 3) // 3 = Completed
                    {
                        completedAppointments.Add(appointment);
                    }
                }

                var treatmentHistoryDtos = _mapper.Map<IEnumerable<TreatmentHistoryDto>>(completedAppointments);

                _logger.LogInformation("Successfully retrieved {Count} patient history records for doctor ID: {DoctorId}",
                    completedAppointments.Count, doctorId);

                return treatmentHistoryDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient history for doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<bool> ApproveAppointmentAsync(int appointmentId)
        {
            try
            {
                _logger.LogInformation("Approving appointment with ID: {AppointmentId}", appointmentId);

                var result = await _appointmentRepository.ApproveAppointmentAsync(appointmentId, CancellationToken.None);

                if (result)
                {
                    _logger.LogInformation("Successfully approved appointment with ID: {AppointmentId}", appointmentId);
                }
                else
                {
                    _logger.LogWarning("Failed to approve appointment with ID: {AppointmentId}", appointmentId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving appointment with ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<bool> RejectAppointmentAsync(int appointmentId)
        {
            try
            {
                _logger.LogInformation("Rejecting appointment with ID: {AppointmentId}", appointmentId);

                var result = await _appointmentRepository.RejectAppointmentAsync(appointmentId, CancellationToken.None);

                if (result)
                {
                    _logger.LogInformation("Successfully rejected appointment with ID: {AppointmentId}", appointmentId);
                }
                else
                {
                    _logger.LogWarning("Failed to reject appointment with ID: {AppointmentId}", appointmentId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting appointment with ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<bool> CompleteAppointmentAsync(AppointmentCompleteDto appointmentCompleteDto)
        {
            try
            {
                _logger.LogInformation("Completing appointment with ID: {AppointmentId}", appointmentCompleteDto.AppointID);

                // Update prescription information
                var prescriptionUpdated = await _appointmentRepository.UpdatePrescriptionAsync(
                    appointmentCompleteDto.AppointID,
                    appointmentCompleteDto.Disease,
                    appointmentCompleteDto.Progress,
                    appointmentCompleteDto.Prescription,
                    CancellationToken.None);

                if (!prescriptionUpdated)
                {
                    _logger.LogWarning("Failed to update prescription for appointment ID: {AppointmentId}",
                        appointmentCompleteDto.AppointID);
                    return false;
                }

                // Mark appointment as completed
                var completionResult = await _appointmentRepository.CompleteAppointmentAsync(
                    appointmentCompleteDto.AppointID, CancellationToken.None);

                if (!completionResult)
                {
                    _logger.LogWarning("Failed to complete appointment with ID: {AppointmentId}",
                        appointmentCompleteDto.AppointID);
                    return false;
                }

                // Update bill status if paid
                if (appointmentCompleteDto.IsBillPaid)
                {
                    await _appointmentRepository.MarkBillAsPaidAsync(appointmentCompleteDto.AppointID, CancellationToken.None);
                }

                // Increment patients treated count for doctor
                await _doctorRepository.IncrementPatientsTreatedAsync(appointmentCompleteDto.DoctorID, CancellationToken.None);

                _logger.LogInformation("Successfully completed appointment with ID: {AppointmentId}",
                    appointmentCompleteDto.AppointID);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing appointment with ID: {AppointmentId}",
                    appointmentCompleteDto.AppointID);
                throw;
            }
        }

        public async Task<bool> UpdatePrescriptionAsync(int appointmentId, string disease, string progress, string prescription)
        {
            try
            {
                _logger.LogInformation("Updating prescription for appointment ID: {AppointmentId}", appointmentId);

                var result = await _appointmentRepository.UpdatePrescriptionAsync(
                    appointmentId, disease, progress, prescription, CancellationToken.None);

                if (result)
                {
                    _logger.LogInformation("Successfully updated prescription for appointment ID: {AppointmentId}", appointmentId);
                }
                else
                {
                    _logger.LogWarning("Failed to update prescription for appointment ID: {AppointmentId}", appointmentId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating prescription for appointment ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation("Checking if doctor exists with email: {Email}", email);

                var doctor = await _doctorRepository.GetByEmailAsync(email, CancellationToken.None);
                var exists = doctor != null;

                _logger.LogInformation("Doctor with email {Email} exists: {Exists}", email, exists);

                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if doctor exists with email: {Email}", email);
                throw;
            }
        }

        public async Task<decimal> GetChargesAsync(int doctorId)
        {
            try
            {
                _logger.LogInformation("Retrieving charges for doctor ID: {DoctorId}", doctorId);

                var doctor = await _doctorRepository.GetByIdAsync(doctorId, CancellationToken.None);

                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found", doctorId);
                    throw new InvalidOperationException($"Doctor with ID {doctorId} not found");
                }

                var charges = doctor.Charges;
                _logger.LogInformation("Successfully retrieved charges {Charges} for doctor ID: {DoctorId}",
                    charges, doctorId);

                return charges;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving charges for doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }
    }
}
