using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

/// <summary>
/// Service implementation for Patient operations
/// </summary>
public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<PatientService> _logger;

    public PatientService(
        IPatientRepository repository,
        IMapper mapper,
        ILogger<PatientService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all patients");
            var patients = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all patients");
            throw;
        }
    }

    public async Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving patient with ID: {PatientId}", id);
            var patient = await _repository.GetByIdAsync(id, cancellationToken);
            return patient != null ? _mapper.Map<PatientDto>(patient) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient with ID: {PatientId}", id);
            throw;
        }
    }

    public async Task<PatientDto> CreateAsync(PatientCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new patient: {Email}", dto.Email);

            var existingPatient = await _repository.GetByEmailAsync(dto.Email, cancellationToken);
            if (existingPatient != null)
            {
                throw new InvalidOperationException("Patient with this email already exists");
            }

            var patient = _mapper.Map<Patient>(dto);
            patient.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            patient.CreatedBy = "System";

            var created = await _repository.AddAsync(patient, cancellationToken);
            return _mapper.Map<PatientDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient: {Email}", dto.Email);
            throw;
        }
    }

    public async Task UpdateAsync(int id, PatientUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating patient with ID: {PatientId}", id);

            var patient = await _repository.GetByIdAsync(id, cancellationToken);
            if (patient == null)
            {
                throw new KeyNotFoundException($"Patient with ID {id} not found");
            }

            _mapper.Map(dto, patient);
            patient.ModifiedBy = "System";
            patient.ModifiedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(patient, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient with ID: {PatientId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting patient with ID: {PatientId}", id);

            var exists = await _repository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                throw new KeyNotFoundException($"Patient with ID {id} not found");
            }

            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient with ID: {PatientId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<PatientDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching patients with term: {SearchTerm}", searchTerm);
            var patients = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching patients with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<(bool IsValid, int PatientId, string Message)> ValidateLoginAsync(
        string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating login for email: {Email}", email);

            var patient = await _repository.GetByEmailAsync(email, cancellationToken);
            if (patient == null)
            {
                return (false, 0, "Invalid email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, patient.Password))
            {
                return (false, 0, "Invalid email or password");
            }

            if (!patient.Status)
            {
                return (false, 0, "Account is inactive");
            }

            return (true, patient.PatientID, "Login successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating login for email: {Email}", email);
            throw;
        }
    }
}
