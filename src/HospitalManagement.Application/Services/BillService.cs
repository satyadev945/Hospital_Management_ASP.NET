using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

public class BillService : IBillService
{
    private readonly IBillRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<BillService> _logger;

    public BillService(IBillRepository repository, IMapper mapper, ILogger<BillService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<BillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all bills");
            var bills = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<BillDto>>(bills);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all bills");
            throw;
        }
    }

    public async Task<BillDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving bill with ID: {BillId}", id);
            var bill = await _repository.GetByIdAsync(id, cancellationToken);
            return bill != null ? _mapper.Map<BillDto>(bill) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bill");
            throw;
        }
    }

    public async Task<BillDto> CreateAsync(BillCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new bill");
            var bill = _mapper.Map<Bill>(dto);
            var created = await _repository.AddAsync(bill, cancellationToken);
            return _mapper.Map<BillDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bill");
            throw;
        }
    }

    public async Task UpdateAsync(int id, BillUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating bill with ID: {BillId}", id);
            var bill = await _repository.GetByIdAsync(id, cancellationToken);
            if (bill == null)
            {
                throw new KeyNotFoundException($"Bill with ID {id} not found");
            }

            _mapper.Map(dto, bill);
            await _repository.UpdateAsync(bill, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bill");
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting bill with ID: {BillId}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bill");
            throw;
        }
    }

    public async Task<IEnumerable<BillDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving bills for patient: {PatientId}", patientId);
            var bills = await _repository.GetByPatientIdAsync(patientId, cancellationToken);
            return _mapper.Map<IEnumerable<BillDto>>(bills);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient bills");
            throw;
        }
    }
}
