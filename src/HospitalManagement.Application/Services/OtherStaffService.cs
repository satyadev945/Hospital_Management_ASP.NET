using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

public class OtherStaffService : IOtherStaffService
{
    private readonly IOtherStaffRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<OtherStaffService> _logger;

    public OtherStaffService(IOtherStaffRepository repository, IMapper mapper, ILogger<OtherStaffService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<OtherStaffDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all staff members");
            var staff = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<OtherStaffDto>>(staff);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all staff members");
            throw;
        }
    }

    public async Task<OtherStaffDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving staff member with ID: {StaffId}", id);
            var staff = await _repository.GetByIdAsync(id, cancellationToken);
            return staff != null ? _mapper.Map<OtherStaffDto>(staff) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff member");
            throw;
        }
    }

    public async Task<OtherStaffDto> CreateAsync(OtherStaffCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new staff member");
            var staff = _mapper.Map<OtherStaff>(dto);
            var created = await _repository.AddAsync(staff, cancellationToken);
            return _mapper.Map<OtherStaffDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating staff member");
            throw;
        }
    }

    public async Task UpdateAsync(int id, OtherStaffUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating staff member with ID: {StaffId}", id);
            var staff = await _repository.GetByIdAsync(id, cancellationToken);
            if (staff == null)
            {
                throw new KeyNotFoundException($"Staff member with ID {id} not found");
            }

            _mapper.Map(dto, staff);
            await _repository.UpdateAsync(staff, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating staff member");
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting staff member with ID: {StaffId}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting staff member");
            throw;
        }
    }

    public async Task<IEnumerable<OtherStaffDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching staff members with term: {SearchTerm}", searchTerm);
            var staff = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<OtherStaffDto>>(staff);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching staff members");
            throw;
        }
    }
}
