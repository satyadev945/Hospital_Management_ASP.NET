using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(IAppointmentRepository repository, IMapper mapper, ILogger<AppointmentService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all appointments");
            var appointments = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all appointments");
            throw;
        }
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointment with ID: {AppointmentId}", id);
            var appointment = await _repository.GetByIdAsync(id, cancellationToken);
            return appointment != null ? _mapper.Map<AppointmentDto>(appointment) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointment");
            throw;
        }
    }

    public async Task<AppointmentDto> CreateAsync(AppointmentCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new appointment");
            var appointment = _mapper.Map<Appointment>(dto);
            var created = await _repository.AddAsync(appointment, cancellationToken);
            return _mapper.Map<AppointmentDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appointment");
            throw;
        }
    }

    public async Task UpdateAsync(int id, AppointmentUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating appointment with ID: {AppointmentId}", id);
            var appointment = await _repository.GetByIdAsync(id, cancellationToken);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {id} not found");
            }

            _mapper.Map(dto, appointment);
            await _repository.UpdateAsync(appointment, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating appointment");
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting appointment with ID: {AppointmentId}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting appointment");
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for patient: {PatientId}", patientId);
            var appointments = await _repository.GetByPatientIdAsync(patientId, cancellationToken);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient appointments");
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for doctor: {DoctorId}", doctorId);
            var appointments = await _repository.GetByDoctorIdAsync(doctorId, cancellationToken);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor appointments");
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetPendingByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving pending appointments for doctor: {DoctorId}", doctorId);
            var appointments = await _repository.GetPendingByDoctorIdAsync(doctorId, cancellationToken);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending appointments");
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentDto>> GetTodaysByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving today's appointments for doctor: {DoctorId}", doctorId);
            var appointments = await _repository.GetTodaysByDoctorIdAsync(doctorId, cancellationToken);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving today's appointments");
            throw;
        }
    }

    public async Task ApproveAppointmentAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Approving appointment with ID: {AppointmentId}", id);
            var appointment = await _repository.GetByIdAsync(id, cancellationToken);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {id} not found");
            }

            appointment.Status = "Approved";
            appointment.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(appointment, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving appointment");
            throw;
        }
    }
}
