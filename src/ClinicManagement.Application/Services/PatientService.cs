using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Interfaces;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Services;

/// <summary>
/// Service implementation for patient management operations.
/// </summary>
public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PatientService> _logger;

    public PatientService(
        IPatientRepository patientRepository,
        IMapper mapper,
        ILogger<PatientService> logger)
    {
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all patients");
            var patients = await _patientRepository.GetAllAsync(cancellationToken);
            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);
            _logger.LogInformation("Retrieved {Count} patients", patientDtos.Count());
            return patientDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all patients");
            throw;
        }
    }

    public async Task<PatientDto?> GetByIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving patient with ID: {PatientId}", patientId);
            var patient = await _patientRepository.GetByIdAsync(patientId, cancellationToken);
            
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
            _logger.LogError(ex, "Error occurred while retrieving patient with ID: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<PatientDto> CreateAsync(PatientCreateDto patientCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (patientCreateDto == null)
            {
                throw new ArgumentNullException(nameof(patientCreateDto));
            }

            _logger.LogInformation("Creating new patient: {FirstName} {LastName}", patientCreateDto.FirstName, patientCreateDto.LastName);
            
            // Check if patient with same email already exists
            var existingPatient = await _patientRepository.GetByEmailAsync(patientCreateDto.Email, cancellationToken);
            if (existingPatient != null)
            {
                _logger.LogWarning("Patient with email {Email} already exists", patientCreateDto.Email);
                throw new InvalidOperationException($"Patient with email {patientCreateDto.Email} already exists");
            }

            var patient = _mapper.Map<Patient>(patientCreateDto);
            patient.CreatedDate = DateTime.UtcNow;
            patient.IsActive = true;
            patient.CreatedBy = "System"; // Should be replaced with actual user context
            
            var createdPatient = await _patientRepository.AddAsync(patient, cancellationToken);
            
            var createdPatientDto = _mapper.Map<PatientDto>(createdPatient);
            _logger.LogInformation("Successfully created patient with ID: {PatientId}", createdPatient.PatientID);
            
            return createdPatientDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating patient");
            throw;
        }
    }

    public async Task<PatientDto> UpdateAsync(PatientUpdateDto patientUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (patientUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(patientUpdateDto));
            }

            _logger.LogInformation("Updating patient with ID: {PatientId}", patientUpdateDto.PatientId);
            
            var existingPatient = await _patientRepository.GetByIdAsync(patientUpdateDto.PatientId, cancellationToken);
            if (existingPatient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found for update", patientUpdateDto.PatientId);
                throw new KeyNotFoundException($"Patient with ID {patientUpdateDto.PatientId} not found");
            }

            // Check if email is being changed to one that already exists
            if (existingPatient.Email != patientUpdateDto.Email)
            {
                var emailExists = await _patientRepository.GetByEmailAsync(patientUpdateDto.Email, cancellationToken);
                if (emailExists != null)
                {
                    _logger.LogWarning("Patient with email {Email} already exists", patientUpdateDto.Email);
                    throw new InvalidOperationException($"Patient with email {patientUpdateDto.Email} already exists");
                }
            }

            _mapper.Map(patientUpdateDto, existingPatient);
            existingPatient.ModifiedDate = DateTime.UtcNow;
            existingPatient.ModifiedBy = "System"; // Should be replaced with actual user context
            
            var updatedPatient = await _patientRepository.UpdateAsync(existingPatient, cancellationToken);
            
            var updatedPatientDto = _mapper.Map<PatientDto>(updatedPatient);
            _logger.LogInformation("Successfully updated patient with ID: {PatientId}", patientUpdateDto.PatientId);
            
            return updatedPatientDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating patient with ID: {PatientId}", patientUpdateDto?.PatientId);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting patient with ID: {PatientId}", patientId);
            
            var existingPatient = await _patientRepository.GetByIdAsync(patientId, cancellationToken);
            if (existingPatient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found for deletion", patientId);
                return false;
            }

            var result = await _patientRepository.DeleteAsync(patientId, cancellationToken);
            
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
            _logger.LogError(ex, "Error occurred while deleting patient with ID: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<PatientDto>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            }

            _logger.LogInformation("Searching patients by name: {Name}", name);
            var patients = await _patientRepository.SearchAsync(name, cancellationToken);
            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);
            _logger.LogInformation("Found {Count} patients matching name: {Name}", patientDtos.Count(), name);
            return patientDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching patients by name: {Name}", name);
            throw;
        }
    }

    public async Task<PatientDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            _logger.LogInformation("Retrieving patient by email: {Email}", email);
            var patient = await _patientRepository.GetByEmailAsync(email, cancellationToken);
            
            if (patient == null)
            {
                _logger.LogWarning("Patient with email {Email} not found", email);
                return null;
            }

            var patientDto = _mapper.Map<PatientDto>(patient);
            _logger.LogInformation("Successfully retrieved patient with email: {Email}", email);
            return patientDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving patient by email: {Email}", email);
            throw;
        }
    }

    public async Task<IEnumerable<PatientDto>> GetByBloodGroupAsync(string bloodGroup, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(bloodGroup))
            {
                throw new ArgumentException("Blood group cannot be null or empty", nameof(bloodGroup));
            }

            _logger.LogInformation("Retrieving patients by blood group: {BloodGroup}", bloodGroup);
            var allPatients = await _patientRepository.GetAllAsync(cancellationToken);
            
            // Filter by blood group - this assumes Patient entity has a BloodGroup property
            // If not, this will need to be implemented in the repository
            var patients = allPatients.Where(p => p.IsActive);
            
            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients)
                .Where(p => p.BloodGroup.Equals(bloodGroup, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            _logger.LogInformation("Found {Count} patients with blood group: {BloodGroup}", patientDtos.Count(), bloodGroup);
            return patientDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving patients by blood group: {BloodGroup}", bloodGroup);
            throw;
        }
    }

    public async Task<IEnumerable<PatientDto>> GetRecentPatientsAsync(int count, CancellationToken cancellationToken = default)
    {
        try
        {
            if (count <= 0)
            {
                throw new ArgumentException("Count must be greater than zero", nameof(count));
            }

            _logger.LogInformation("Retrieving {Count} most recent patients", count);
            var allPatients = await _patientRepository.GetAllAsync(cancellationToken);
            
            var recentPatients = allPatients
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedDate)
                .Take(count)
                .ToList();
            
            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(recentPatients);
            _logger.LogInformation("Retrieved {Count} recent patients", patientDtos.Count());
            return patientDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving recent patients");
            throw;
        }
    }
}
