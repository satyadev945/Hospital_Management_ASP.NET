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
    /// Service implementation for department-related operations
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper,
            ILogger<DepartmentService> logger)
        {
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DepartmentDto> GetByIdAsync(int deptNo)
        {
            try
            {
                _logger.LogInformation("Retrieving department with ID: {DeptNo}", deptNo);

                var department = await _departmentRepository.GetByIdAsync(deptNo, CancellationToken.None);

                if (department == null)
                {
                    _logger.LogWarning("Department with ID {DeptNo} not found", deptNo);
                    return null;
                }

                var departmentDto = _mapper.Map<DepartmentDto>(department);
                _logger.LogInformation("Successfully retrieved department with ID: {DeptNo}", deptNo);

                return departmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department with ID: {DeptNo}", deptNo);
                throw;
            }
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all departments");

                var departments = await _departmentRepository.GetAllAsync(CancellationToken.None);
                var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);

                _logger.LogInformation("Successfully retrieved {Count} departments",
                    ((List<DepartmentDto>)departmentDtos).Count);

                return departmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all departments");
                throw;
            }
        }

        public async Task<DepartmentDto> GetByNameAsync(string departmentName)
        {
            try
            {
                _logger.LogInformation("Retrieving department with name: {DepartmentName}", departmentName);

                var department = await _departmentRepository.GetByNameAsync(departmentName, CancellationToken.None);

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
                _logger.LogError(ex, "Error retrieving department with name: {DepartmentName}", departmentName);
                throw;
            }
        }

        public async Task<DepartmentDto> CreateAsync(DepartmentCreateDto departmentCreateDto)
        {
            try
            {
                _logger.LogInformation("Creating new department with name: {DepartmentName}",
                    departmentCreateDto.DepartmentName);

                // Check if department name already exists
                var existingDepartment = await _departmentRepository.GetByNameAsync(
                    departmentCreateDto.DepartmentName, CancellationToken.None);

                if (existingDepartment != null)
                {
                    _logger.LogWarning("Department with name {DepartmentName} already exists",
                        departmentCreateDto.DepartmentName);
                    throw new InvalidOperationException(
                        $"Department with name {departmentCreateDto.DepartmentName} already exists");
                }

                var department = _mapper.Map<Department>(departmentCreateDto);
                department.CreatedDate = DateTime.UtcNow;
                department.IsActive = true;

                var createdDepartment = await _departmentRepository.AddAsync(department, CancellationToken.None);
                var departmentDto = _mapper.Map<DepartmentDto>(createdDepartment);

                _logger.LogInformation("Successfully created department with ID: {DeptNo}", createdDepartment.DeptNo);

                return departmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department with name: {DepartmentName}",
                    departmentCreateDto.DepartmentName);
                throw;
            }
        }

        public async Task<DepartmentDto> UpdateAsync(DepartmentUpdateDto departmentUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating department with ID: {DeptNo}", departmentUpdateDto.DeptNo);

                var existingDepartment = await _departmentRepository.GetByIdAsync(
                    departmentUpdateDto.DeptNo, CancellationToken.None);

                if (existingDepartment == null)
                {
                    _logger.LogWarning("Department with ID {DeptNo} not found", departmentUpdateDto.DeptNo);
                    throw new InvalidOperationException($"Department with ID {departmentUpdateDto.DeptNo} not found");
                }

                // Check if new name conflicts with existing department (if name is being changed)
                if (!string.Equals(existingDepartment.DepartmentName, departmentUpdateDto.DepartmentName,
                    StringComparison.OrdinalIgnoreCase))
                {
                    var conflictingDepartment = await _departmentRepository.GetByNameAsync(
                        departmentUpdateDto.DepartmentName, CancellationToken.None);

                    if (conflictingDepartment != null && conflictingDepartment.DeptNo != departmentUpdateDto.DeptNo)
                    {
                        _logger.LogWarning("Department with name {DepartmentName} already exists",
                            departmentUpdateDto.DepartmentName);
                        throw new InvalidOperationException(
                            $"Department with name {departmentUpdateDto.DepartmentName} already exists");
                    }
                }

                _mapper.Map(departmentUpdateDto, existingDepartment);
                existingDepartment.ModifiedDate = DateTime.UtcNow;

                var updatedDepartment = await _departmentRepository.UpdateAsync(existingDepartment, CancellationToken.None);
                var departmentDto = _mapper.Map<DepartmentDto>(updatedDepartment);

                _logger.LogInformation("Successfully updated department with ID: {DeptNo}", departmentUpdateDto.DeptNo);

                return departmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department with ID: {DeptNo}", departmentUpdateDto.DeptNo);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int deptNo)
        {
            try
            {
                _logger.LogInformation("Deleting department with ID: {DeptNo}", deptNo);

                var department = await _departmentRepository.GetByIdAsync(deptNo, CancellationToken.None);
                if (department == null)
                {
                    _logger.LogWarning("Department with ID {DeptNo} not found", deptNo);
                    return false;
                }

                // Check if department has active doctors
                var doctors = await _doctorRepository.GetByDepartmentAsync(deptNo, CancellationToken.None);
                var doctorsList = new List<Doctor>(doctors);

                if (doctorsList.Count > 0)
                {
                    _logger.LogWarning("Cannot delete department with ID {DeptNo} as it has {Count} doctors assigned",
                        deptNo, doctorsList.Count);
                    throw new InvalidOperationException(
                        $"Cannot delete department with {doctorsList.Count} doctors assigned. Please reassign or remove doctors first.");
                }

                var result = await _departmentRepository.DeleteAsync(deptNo, CancellationToken.None);

                if (result)
                {
                    _logger.LogInformation("Successfully deleted department with ID: {DeptNo}", deptNo);
                }
                else
                {
                    _logger.LogWarning("Failed to delete department with ID: {DeptNo}", deptNo);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department with ID: {DeptNo}", deptNo);
                throw;
            }
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartmentInfoAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving department information with doctor counts");

                var departments = await _departmentRepository.GetAllWithDoctorCountsAsync(CancellationToken.None);
                var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);

                _logger.LogInformation("Successfully retrieved {Count} departments with doctor counts",
                    ((List<DepartmentDto>)departmentDtos).Count);

                return departmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department information with doctor counts");
                throw;
            }
        }

        public async Task<IEnumerable<DoctorDto>> GetDepartmentDoctorsAsync(string departmentName)
        {
            try
            {
                _logger.LogInformation("Retrieving doctors for department: {DepartmentName}", departmentName);

                var doctors = await _doctorRepository.GetByDepartmentNameAsync(departmentName, CancellationToken.None);
                var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);

                _logger.LogInformation("Successfully retrieved {Count} doctors for department: {DepartmentName}",
                    ((List<DoctorDto>)doctorDtos).Count, departmentName);

                return doctorDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctors for department: {DepartmentName}", departmentName);
                throw;
            }
        }

        public async Task<bool> ExistsByNameAsync(string departmentName)
        {
            try
            {
                _logger.LogInformation("Checking if department exists with name: {DepartmentName}", departmentName);

                var department = await _departmentRepository.GetByNameAsync(departmentName, CancellationToken.None);
                var exists = department != null;

                _logger.LogInformation("Department with name {DepartmentName} exists: {Exists}",
                    departmentName, exists);

                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if department exists with name: {DepartmentName}",
                    departmentName);
                throw;
            }
        }
    }
}
