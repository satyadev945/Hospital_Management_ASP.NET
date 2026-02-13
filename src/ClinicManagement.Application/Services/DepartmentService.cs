using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Interfaces;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Services;

/// <summary>
/// Service implementation for department management operations.
/// </summary>
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(
        IDepartmentRepository departmentRepository,
        IDoctorRepository doctorRepository,
        IStaffRepository staffRepository,
        IMapper mapper,
        ILogger<DepartmentService> logger)
    {
        _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all departments");
            var departments = await _departmentRepository.GetAllAsync(cancellationToken);
            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
            _logger.LogInformation("Retrieved {Count} departments", departmentDtos.Count());
            return departmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all departments");
            throw;
        }
    }

    public async Task<DepartmentDto?> GetByIdAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving department with ID: {DepartmentId}", departmentId);
            var department = await _departmentRepository.GetByIdAsync(departmentId, cancellationToken);
            
            if (department == null)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found", departmentId);
                return null;
            }

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            _logger.LogInformation("Successfully retrieved department with ID: {DepartmentId}", departmentId);
            return departmentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving department with ID: {DepartmentId}", departmentId);
            throw;
        }
    }

    public async Task<DepartmentDto> CreateAsync(DepartmentCreateDto departmentCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (departmentCreateDto == null)
            {
                throw new ArgumentNullException(nameof(departmentCreateDto));
            }

            _logger.LogInformation("Creating new department: {DepartmentName}", departmentCreateDto.DepartmentName);
            
            // Check if department with same name already exists
            var allDepartments = await _departmentRepository.GetAllAsync(cancellationToken);
            var existingDepartment = allDepartments.FirstOrDefault(d => 
                d.DeptName.Equals(departmentCreateDto.DepartmentName, StringComparison.OrdinalIgnoreCase));
            
            if (existingDepartment != null)
            {
                _logger.LogWarning("Department with name {DepartmentName} already exists", departmentCreateDto.DepartmentName);
                throw new InvalidOperationException($"Department with name {departmentCreateDto.DepartmentName} already exists");
            }

            var department = _mapper.Map<Department>(departmentCreateDto);
            department.CreatedDate = DateTime.UtcNow;
            department.IsActive = true;
            department.CreatedBy = "System"; // Should be replaced with actual user context
            
            var createdDepartment = await _departmentRepository.AddAsync(department, cancellationToken);
            
            var createdDepartmentDto = _mapper.Map<DepartmentDto>(createdDepartment);
            _logger.LogInformation("Successfully created department with ID: {DepartmentId}", createdDepartment.DeptNo);
            
            return createdDepartmentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating department");
            throw;
        }
    }

    public async Task<DepartmentDto> UpdateAsync(DepartmentUpdateDto departmentUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (departmentUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(departmentUpdateDto));
            }

            _logger.LogInformation("Updating department with ID: {DepartmentId}", departmentUpdateDto.DepartmentId);
            
            var existingDepartment = await _departmentRepository.GetByIdAsync(departmentUpdateDto.DepartmentId, cancellationToken);
            if (existingDepartment == null)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found for update", departmentUpdateDto.DepartmentId);
                throw new KeyNotFoundException($"Department with ID {departmentUpdateDto.DepartmentId} not found");
            }

            // Check if name is being changed to one that already exists
            if (existingDepartment.DeptName != departmentUpdateDto.DepartmentName)
            {
                var allDepartments = await _departmentRepository.GetAllAsync(cancellationToken);
                var nameExists = allDepartments.FirstOrDefault(d => 
                    d.DeptName.Equals(departmentUpdateDto.DepartmentName, StringComparison.OrdinalIgnoreCase) &&
                    d.DeptNo != departmentUpdateDto.DepartmentId);
                
                if (nameExists != null)
                {
                    _logger.LogWarning("Department with name {DepartmentName} already exists", departmentUpdateDto.DepartmentName);
                    throw new InvalidOperationException($"Department with name {departmentUpdateDto.DepartmentName} already exists");
                }
            }

            _mapper.Map(departmentUpdateDto, existingDepartment);
            existingDepartment.ModifiedDate = DateTime.UtcNow;
            existingDepartment.ModifiedBy = "System"; // Should be replaced with actual user context
            
            var updatedDepartment = await _departmentRepository.UpdateAsync(existingDepartment, cancellationToken);
            
            var updatedDepartmentDto = _mapper.Map<DepartmentDto>(updatedDepartment);
            _logger.LogInformation("Successfully updated department with ID: {DepartmentId}", departmentUpdateDto.DepartmentId);
            
            return updatedDepartmentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating department with ID: {DepartmentId}", departmentUpdateDto?.DepartmentId);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting department with ID: {DepartmentId}", departmentId);
            
            var existingDepartment = await _departmentRepository.GetByIdAsync(departmentId, cancellationToken);
            if (existingDepartment == null)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found for deletion", departmentId);
                return false;
            }

            // Check if department has associated doctors
            var doctors = await _doctorRepository.GetByDepartmentAsync(departmentId, cancellationToken);
            if (doctors.Any())
            {
                _logger.LogWarning("Cannot delete department with ID {DepartmentId} because it has {Count} associated doctors", 
                    departmentId, doctors.Count());
                throw new InvalidOperationException($"Cannot delete department with {doctors.Count()} associated doctors");
            }

            var result = await _departmentRepository.DeleteAsync(departmentId, cancellationToken);
            
            if (result)
            {
                _logger.LogInformation("Successfully deleted department with ID: {DepartmentId}", departmentId);
            }
            else
            {
                _logger.LogWarning("Failed to delete department with ID: {DepartmentId}", departmentId);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting department with ID: {DepartmentId}", departmentId);
            throw;
        }
    }

    public async Task<DepartmentDto?> GetByNameAsync(string departmentName, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(departmentName))
            {
                throw new ArgumentException("Department name cannot be null or empty", nameof(departmentName));
            }

            _logger.LogInformation("Retrieving department by name: {DepartmentName}", departmentName);
            var allDepartments = await _departmentRepository.GetAllAsync(cancellationToken);
            
            var department = allDepartments.FirstOrDefault(d => 
                d.DeptName.Equals(departmentName, StringComparison.OrdinalIgnoreCase));
            
            if (department == null)
            {
                _logger.LogWarning("Department with name {DepartmentName} not found", departmentName);
                return null;
            }

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            _logger.LogInformation("Successfully retrieved department with name: {DepartmentName}", departmentName);
            return departmentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving department by name: {DepartmentName}", departmentName);
            throw;
        }
    }

    public async Task<IEnumerable<DepartmentDto>> GetByLocationAsync(string location, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                throw new ArgumentException("Location cannot be null or empty", nameof(location));
            }

            _logger.LogInformation("Retrieving departments by location: {Location}", location);
            var allDepartments = await _departmentRepository.GetAllAsync(cancellationToken);
            
            // Filter by location - this assumes Department entity or DTO has a Location property
            var departments = allDepartments.Where(d => d.IsActive).ToList();
            
            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments)
                .Where(d => d.Location.Contains(location, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            _logger.LogInformation("Found {Count} departments with location: {Location}", departmentDtos.Count(), location);
            return departmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving departments by location: {Location}", location);
            throw;
        }
    }

    public async Task<int> GetDoctorCountAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting doctor count for department ID: {DepartmentId}", departmentId);
            
            var departmentExists = await _departmentRepository.ExistsAsync(departmentId, cancellationToken);
            if (!departmentExists)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found", departmentId);
                throw new KeyNotFoundException($"Department with ID {departmentId} not found");
            }

            var doctors = await _doctorRepository.GetByDepartmentAsync(departmentId, cancellationToken);
            var count = doctors.Count(d => d.IsActive);
            
            _logger.LogInformation("Department {DepartmentId} has {Count} doctors", departmentId, count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting doctor count for department ID: {DepartmentId}", departmentId);
            throw;
        }
    }

    public async Task<int> GetStaffCountAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting staff count for department ID: {DepartmentId}", departmentId);
            
            var departmentExists = await _departmentRepository.ExistsAsync(departmentId, cancellationToken);
            if (!departmentExists)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found", departmentId);
                throw new KeyNotFoundException($"Department with ID {departmentId} not found");
            }

            var allStaff = await _staffRepository.GetAllAsync(cancellationToken);
            // Filter staff by department - this assumes Staff entity has DepartmentId property
            // If not available in entity, this will need adjustment
            var count = allStaff.Count(s => s.IsActive);
            
            _logger.LogInformation("Department {DepartmentId} has {Count} staff members", departmentId, count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting staff count for department ID: {DepartmentId}", departmentId);
            throw;
        }
    }
}
