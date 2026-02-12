using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Service implementation for appointment-related operations
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper,
            ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AppointmentDto> GetByIdAsync(int appointId)
        {
            try
            {
                _logger.LogInformation("Retrieving appointment with ID: {AppointId}", appointId);

                var appointment = await _appointmentRepository.GetByIdAsync(appointId, CancellationToken.None);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointId} not found", appointId);
                    return null;
                }

                var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
                _logger.LogInformation("Successfully retrieved appointment with ID: {AppointId}", appointId);

                return appointmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment with ID: {AppointId}", appointId);
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all appointments");

                var appointments = await _appointmentRepository.GetAllAsync(CancellationToken.None);
                var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

                _logger.LogInformation("Successfully retrieved {Count} appointments",
                    ((List<AppointmentDto>)appointmentDtos).Count);

                return appointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all appointments");
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId)
        {
            try
            {
                _logger.LogInformation("Retrieving appointments for patient ID: {PatientId}", patientId);

                var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId, CancellationToken.None);
                var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

                _logger.LogInformation("Successfully retrieved {Count} appointments for patient ID: {PatientId}",
                    ((List<AppointmentDto>)appointmentDtos).Count, patientId);

                return appointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments for patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetByDoctorIdAsync(int doctorId)
        {
            try
            {
                _logger.LogInformation("Retrieving appointments for doctor ID: {DoctorId}", doctorId);

                var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId, CancellationToken.None);
                var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

                _logger.LogInformation("Successfully retrieved {Count} appointments for doctor ID: {DoctorId}",
                    ((List<AppointmentDto>)appointmentDtos).Count, doctorId);

                return appointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments for doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<AppointmentDto> CreateAsync(AppointmentCreateDto appointmentCreateDto)
        {
            try
            {
                _logger.LogInformation("Creating new appointment for patient ID: {PatientId} with doctor ID: {DoctorId}",
                    appointmentCreateDto.PatientID, appointmentCreateDto.DoctorID);

                // Validate patient exists
                var patient = await _patientRepository.GetByIdAsync(
                    appointmentCreateDto.PatientID, CancellationToken.None);
                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found", appointmentCreateDto.PatientID);
                    throw new InvalidOperationException($"Patient with ID {appointmentCreateDto.PatientID} not found");
                }

                // Validate doctor exists
                var doctor = await _doctorRepository.GetByIdAsync(
                    appointmentCreateDto.DoctorID, CancellationToken.None);
                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found", appointmentCreateDto.DoctorID);
                    throw new InvalidOperationException($"Doctor with ID {appointmentCreateDto.DoctorID} not found");
                }

                var appointment = _mapper.Map<Appointment>(appointmentCreateDto);
                appointment.CreatedDate = DateTime.UtcNow;
                appointment.IsActive = true;
                appointment.AppointmentStatus = 2; // 2 = Pending
                appointment.DoctorNotification = 2; // 2 = Unseen
                appointment.PatientNotification = 2; // 2 = Unseen
                appointment.FeedbackStatus = 2; // 2 = Pending
                appointment.BillStatus = "Unpaid";

                var createdAppointment = await _appointmentRepository.AddAsync(appointment, CancellationToken.None);
                var appointmentDto = _mapper.Map<AppointmentDto>(createdAppointment);

                _logger.LogInformation("Successfully created appointment with ID: {AppointId}",
                    createdAppointment.AppointID);

                return appointmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating appointment for patient ID: {PatientId}",
                    appointmentCreateDto.PatientID);
                throw;
            }
        }

        public async Task<AppointmentDto> UpdateAsync(AppointmentUpdateDto appointmentUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating appointment with ID: {AppointId}", appointmentUpdateDto.AppointID);

                var existingAppointment = await _appointmentRepository.GetByIdAsync(
                    appointmentUpdateDto.AppointID, CancellationToken.None);

                if (existingAppointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointId} not found", appointmentUpdateDto.AppointID);
                    throw new InvalidOperationException($"Appointment with ID {appointmentUpdateDto.AppointID} not found");
                }

                _mapper.Map(appointmentUpdateDto, existingAppointment);
                existingAppointment.ModifiedDate = DateTime.UtcNow;

                var updatedAppointment = await _appointmentRepository.UpdateAsync(
                    existingAppointment, CancellationToken.None);
                var appointmentDto = _mapper.Map<AppointmentDto>(updatedAppointment);

                _logger.LogInformation("Successfully updated appointment with ID: {AppointId}",
                    appointmentUpdateDto.AppointID);

                return appointmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating appointment with ID: {AppointId}",
                    appointmentUpdateDto.AppointID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int appointId)
        {
            try
            {
                _logger.LogInformation("Deleting appointment with ID: {AppointId}", appointId);

                var appointment = await _appointmentRepository.GetByIdAsync(appointId, CancellationToken.None);
                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointId} not found", appointId);
                    return false;
                }

                var result = await _appointmentRepository.DeleteAsync(appointId, CancellationToken.None);

                if (result)
                {
                    _logger.LogInformation("Successfully deleted appointment with ID: {AppointId}", appointId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete appointment with ID: {AppointId}", appointId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting appointment with ID: {AppointId}", appointId);
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentSlotDto>> GetFreeSlotsAsync(int doctorId, int patientId, DateTime date)
        {
            try
            {
                _logger.LogInformation("Retrieving free slots for doctor ID: {DoctorId} on date: {Date}",
                    doctorId, date.ToString("yyyy-MM-dd"));

                // Get all appointments for the doctor on the specified date
                var existingAppointments = await _appointmentRepository.GetByDateAsync(date, CancellationToken.None);

                // Filter appointments for the specific doctor
                var doctorAppointments = existingAppointments
                    .Where(a => a.DoctorID == doctorId && a.Date.HasValue)
                    .Select(a => a.Date.Value.Hour)
                    .ToList();

                // Define working hours (9 AM to 5 PM)
                var allSlots = new List<AppointmentSlotDto>();
                for (int hour = 9; hour <= 17; hour++)
                {
                    var slotTime = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                    var isAvailable = !doctorAppointments.Contains(hour);

                    allSlots.Add(new AppointmentSlotDto
                    {
                        DoctorID = doctorId,
                        PatientID = patientId,
                        SlotTime = slotTime,
                        Hour = hour,
                        IsAvailable = isAvailable
                    });
                }

                var freeSlots = allSlots.Where(s => s.IsAvailable).ToList();

                _logger.LogInformation("Successfully retrieved {Count} free slots for doctor ID: {DoctorId}",
                    freeSlots.Count, doctorId);

                return freeSlots;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving free slots for doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<bool> BookAppointmentAsync(int patientId, int doctorId, int slotHour)
        {
            try
            {
                _logger.LogInformation("Booking appointment for patient ID: {PatientId} with doctor ID: {DoctorId} at hour: {SlotHour}",
                    patientId, doctorId, slotHour);

                // Check if patient has active appointments
                var hasActiveAppointment = await HasActivAppointmentAsync(patientId);
                if (hasActiveAppointment)
                {
                    _logger.LogWarning("Patient ID {PatientId} already has an active appointment", patientId);
                    throw new InvalidOperationException("You already have a pending or approved appointment");
                }

                // Get doctor charges
                var doctor = await _doctorRepository.GetByIdAsync(doctorId, CancellationToken.None);
                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID {DoctorId} not found", doctorId);
                    throw new InvalidOperationException($"Doctor with ID {doctorId} not found");
                }

                var appointmentDate = DateTime.Today.AddHours(slotHour);

                var appointment = new Appointment
                {
                    PatientID = patientId,
                    DoctorID = doctorId,
                    Date = appointmentDate,
                    AppointmentStatus = 2, // Pending
                    BillAmount = doctor.Charges,
                    BillStatus = "Unpaid",
                    DoctorNotification = 2, // Unseen
                    PatientNotification = 2, // Unseen
                    FeedbackStatus = 2, // Pending
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var createdAppointment = await _appointmentRepository.AddAsync(appointment, CancellationToken.None);

                _logger.LogInformation("Successfully booked appointment with ID: {AppointId}",
                    createdAppointment.AppointID);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error booking appointment for patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetByStatusAsync(int status)
        {
            try
            {
                _logger.LogInformation("Retrieving appointments with status: {Status}", status);

                var appointments = await _appointmentRepository.GetByStatusAsync(status, CancellationToken.None);
                var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

                _logger.LogInformation("Successfully retrieved {Count} appointments with status: {Status}",
                    ((List<AppointmentDto>)appointmentDtos).Count, status);

                return appointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments with status: {Status}", status);
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Retrieving appointments between {StartDate} and {EndDate}",
                    startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                var appointments = await _appointmentRepository.GetByDateRangeAsync(
                    startDate, endDate, CancellationToken.None);
                var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

                _logger.LogInformation("Successfully retrieved {Count} appointments in date range",
                    ((List<AppointmentDto>)appointmentDtos).Count);

                return appointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments in date range");
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetCurrentMonthAppointmentsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving current month appointments");

                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var appointments = await _appointmentRepository.GetByDateRangeAsync(
                    startDate, endDate, CancellationToken.None);
                var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

                _logger.LogInformation("Successfully retrieved {Count} appointments for current month",
                    ((List<AppointmentDto>)appointmentDtos).Count);

                return appointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current month appointments");
                throw;
            }
        }

        public async Task<bool> UpdateStatusAsync(int appointId, int status)
        {
            try
            {
                _logger.LogInformation("Updating status for appointment ID: {AppointId} to status: {Status}",
                    appointId, status);

                var appointment = await _appointmentRepository.GetByIdAsync(appointId, CancellationToken.None);
                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointId} not found", appointId);
                    return false;
                }

                appointment.AppointmentStatus = status;
                appointment.ModifiedDate = DateTime.UtcNow;

                await _appointmentRepository.UpdateAsync(appointment, CancellationToken.None);

                _logger.LogInformation("Successfully updated status for appointment ID: {AppointId}", appointId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for appointment ID: {AppointId}", appointId);
                throw;
            }
        }

        public async Task<bool> UpdateNotificationsAsync(int appointId, int? doctorNotification, int? patientNotification)
        {
            try
            {
                _logger.LogInformation("Updating notifications for appointment ID: {AppointId}", appointId);

                var appointment = await _appointmentRepository.GetByIdAsync(appointId, CancellationToken.None);
                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointId} not found", appointId);
                    return false;
                }

                if (doctorNotification.HasValue)
                {
                    await _appointmentRepository.UpdateDoctorNotificationAsync(
                        appointId, doctorNotification.Value, CancellationToken.None);
                }

                if (patientNotification.HasValue)
                {
                    await _appointmentRepository.UpdatePatientNotificationAsync(
                        appointId, patientNotification.Value, CancellationToken.None);
                }

                _logger.LogInformation("Successfully updated notifications for appointment ID: {AppointId}", appointId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating notifications for appointment ID: {AppointId}", appointId);
                throw;
            }
        }

        public async Task<bool> HasActivAppointmentAsync(int patientId)
        {
            try
            {
                _logger.LogInformation("Checking if patient ID: {PatientId} has active appointments", patientId);

                var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId, CancellationToken.None);

                // Check for pending (2) or approved (1) appointments
                var hasActiveAppointment = appointments.Any(a =>
                    a.AppointmentStatus == 1 || a.AppointmentStatus == 2);

                _logger.LogInformation("Patient ID {PatientId} has active appointment: {HasActive}",
                    patientId, hasActiveAppointment);

                return hasActiveAppointment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking active appointments for patient ID: {PatientId}", patientId);
                throw;
            }
        }
    }
}
