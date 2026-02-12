using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(IDepartmentRepository repository, IMapper mapper, ILogger<DepartmentService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all departments");
            var departments = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all departments");
            throw;
        }
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving department with ID: {DepartmentId}", id);
            var department = await _repository.GetByIdAsync(id, cancellationToken);
            return department != null ? _mapper.Map<DepartmentDto>(department) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department");
            throw;
        }
    }

    public async Task<DepartmentDto> CreateAsync(DepartmentCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new department");
            var department = _mapper.Map<Department>(dto);
            var created = await _repository.AddAsync(department, cancellationToken);
            return _mapper.Map<DepartmentDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating department");
            throw;
        }
    }

    public async Task UpdateAsync(int id, DepartmentUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating department with ID: {DepartmentId}", id);
            var department = await _repository.GetByIdAsync(id, cancellationToken);
            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found");
            }

            _mapper.Map(dto, department);
            await _repository.UpdateAsync(department, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department");
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting department with ID: {DepartmentId}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department");
            throw;
        }
    }
}
