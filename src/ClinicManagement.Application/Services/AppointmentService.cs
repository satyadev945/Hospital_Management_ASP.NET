using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Interfaces;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Services;

/// <summary>
/// Service implementation for appointment management operations.
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

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all appointments");
            var appointments = await _appointmentRepository.GetAllAsync(cancellationToken);
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Retrieved {Count} appointments", appointmentDtos.Count());
            return appointmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all appointments");
            throw;
        }
    }

    public async Task<AppointmentDto?> GetByIdAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointment with ID: {AppointmentId}", appointmentId);
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId, cancellationToken);
            
            if (appointment == null)
            {
                _logger.LogWarning("Appointment with ID {AppointmentId} not found", appointmentId);
                return null;
            }

            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            _logger.LogInformation("Successfully retrieved appointment with ID: {AppointmentId}", appointmentId);
            return appointmentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointment with ID: {AppointmentId}", appointmentId);
            throw;
        }
    }

    public async Task<AppointmentDto> CreateAsync(AppointmentCreateDto appointmentCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (appointmentCreateDto == null)
            {
                throw new ArgumentNullException(nameof(appointmentCreateDto));
            }

            _logger.LogInformation("Creating new appointment for patient ID: {PatientId} with doctor ID: {DoctorId}", 
                appointmentCreateDto.PatientId, appointmentCreateDto.DoctorId);
            
            // Validate patient exists
            var patientExists = await _patientRepository.ExistsAsync(appointmentCreateDto.PatientId, cancellationToken);
            if (!patientExists)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found", appointmentCreateDto.PatientId);
                throw new KeyNotFoundException($"Patient with ID {appointmentCreateDto.PatientId} not found");
            }

            // Validate doctor exists
            var doctorExists = await _doctorRepository.ExistsAsync(appointmentCreateDto.DoctorId, cancellationToken);
            if (!doctorExists)
            {
                _logger.LogWarning("Doctor with ID {DoctorId} not found", appointmentCreateDto.DoctorId);
                throw new KeyNotFoundException($"Doctor with ID {appointmentCreateDto.DoctorId} not found");
            }

            var appointment = _mapper.Map<Appointment>(appointmentCreateDto);
            appointment.CreatedDate = DateTime.UtcNow;
            appointment.IsActive = true;
            appointment.Status = "Scheduled"; // Default status
            appointment.CreatedBy = "System"; // Should be replaced with actual user context
            
            var createdAppointment = await _appointmentRepository.AddAsync(appointment, cancellationToken);
            
            var createdAppointmentDto = _mapper.Map<AppointmentDto>(createdAppointment);
            _logger.LogInformation("Successfully created appointment with ID: {AppointmentId}", createdAppointment.AppointmentID);
            
            return createdAppointmentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating appointment");
            throw;
        }
    }

    public async Task<AppointmentDto> UpdateAsync(AppointmentUpdateDto appointmentUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (appointmentUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(appointmentUpdateDto));
            }

            _logger.LogInformation("Updating appointment with ID: {AppointmentId}", appointmentUpdateDto.AppointmentId);
            
            var existingAppointment = await _appointmentRepository.GetByIdAsync(appointmentUpdateDto.AppointmentId, cancellationToken);
            if (existingAppointment == null)
            {
                _logger.LogWarning("Appointment with ID {AppointmentId} not found for update", appointmentUpdateDto.AppointmentId);
                throw new KeyNotFoundException($"Appointment with ID {appointmentUpdateDto.AppointmentId} not found");
            }

            // Validate patient exists
            var patientExists = await _patientRepository.ExistsAsync(appointmentUpdateDto.PatientId, cancellationToken);
            if (!patientExists)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found", appointmentUpdateDto.PatientId);
                throw new KeyNotFoundException($"Patient with ID {appointmentUpdateDto.PatientId} not found");
            }

            // Validate doctor exists
            var doctorExists = await _doctorRepository.ExistsAsync(appointmentUpdateDto.DoctorId, cancellationToken);
            if (!doctorExists)
            {
                _logger.LogWarning("Doctor with ID {DoctorId} not found", appointmentUpdateDto.DoctorId);
                throw new KeyNotFoundException($"Doctor with ID {appointmentUpdateDto.DoctorId} not found");
            }

            _mapper.Map(appointmentUpdateDto, existingAppointment);
            existingAppointment.ModifiedDate = DateTime.UtcNow;
            existingAppointment.ModifiedBy = "System"; // Should be replaced with actual user context
            
            var updatedAppointment = await _appointmentRepository.UpdateAsync(existingAppointment, cancellationToken);
            
            var updatedAppointmentDto = _mapper.Map<AppointmentDto>(updatedAppointment);
            _logger.LogInformation("Successfully updated appointment with ID: {AppointmentId}", appointmentUpdateDto.AppointmentId);
            
            return updatedAppointmentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating appointment with ID: {AppointmentId}", appointmentUpdateDto?.AppointmentId);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting appointment with ID: {AppointmentId}", appointmentId);
            
            var existingAppointment = await _appointmentRepository.GetByIdAsync(appointmentId, cancellationToken);
            if (existingAppointment == null)
            {
                _logger.LogWarning("Appointment with ID {AppointmentId} not found for deletion", appointmentId);
                return false;
            }

            var result = await _appointmentRepository.DeleteAsync(appointmentId, cancellationToken);
            
            if (result)
            {
                _logger.LogInformation("Successfully deleted appointment with ID: {AppointmentId}", appointmentId);
            }
            else
            {
                _logger.LogWarning("Failed to delete appointment with ID: {AppointmentId}", appointmentId);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting appointment with ID: {AppointmentId}", appointmentId);
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for patient ID: {PatientId}", patientId);
            var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId, cancellationToken);
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Retrieved {Count} appointments for patient ID: {PatientId}", appointmentDtos.Count(), patientId);
            return appointmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointments for patient ID: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for doctor ID: {DoctorId}", doctorId);
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId, cancellationToken);
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Retrieved {Count} appointments for doctor ID: {DoctorId}", appointmentDtos.Count(), doctorId);
            return appointmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointments for doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for date: {Date}", date.ToString("yyyy-MM-dd"));
            var allAppointments = await _appointmentRepository.GetAllAsync(cancellationToken);
            
            var appointments = allAppointments
                .Where(a => a.IsActive && a.AppointmentDate.Date == date.Date)
                .OrderBy(a => a.AppointmentDate)
                .ToList();
            
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Retrieved {Count} appointments for date: {Date}", appointmentDtos.Count(), date.ToString("yyyy-MM-dd"));
            return appointmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointments for date: {Date}", date.ToString("yyyy-MM-dd"));
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be greater than end date");
            }

            _logger.LogInformation("Retrieving appointments from {StartDate} to {EndDate}", 
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            
            var allAppointments = await _appointmentRepository.GetAllAsync(cancellationToken);
            
            var appointments = allAppointments
                .Where(a => a.IsActive && 
                           a.AppointmentDate.Date >= startDate.Date && 
                           a.AppointmentDate.Date <= endDate.Date)
                .OrderBy(a => a.AppointmentDate)
                .ToList();
            
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Retrieved {Count} appointments for date range", appointmentDtos.Count());
            return appointmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointments for date range");
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException("Status cannot be null or empty", nameof(status));
            }

            _logger.LogInformation("Retrieving appointments with status: {Status}", status);
            var allAppointments = await _appointmentRepository.GetAllAsync(cancellationToken);
            
            var appointments = allAppointments
                .Where(a => a.IsActive && a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.AppointmentDate)
                .ToList();
            
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Retrieved {Count} appointments with status: {Status}", appointmentDtos.Count(), status);
            return appointmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointments by status: {Status}", status);
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetTodayAppointmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving today's appointments");
            var appointments = await _appointmentRepository.GetTodayAsync(cancellationToken);
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Retrieved {Count} appointments for today", appointmentDtos.Count());
            return appointmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving today's appointments");
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(int daysAhead, CancellationToken cancellationToken = default)
    {
        try
        {
            if (daysAhead <= 0)
            {
                throw new ArgumentException("Days ahead must be greater than zero", nameof(daysAhead));
            }

            _logger.LogInformation("Retrieving upcoming appointments for next {DaysAhead} days", daysAhead);
            var today = DateTime.UtcNow.Date;
            var endDate = today.AddDays(daysAhead);
            
            var allAppointments = await _appointmentRepository.GetAllAsync(cancellationToken);
            
            var appointments = allAppointments
                .Where(a => a.IsActive && 
                           a.AppointmentDate.Date >= today && 
                           a.AppointmentDate.Date <= endDate)
                .OrderBy(a => a.AppointmentDate)
                .ToList();
            
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Retrieved {Count} upcoming appointments", appointmentDtos.Count());
            return appointmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving upcoming appointments");
            throw;
        }
    }

    public async Task<bool> UpdateStatusAsync(int appointmentId, string status, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException("Status cannot be null or empty", nameof(status));
            }

            _logger.LogInformation("Updating status for appointment ID: {AppointmentId} to {Status}", appointmentId, status);
            
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId, cancellationToken);
            if (appointment == null)
            {
                _logger.LogWarning("Appointment with ID {AppointmentId} not found", appointmentId);
                return false;
            }

            appointment.Status = status;
            appointment.ModifiedDate = DateTime.UtcNow;
            appointment.ModifiedBy = "System"; // Should be replaced with actual user context
            
            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
            
            _logger.LogInformation("Successfully updated status for appointment ID: {AppointmentId}", appointmentId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating status for appointment ID: {AppointmentId}", appointmentId);
            throw;
        }
    }
}
