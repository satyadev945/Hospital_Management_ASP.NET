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
    /// Service implementation for patient-related operations
    /// </summary>
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientService> _logger;

        public PatientService(
            IPatientRepository patientRepository,
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            ILogger<PatientService> logger)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PatientDto> GetByIdAsync(int patientId)
        {
            try
            {
                _logger.LogInformation("Retrieving patient with ID: {PatientId}", patientId);

                var patient = await _patientRepository.GetByIdAsync(patientId, CancellationToken.None);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found", patientId);
                    return null;
                }

                var patientDto = _mapper.Map<PatientDto>(patient);
                _logger.LogInformation("Successfully retrieved patient with ID: {PatientId}", patientId);

                return patientDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all patients");

                var patients = await _patientRepository.GetAllAsync(CancellationToken.None);
                var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);

                _logger.LogInformation("Successfully retrieved {Count} patients", ((List<PatientDto>)patientDtos).Count);

                return patientDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patients");
                throw;
            }
        }

        public async Task<PatientDto> CreateAsync(PatientCreateDto patientCreateDto)
        {
            try
            {
                _logger.LogInformation("Creating new patient with email: {Email}", patientCreateDto.Email);

                // Check if email already exists
                var existingPatient = await _patientRepository.GetByEmailAsync(patientCreateDto.Email, CancellationToken.None);
                if (existingPatient != null)
                {
                    _logger.LogWarning("Patient with email {Email} already exists", patientCreateDto.Email);
                    throw new InvalidOperationException($"Patient with email {patientCreateDto.Email} already exists");
                }

                var patient = _mapper.Map<Patient>(patientCreateDto);
                patient.CreatedDate = DateTime.UtcNow;
                patient.IsActive = true;

                var createdPatient = await _patientRepository.AddAsync(patient, CancellationToken.None);
                var patientDto = _mapper.Map<PatientDto>(createdPatient);

                _logger.LogInformation("Successfully created patient with ID: {PatientId}", createdPatient.PatientID);

                return patientDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient with email: {Email}", patientCreateDto.Email);
                throw;
            }
        }

        public async Task<PatientDto> UpdateAsync(PatientUpdateDto patientUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating patient with ID: {PatientId}", patientUpdateDto.PatientID);

                var existingPatient = await _patientRepository.GetByIdAsync(patientUpdateDto.PatientID, CancellationToken.None);
                if (existingPatient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found", patientUpdateDto.PatientID);
                    throw new InvalidOperationException($"Patient with ID {patientUpdateDto.PatientID} not found");
                }

                _mapper.Map(patientUpdateDto, existingPatient);
                existingPatient.ModifiedDate = DateTime.UtcNow;

                var updatedPatient = await _patientRepository.UpdateAsync(existingPatient, CancellationToken.None);
                var patientDto = _mapper.Map<PatientDto>(updatedPatient);

                _logger.LogInformation("Successfully updated patient with ID: {PatientId}", patientUpdateDto.PatientID);

                return patientDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient with ID: {PatientId}", patientUpdateDto.PatientID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int patientId)
        {
            try
            {
                _logger.LogInformation("Deleting patient with ID: {PatientId}", patientId);

                var patient = await _patientRepository.GetByIdAsync(patientId, CancellationToken.None);
                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found", patientId);
                    return false;
                }

                var result = await _patientRepository.DeleteAsync(patientId, CancellationToken.None);

                if (result)
                {
                    _logger.LogInformation("Successfully deleted patient with ID: {PatientId}", patientId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete patient with ID: {PatientId}", patientId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting patient with ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<BillHistoryDto>> GetBillHistoryAsync(int patientId)
        {
            try
            {
                _logger.LogInformation("Retrieving bill history for patient ID: {PatientId}", patientId);

                var appointments = await _appointmentRepository.GetBillHistoryByPatientAsync(patientId, CancellationToken.None);
                var billHistoryDtos = _mapper.Map<IEnumerable<BillHistoryDto>>(appointments);

                _logger.LogInformation("Successfully retrieved bill history for patient ID: {PatientId}", patientId);

                return billHistoryDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bill history for patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<TreatmentHistoryDto>> GetTreatmentHistoryAsync(int patientId)
        {
            try
            {
                _logger.LogInformation("Retrieving treatment history for patient ID: {PatientId}", patientId);

                var appointments = await _appointmentRepository.GetTreatmentHistoryByPatientAsync(patientId, CancellationToken.None);
                var treatmentHistoryDtos = _mapper.Map<IEnumerable<TreatmentHistoryDto>>(appointments);

                _logger.LogInformation("Successfully retrieved treatment history for patient ID: {PatientId}", patientId);

                return treatmentHistoryDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving treatment history for patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<AppointmentDto> GetCurrentAppointmentAsync(int patientId)
        {
            try
            {
                _logger.LogInformation("Retrieving current appointment for patient ID: {PatientId}", patientId);

                var appointment = await _appointmentRepository.GetCurrentAppointmentByPatientAsync(patientId, CancellationToken.None);

                if (appointment == null)
                {
                    _logger.LogInformation("No current appointment found for patient ID: {PatientId}", patientId);
                    return null;
                }

                var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
                _logger.LogInformation("Successfully retrieved current appointment for patient ID: {PatientId}", patientId);

                return appointmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current appointment for patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<NotificationDto> GetNotificationsAsync(int patientId)
        {
            try
            {
                _logger.LogInformation("Retrieving notifications for patient ID: {PatientId}", patientId);

                var unseenNotifications = await _appointmentRepository.GetUnseenPatientNotificationsAsync(patientId, CancellationToken.None);

                var notificationDto = new NotificationDto
                {
                    PatientID = patientId,
                    UnseenCount = ((List<Appointment>)unseenNotifications).Count,
                    Appointments = _mapper.Map<IEnumerable<AppointmentDto>>(unseenNotifications)
                };

                _logger.LogInformation("Successfully retrieved {Count} notifications for patient ID: {PatientId}",
                    notificationDto.UnseenCount, patientId);

                return notificationDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notifications for patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<AppointmentDto> GetPendingFeedbackAsync(int patientId)
        {
            try
            {
                _logger.LogInformation("Retrieving pending feedback appointment for patient ID: {PatientId}", patientId);

                var appointment = await _appointmentRepository.GetPendingFeedbackByPatientAsync(patientId, CancellationToken.None);

                if (appointment == null)
                {
                    _logger.LogInformation("No pending feedback found for patient ID: {PatientId}", patientId);
                    return null;
                }

                var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
                _logger.LogInformation("Successfully retrieved pending feedback appointment for patient ID: {PatientId}", patientId);

                return appointmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending feedback for patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<bool> StoreFeedbackAsync(int appointmentId)
        {
            try
            {
                _logger.LogInformation("Storing feedback for appointment ID: {AppointmentId}", appointmentId);

                var result = await _appointmentRepository.UpdateFeedbackStatusAsync(appointmentId, 1, CancellationToken.None);

                if (result)
                {
                    _logger.LogInformation("Successfully stored feedback for appointment ID: {AppointmentId}", appointmentId);
                }
                else
                {
                    _logger.LogWarning("Failed to store feedback for appointment ID: {AppointmentId}", appointmentId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error storing feedback for appointment ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation("Checking if patient exists with email: {Email}", email);

                var patient = await _patientRepository.GetByEmailAsync(email, CancellationToken.None);
                var exists = patient != null;

                _logger.LogInformation("Patient with email {Email} exists: {Exists}", email, exists);

                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if patient exists with email: {Email}", email);
                throw;
            }
        }
    }
}
