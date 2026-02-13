using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Interfaces;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Services;

/// <summary>
/// Service implementation for doctor management operations.
/// </summary>
public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DoctorService> _logger;

    public DoctorService(
        IDoctorRepository doctorRepository,
        IDepartmentRepository departmentRepository,
        IMapper mapper,
        ILogger<DoctorService> logger)
    {
        _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all doctors");
            var doctors = await _doctorRepository.GetAllAsync(cancellationToken);
            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            _logger.LogInformation("Retrieved {Count} doctors", doctorDtos.Count());
            return doctorDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all doctors");
            throw;
        }
    }

    public async Task<DoctorDto?> GetByIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctor with ID: {DoctorId}", doctorId);
            var doctor = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);
            
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
            _logger.LogError(ex, "Error occurred while retrieving doctor with ID: {DoctorId}", doctorId);
            throw;
        }
    }

    public async Task<DoctorDto> CreateAsync(DoctorCreateDto doctorCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (doctorCreateDto == null)
            {
                throw new ArgumentNullException(nameof(doctorCreateDto));
            }

            _logger.LogInformation("Creating new doctor: {FirstName} {LastName}", doctorCreateDto.FirstName, doctorCreateDto.LastName);
            
            // Validate department exists
            var departmentExists = await _departmentRepository.ExistsAsync(doctorCreateDto.DepartmentId, cancellationToken);
            if (!departmentExists)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found", doctorCreateDto.DepartmentId);
                throw new KeyNotFoundException($"Department with ID {doctorCreateDto.DepartmentId} not found");
            }

            // Check if doctor with same email already exists
            var existingDoctor = await _doctorRepository.GetByEmailAsync(doctorCreateDto.Email, cancellationToken);
            if (existingDoctor != null)
            {
                _logger.LogWarning("Doctor with email {Email} already exists", doctorCreateDto.Email);
                throw new InvalidOperationException($"Doctor with email {doctorCreateDto.Email} already exists");
            }

            var doctor = _mapper.Map<Doctor>(doctorCreateDto);
            doctor.CreatedDate = DateTime.UtcNow;
            doctor.IsActive = true;
            doctor.Status = true;
            doctor.CreatedBy = "System"; // Should be replaced with actual user context
            
            var createdDoctor = await _doctorRepository.AddAsync(doctor, cancellationToken);
            
            var createdDoctorDto = _mapper.Map<DoctorDto>(createdDoctor);
            _logger.LogInformation("Successfully created doctor with ID: {DoctorId}", createdDoctor.DoctorID);
            
            return createdDoctorDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating doctor");
            throw;
        }
    }

    public async Task<DoctorDto> UpdateAsync(DoctorUpdateDto doctorUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (doctorUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(doctorUpdateDto));
            }

            _logger.LogInformation("Updating doctor with ID: {DoctorId}", doctorUpdateDto.DoctorId);
            
            var existingDoctor = await _doctorRepository.GetByIdAsync(doctorUpdateDto.DoctorId, cancellationToken);
            if (existingDoctor == null)
            {
                _logger.LogWarning("Doctor with ID {DoctorId} not found for update", doctorUpdateDto.DoctorId);
                throw new KeyNotFoundException($"Doctor with ID {doctorUpdateDto.DoctorId} not found");
            }

            // Validate department exists
            var departmentExists = await _departmentRepository.ExistsAsync(doctorUpdateDto.DepartmentId, cancellationToken);
            if (!departmentExists)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found", doctorUpdateDto.DepartmentId);
                throw new KeyNotFoundException($"Department with ID {doctorUpdateDto.DepartmentId} not found");
            }

            // Check if email is being changed to one that already exists
            if (existingDoctor.Email != doctorUpdateDto.Email)
            {
                var emailExists = await _doctorRepository.GetByEmailAsync(doctorUpdateDto.Email, cancellationToken);
                if (emailExists != null)
                {
                    _logger.LogWarning("Doctor with email {Email} already exists", doctorUpdateDto.Email);
                    throw new InvalidOperationException($"Doctor with email {doctorUpdateDto.Email} already exists");
                }
            }

            _mapper.Map(doctorUpdateDto, existingDoctor);
            existingDoctor.ModifiedDate = DateTime.UtcNow;
            existingDoctor.ModifiedBy = "System"; // Should be replaced with actual user context
            
            var updatedDoctor = await _doctorRepository.UpdateAsync(existingDoctor, cancellationToken);
            
            var updatedDoctorDto = _mapper.Map<DoctorDto>(updatedDoctor);
            _logger.LogInformation("Successfully updated doctor with ID: {DoctorId}", doctorUpdateDto.DoctorId);
            
            return updatedDoctorDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating doctor with ID: {DoctorId}", doctorUpdateDto?.DoctorId);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting doctor with ID: {DoctorId}", doctorId);
            
            var existingDoctor = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);
            if (existingDoctor == null)
            {
                _logger.LogWarning("Doctor with ID {DoctorId} not found for deletion", doctorId);
                return false;
            }

            var result = await _doctorRepository.DeleteAsync(doctorId, cancellationToken);
            
            if (result)
            {
                _logger.LogInformation("Successfully deleted doctor with ID: {DoctorId}", doctorId);
            }
            else
            {
                _logger.LogWarning("Failed to delete doctor with ID: {DoctorId}", doctorId);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting doctor with ID: {DoctorId}", doctorId);
            throw;
        }
    }

    public async Task<IEnumerable<DoctorDto>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctors for department ID: {DepartmentId}", departmentId);
            var doctors = await _doctorRepository.GetByDepartmentAsync(departmentId, cancellationToken);
            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            _logger.LogInformation("Retrieved {Count} doctors for department ID: {DepartmentId}", doctorDtos.Count(), departmentId);
            return doctorDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving doctors for department ID: {DepartmentId}", departmentId);
            throw;
        }
    }

    public async Task<IEnumerable<DoctorDto>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(specialization))
            {
                throw new ArgumentException("Specialization cannot be null or empty", nameof(specialization));
            }

            _logger.LogInformation("Retrieving doctors by specialization: {Specialization}", specialization);
            var allDoctors = await _doctorRepository.GetAllAsync(cancellationToken);
            
            var doctors = allDoctors
                .Where(d => d.IsActive && 
                           d.Specialization.Equals(specialization, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            _logger.LogInformation("Found {Count} doctors with specialization: {Specialization}", doctorDtos.Count(), specialization);
            return doctorDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving doctors by specialization: {Specialization}", specialization);
            throw;
        }
    }

    public async Task<IEnumerable<DoctorDto>> GetAvailableDoctorsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving available doctors");
            var allDoctors = await _doctorRepository.GetAllAsync(cancellationToken);
            
            var availableDoctors = allDoctors
                .Where(d => d.IsActive && d.Status)
                .ToList();
            
            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(availableDoctors);
            _logger.LogInformation("Retrieved {Count} available doctors", doctorDtos.Count());
            return doctorDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving available doctors");
            throw;
        }
    }

    public async Task<DoctorDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            _logger.LogInformation("Retrieving doctor by email: {Email}", email);
            var doctor = await _doctorRepository.GetByEmailAsync(email, cancellationToken);
            
            if (doctor == null)
            {
                _logger.LogWarning("Doctor with email {Email} not found", email);
                return null;
            }

            var doctorDto = _mapper.Map<DoctorDto>(doctor);
            _logger.LogInformation("Successfully retrieved doctor with email: {Email}", email);
            return doctorDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving doctor by email: {Email}", email);
            throw;
        }
    }
}
