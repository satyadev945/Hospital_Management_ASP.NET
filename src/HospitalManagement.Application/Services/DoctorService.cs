using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DoctorService> _logger;

    public DoctorService(IDoctorRepository repository, IMapper mapper, ILogger<DoctorService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all doctors");
            var doctors = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all doctors");
            throw;
        }
    }

    public async Task<DoctorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctor with ID: {DoctorId}", id);
            var doctor = await _repository.GetByIdAsync(id, cancellationToken);
            return doctor != null ? _mapper.Map<DoctorDto>(doctor) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor with ID: {DoctorId}", id);
            throw;
        }
    }

    public async Task<DoctorDto> CreateAsync(DoctorCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new doctor: {Email}", dto.Email);

            var existingDoctor = await _repository.GetByEmailAsync(dto.Email, cancellationToken);
            if (existingDoctor != null)
            {
                throw new InvalidOperationException("Doctor with this email already exists");
            }

            var doctor = _mapper.Map<Doctor>(dto);
            doctor.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            doctor.CreatedBy = "Admin";

            var created = await _repository.AddAsync(doctor, cancellationToken);
            return _mapper.Map<DoctorDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating doctor: {Email}", dto.Email);
            throw;
        }
    }

    public async Task UpdateAsync(int id, DoctorUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating doctor with ID: {DoctorId}", id);

            var doctor = await _repository.GetByIdAsync(id, cancellationToken);
            if (doctor == null)
            {
                throw new KeyNotFoundException($"Doctor with ID {id} not found");
            }

            _mapper.Map(dto, doctor);
            doctor.ModifiedBy = "Admin";
            doctor.ModifiedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(doctor, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating doctor with ID: {DoctorId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting doctor with ID: {DoctorId}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting doctor with ID: {DoctorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DoctorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching doctors with term: {SearchTerm}", searchTerm);
            var doctors = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching doctors");
            throw;
        }
    }

    public async Task<IEnumerable<DoctorDto>> GetByDepartmentAsync(int deptNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctors for department: {DeptNo}", deptNo);
            var doctors = await _repository.GetByDepartmentAsync(deptNo, cancellationToken);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctors for department");
            throw;
        }
    }

    public async Task<(bool IsValid, int DoctorId, string Message)> ValidateLoginAsync(
        string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating doctor login for email: {Email}", email);

            var doctor = await _repository.GetByEmailAsync(email, cancellationToken);
            if (doctor == null)
            {
                return (false, 0, "Invalid email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, doctor.Password))
            {
                return (false, 0, "Invalid email or password");
            }

            if (!doctor.Status)
            {
                return (false, 0, "Account is inactive");
            }

            return (true, doctor.DoctorID, "Login successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating doctor login");
            throw;
        }
    }
}
