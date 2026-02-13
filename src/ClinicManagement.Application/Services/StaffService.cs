using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Interfaces;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Services;

/// <summary>
/// Service implementation for staff management operations.
/// </summary>
public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<StaffService> _logger;

    public StaffService(
        IStaffRepository staffRepository,
        IDepartmentRepository departmentRepository,
        IMapper mapper,
        ILogger<StaffService> logger)
    {
        _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
        _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<StaffDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all staff members");
            var staffMembers = await _staffRepository.GetAllAsync(cancellationToken);
            var staffDtos = _mapper.Map<IEnumerable<StaffDto>>(staffMembers);
            _logger.LogInformation("Retrieved {Count} staff members", staffDtos.Count());
            return staffDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all staff members");
            throw;
        }
    }

    public async Task<StaffDto?> GetByIdAsync(int staffId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving staff member with ID: {StaffId}", staffId);
            var staff = await _staffRepository.GetByIdAsync(staffId, cancellationToken);
            
            if (staff == null)
            {
                _logger.LogWarning("Staff member with ID {StaffId} not found", staffId);
                return null;
            }

            var staffDto = _mapper.Map<StaffDto>(staff);
            _logger.LogInformation("Successfully retrieved staff member with ID: {StaffId}", staffId);
            return staffDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving staff member with ID: {StaffId}", staffId);
            throw;
        }
    }

    public async Task<StaffDto> CreateAsync(StaffCreateDto staffCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (staffCreateDto == null)
            {
                throw new ArgumentNullException(nameof(staffCreateDto));
            }

            _logger.LogInformation("Creating new staff member: {FirstName} {LastName}", 
                staffCreateDto.FirstName, staffCreateDto.LastName);
            
            // Validate department exists
            var departmentExists = await _departmentRepository.ExistsAsync(staffCreateDto.DepartmentId, cancellationToken);
            if (!departmentExists)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found", staffCreateDto.DepartmentId);
                throw new KeyNotFoundException($"Department with ID {staffCreateDto.DepartmentId} not found");
            }

            // Check if staff with same email already exists (if email is available in Staff entity)
            // Note: This check is optional and depends on whether Staff entity has an Email property
            var allStaff = await _staffRepository.GetAllAsync(cancellationToken);
            var existingStaffWithEmail = allStaff.FirstOrDefault(s => 
                s.IsActive); // Add email check if Staff entity has Email property
            
            var staff = _mapper.Map<Staff>(staffCreateDto);
            staff.CreatedDate = DateTime.UtcNow;
            staff.IsActive = true;
            staff.CreatedBy = "System"; // Should be replaced with actual user context
            
            var createdStaff = await _staffRepository.AddAsync(staff, cancellationToken);
            
            var createdStaffDto = _mapper.Map<StaffDto>(createdStaff);
            _logger.LogInformation("Successfully created staff member with ID: {StaffId}", createdStaff.StaffID);
            
            return createdStaffDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating staff member");
            throw;
        }
    }

    public async Task<StaffDto> UpdateAsync(StaffUpdateDto staffUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (staffUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(staffUpdateDto));
            }

            _logger.LogInformation("Updating staff member with ID: {StaffId}", staffUpdateDto.StaffId);
            
            var existingStaff = await _staffRepository.GetByIdAsync(staffUpdateDto.StaffId, cancellationToken);
            if (existingStaff == null)
            {
                _logger.LogWarning("Staff member with ID {StaffId} not found for update", staffUpdateDto.StaffId);
                throw new KeyNotFoundException($"Staff member with ID {staffUpdateDto.StaffId} not found");
            }

            // Validate department exists
            var departmentExists = await _departmentRepository.ExistsAsync(staffUpdateDto.DepartmentId, cancellationToken);
            if (!departmentExists)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found", staffUpdateDto.DepartmentId);
                throw new KeyNotFoundException($"Department with ID {staffUpdateDto.DepartmentId} not found");
            }

            _mapper.Map(staffUpdateDto, existingStaff);
            existingStaff.ModifiedDate = DateTime.UtcNow;
            existingStaff.ModifiedBy = "System"; // Should be replaced with actual user context
            
            var updatedStaff = await _staffRepository.UpdateAsync(existingStaff, cancellationToken);
            
            var updatedStaffDto = _mapper.Map<StaffDto>(updatedStaff);
            _logger.LogInformation("Successfully updated staff member with ID: {StaffId}", staffUpdateDto.StaffId);
            
            return updatedStaffDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating staff member with ID: {StaffId}", staffUpdateDto?.StaffId);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int staffId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting staff member with ID: {StaffId}", staffId);
            
            var existingStaff = await _staffRepository.GetByIdAsync(staffId, cancellationToken);
            if (existingStaff == null)
            {
                _logger.LogWarning("Staff member with ID {StaffId} not found for deletion", staffId);
                return false;
            }

            var result = await _staffRepository.DeleteAsync(staffId, cancellationToken);
            
            if (result)
            {
                _logger.LogInformation("Successfully deleted staff member with ID: {StaffId}", staffId);
            }
            else
            {
                _logger.LogWarning("Failed to delete staff member with ID: {StaffId}", staffId);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting staff member with ID: {StaffId}", staffId);
            throw;
        }
    }

    public async Task<IEnumerable<StaffDto>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving staff members for department ID: {DepartmentId}", departmentId);
            var allStaff = await _staffRepository.GetAllAsync(cancellationToken);
            
            // Filter by department - this assumes Staff entity has DepartmentId property
            // If not, this will need to be implemented differently
            var staffMembers = allStaff
                .Where(s => s.IsActive)
                .ToList();
            
            // Note: If Staff entity doesn't have DepartmentId, filtering by department needs to be added
            var staffDtos = _mapper.Map<IEnumerable<StaffDto>>(staffMembers)
                .Where(s => s.DepartmentId == departmentId)
                .ToList();
            
            _logger.LogInformation("Retrieved {Count} staff members for department ID: {DepartmentId}", 
                staffDtos.Count(), departmentId);
            return staffDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving staff members for department ID: {DepartmentId}", departmentId);
            throw;
        }
    }

    public async Task<IEnumerable<StaffDto>> GetByPositionAsync(string position, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(position))
            {
                throw new ArgumentException("Position cannot be null or empty", nameof(position));
            }

            _logger.LogInformation("Retrieving staff members by position: {Position}", position);
            var allStaff = await _staffRepository.GetAllAsync(cancellationToken);
            
            // Filter by position - using Designation field from Staff entity
            var staffMembers = allStaff
                .Where(s => s.IsActive && 
                           s.Designation.Equals(position, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            var staffDtos = _mapper.Map<IEnumerable<StaffDto>>(staffMembers);
            _logger.LogInformation("Found {Count} staff members with position: {Position}", staffDtos.Count(), position);
            return staffDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving staff members by position: {Position}", position);
            throw;
        }
    }

    public async Task<IEnumerable<StaffDto>> GetByShiftAsync(string shift, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(shift))
            {
                throw new ArgumentException("Shift cannot be null or empty", nameof(shift));
            }

            _logger.LogInformation("Retrieving staff members by shift: {Shift}", shift);
            var allStaff = await _staffRepository.GetAllAsync(cancellationToken);
            
            // Filter by shift - this assumes Staff entity has a Shift property
            // If not, filtering will be done at DTO level after mapping
            var staffMembers = allStaff
                .Where(s => s.IsActive)
                .ToList();
            
            var staffDtos = _mapper.Map<IEnumerable<StaffDto>>(staffMembers)
                .Where(s => s.Shift.Equals(shift, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            _logger.LogInformation("Found {Count} staff members with shift: {Shift}", staffDtos.Count(), shift);
            return staffDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving staff members by shift: {Shift}", shift);
            throw;
        }
    }

    public async Task<StaffDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            _logger.LogInformation("Retrieving staff member by email: {Email}", email);
            var allStaff = await _staffRepository.GetAllAsync(cancellationToken);
            
            // Filter by email - this assumes Staff entity has an Email property or we filter at DTO level
            var staffMembers = allStaff.Where(s => s.IsActive).ToList();
            
            var staffDtos = _mapper.Map<IEnumerable<StaffDto>>(staffMembers);
            var staffDto = staffDtos.FirstOrDefault(s => 
                s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            
            if (staffDto == null)
            {
                _logger.LogWarning("Staff member with email {Email} not found", email);
                return null;
            }

            _logger.LogInformation("Successfully retrieved staff member with email: {Email}", email);
            return staffDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving staff member by email: {Email}", email);
            throw;
        }
    }

    public async Task<IEnumerable<StaffDto>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            }

            _logger.LogInformation("Searching staff members by name: {Name}", name);
            var staffMembers = await _staffRepository.SearchAsync(name, cancellationToken);
            var staffDtos = _mapper.Map<IEnumerable<StaffDto>>(staffMembers);
            _logger.LogInformation("Found {Count} staff members matching name: {Name}", staffDtos.Count(), name);
            return staffDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching staff members by name: {Name}", name);
            throw;
        }
    }
}
