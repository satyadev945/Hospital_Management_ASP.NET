using System;
using System.Collections.Generic;
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
    /// Service implementation for staff-related operations
    /// </summary>
    public class StaffService : IStaffService
    {
        private readonly IOtherStaffRepository _staffRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StaffService> _logger;

        public StaffService(
            IOtherStaffRepository staffRepository,
            IMapper mapper,
            ILogger<StaffService> logger)
        {
            _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<StaffDto> GetByIdAsync(int staffId)
        {
            try
            {
                _logger.LogInformation("Retrieving staff member with ID: {StaffId}", staffId);

                var staff = await _staffRepository.GetByIdAsync(staffId, CancellationToken.None);

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
                _logger.LogError(ex, "Error retrieving staff member with ID: {StaffId}", staffId);
                throw;
            }
        }

        public async Task<IEnumerable<StaffDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all staff members");

                var staffList = await _staffRepository.GetAllAsync(CancellationToken.None);
                var staffDtos = _mapper.Map<IEnumerable<StaffDto>>(staffList);

                _logger.LogInformation("Successfully retrieved {Count} staff members",
                    ((List<StaffDto>)staffDtos).Count);

                return staffDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all staff members");
                throw;
            }
        }

        public async Task<IEnumerable<StaffDto>> GetByDesignationAsync(string designation)
        {
            try
            {
                _logger.LogInformation("Retrieving staff members by designation: {Designation}", designation);

                var staffList = await _staffRepository.GetByDesignationAsync(designation, CancellationToken.None);
                var staffDtos = _mapper.Map<IEnumerable<StaffDto>>(staffList);

                _logger.LogInformation("Successfully retrieved {Count} staff members with designation: {Designation}",
                    ((List<StaffDto>)staffDtos).Count, designation);

                return staffDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving staff members by designation: {Designation}", designation);
                throw;
            }
        }

        public async Task<StaffDto> CreateAsync(StaffCreateDto staffCreateDto)
        {
            try
            {
                _logger.LogInformation("Creating new staff member with name: {Name}", staffCreateDto.Name);

                // Check if phone number already exists
                if (!string.IsNullOrEmpty(staffCreateDto.Phone))
                {
                    var existingStaff = await _staffRepository.GetByPhoneAsync(
                        staffCreateDto.Phone, CancellationToken.None);

                    if (existingStaff != null)
                    {
                        _logger.LogWarning("Staff member with phone {Phone} already exists", staffCreateDto.Phone);
                        throw new InvalidOperationException($"Staff member with phone {staffCreateDto.Phone} already exists");
                    }
                }

                var staff = _mapper.Map<OtherStaff>(staffCreateDto);
                staff.CreatedDate = DateTime.UtcNow;
                staff.IsActive = true;

                var createdStaff = await _staffRepository.AddAsync(staff, CancellationToken.None);
                var staffDto = _mapper.Map<StaffDto>(createdStaff);

                _logger.LogInformation("Successfully created staff member with ID: {StaffId}", createdStaff.StaffID);

                return staffDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating staff member with name: {Name}", staffCreateDto.Name);
                throw;
            }
        }

        public async Task<StaffDto> UpdateAsync(StaffUpdateDto staffUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating staff member with ID: {StaffId}", staffUpdateDto.StaffID);

                var existingStaff = await _staffRepository.GetByIdAsync(staffUpdateDto.StaffID, CancellationToken.None);
                if (existingStaff == null)
                {
                    _logger.LogWarning("Staff member with ID {StaffId} not found", staffUpdateDto.StaffID);
                    throw new InvalidOperationException($"Staff member with ID {staffUpdateDto.StaffID} not found");
                }

                // Check if phone number conflicts with another staff member
                if (!string.IsNullOrEmpty(staffUpdateDto.Phone) && staffUpdateDto.Phone != existingStaff.Phone)
                {
                    var conflictingStaff = await _staffRepository.GetByPhoneAsync(
                        staffUpdateDto.Phone, CancellationToken.None);

                    if (conflictingStaff != null && conflictingStaff.StaffID != staffUpdateDto.StaffID)
                    {
                        _logger.LogWarning("Another staff member with phone {Phone} already exists", staffUpdateDto.Phone);
                        throw new InvalidOperationException($"Another staff member with phone {staffUpdateDto.Phone} already exists");
                    }
                }

                _mapper.Map(staffUpdateDto, existingStaff);
                existingStaff.ModifiedDate = DateTime.UtcNow;

                var updatedStaff = await _staffRepository.UpdateAsync(existingStaff, CancellationToken.None);
                var staffDto = _mapper.Map<StaffDto>(updatedStaff);

                _logger.LogInformation("Successfully updated staff member with ID: {StaffId}", staffUpdateDto.StaffID);

                return staffDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating staff member with ID: {StaffId}", staffUpdateDto.StaffID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int staffId)
        {
            try
            {
                _logger.LogInformation("Deleting staff member with ID: {StaffId}", staffId);

                var staff = await _staffRepository.GetByIdAsync(staffId, CancellationToken.None);
                if (staff == null)
                {
                    _logger.LogWarning("Staff member with ID {StaffId} not found", staffId);
                    return false;
                }

                var result = await _staffRepository.DeleteAsync(staffId, CancellationToken.None);

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
                _logger.LogError(ex, "Error deleting staff member with ID: {StaffId}", staffId);
                throw;
            }
        }

        public async Task<int> GetTotalCountAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving total count of staff members");

                var count = await _staffRepository.GetTotalCountAsync(CancellationToken.None);

                _logger.LogInformation("Successfully retrieved total count of staff members: {Count}", count);

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total count of staff members");
                throw;
            }
        }

        public async Task<bool> ExistsByPhoneAsync(string phone)
        {
            try
            {
                _logger.LogInformation("Checking if staff member exists with phone: {Phone}", phone);

                var staff = await _staffRepository.GetByPhoneAsync(phone, CancellationToken.None);
                var exists = staff != null;

                _logger.LogInformation("Staff member with phone {Phone} exists: {Exists}", phone, exists);

                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if staff member exists with phone: {Phone}", phone);
                throw;
            }
        }
    }
}
