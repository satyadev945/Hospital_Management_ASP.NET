using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Interfaces;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Services;

/// <summary>
/// Service implementation for bill management operations.
/// </summary>
public class BillService : IBillService
{
    private readonly IBillRepository _billRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BillService> _logger;

    public BillService(
        IBillRepository billRepository,
        IPatientRepository patientRepository,
        IAppointmentRepository appointmentRepository,
        IMapper mapper,
        ILogger<BillService> logger)
    {
        _billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<BillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all bills");
            var bills = await _billRepository.GetAllAsync(cancellationToken);
            var billDtos = _mapper.Map<IEnumerable<BillDto>>(bills);
            _logger.LogInformation("Retrieved {Count} bills", billDtos.Count());
            return billDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all bills");
            throw;
        }
    }

    public async Task<BillDto?> GetByIdAsync(int billId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving bill with ID: {BillId}", billId);
            var bill = await _billRepository.GetByIdAsync(billId, cancellationToken);
            
            if (bill == null)
            {
                _logger.LogWarning("Bill with ID {BillId} not found", billId);
                return null;
            }

            var billDto = _mapper.Map<BillDto>(bill);
            _logger.LogInformation("Successfully retrieved bill with ID: {BillId}", billId);
            return billDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bill with ID: {BillId}", billId);
            throw;
        }
    }

    public async Task<BillDto> CreateAsync(BillCreateDto billCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (billCreateDto == null)
            {
                throw new ArgumentNullException(nameof(billCreateDto));
            }

            _logger.LogInformation("Creating new bill for patient ID: {PatientId}", billCreateDto.PatientId);
            
            // Validate patient exists
            var patientExists = await _patientRepository.ExistsAsync(billCreateDto.PatientId, cancellationToken);
            if (!patientExists)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found", billCreateDto.PatientId);
                throw new KeyNotFoundException($"Patient with ID {billCreateDto.PatientId} not found");
            }

            // Validate appointment exists if provided
            if (billCreateDto.AppointmentId.HasValue)
            {
                var appointment = await _appointmentRepository.GetByIdAsync(billCreateDto.AppointmentId.Value, cancellationToken);
                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found", billCreateDto.AppointmentId.Value);
                    throw new KeyNotFoundException($"Appointment with ID {billCreateDto.AppointmentId.Value} not found");
                }
            }

            var bill = _mapper.Map<Bill>(billCreateDto);
            bill.BillDate = DateTime.UtcNow;
            bill.CreatedDate = DateTime.UtcNow;
            bill.IsActive = true;
            bill.IsPaid = billCreateDto.PaidAmount >= billCreateDto.TotalAmount;
            bill.CreatedBy = "System"; // Should be replaced with actual user context
            
            var createdBill = await _billRepository.AddAsync(bill, cancellationToken);
            
            var createdBillDto = _mapper.Map<BillDto>(createdBill);
            _logger.LogInformation("Successfully created bill with ID: {BillId}", createdBill.BillID);
            
            return createdBillDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating bill");
            throw;
        }
    }

    public async Task<BillDto> UpdateAsync(BillUpdateDto billUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (billUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(billUpdateDto));
            }

            _logger.LogInformation("Updating bill with ID: {BillId}", billUpdateDto.BillId);
            
            var existingBill = await _billRepository.GetByIdAsync(billUpdateDto.BillId, cancellationToken);
            if (existingBill == null)
            {
                _logger.LogWarning("Bill with ID {BillId} not found for update", billUpdateDto.BillId);
                throw new KeyNotFoundException($"Bill with ID {billUpdateDto.BillId} not found");
            }

            // Validate patient exists
            var patientExists = await _patientRepository.ExistsAsync(billUpdateDto.PatientId, cancellationToken);
            if (!patientExists)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found", billUpdateDto.PatientId);
                throw new KeyNotFoundException($"Patient with ID {billUpdateDto.PatientId} not found");
            }

            // Validate appointment exists if provided
            if (billUpdateDto.AppointmentId.HasValue)
            {
                var appointment = await _appointmentRepository.GetByIdAsync(billUpdateDto.AppointmentId.Value, cancellationToken);
                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found", billUpdateDto.AppointmentId.Value);
                    throw new KeyNotFoundException($"Appointment with ID {billUpdateDto.AppointmentId.Value} not found");
                }
            }

            _mapper.Map(billUpdateDto, existingBill);
            existingBill.ModifiedDate = DateTime.UtcNow;
            existingBill.ModifiedBy = "System"; // Should be replaced with actual user context
            existingBill.IsPaid = billUpdateDto.PaidAmount >= billUpdateDto.TotalAmount;
            
            var updatedBill = await _billRepository.UpdateAsync(existingBill, cancellationToken);
            
            var updatedBillDto = _mapper.Map<BillDto>(updatedBill);
            _logger.LogInformation("Successfully updated bill with ID: {BillId}", billUpdateDto.BillId);
            
            return updatedBillDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating bill with ID: {BillId}", billUpdateDto?.BillId);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int billId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting bill with ID: {BillId}", billId);
            
            var existingBill = await _billRepository.GetByIdAsync(billId, cancellationToken);
            if (existingBill == null)
            {
                _logger.LogWarning("Bill with ID {BillId} not found for deletion", billId);
                return false;
            }

            var result = await _billRepository.DeleteAsync(billId, cancellationToken);
            
            if (result)
            {
                _logger.LogInformation("Successfully deleted bill with ID: {BillId}", billId);
            }
            else
            {
                _logger.LogWarning("Failed to delete bill with ID: {BillId}", billId);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting bill with ID: {BillId}", billId);
            throw;
        }
    }

    public async Task<IEnumerable<BillDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving bills for patient ID: {PatientId}", patientId);
            var bills = await _billRepository.GetByPatientIdAsync(patientId, cancellationToken);
            var billDtos = _mapper.Map<IEnumerable<BillDto>>(bills);
            _logger.LogInformation("Retrieved {Count} bills for patient ID: {PatientId}", billDtos.Count(), patientId);
            return billDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bills for patient ID: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<BillDto>> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving bills for appointment ID: {AppointmentId}", appointmentId);
            var bill = await _billRepository.GetByAppointmentIdAsync(appointmentId, cancellationToken);
            
            var bills = bill != null ? new List<Bill> { bill } : new List<Bill>();
            var billDtos = _mapper.Map<IEnumerable<BillDto>>(bills);
            
            _logger.LogInformation("Retrieved {Count} bills for appointment ID: {AppointmentId}", billDtos.Count(), appointmentId);
            return billDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bills for appointment ID: {AppointmentId}", appointmentId);
            throw;
        }
    }

    public async Task<IEnumerable<BillDto>> GetByPaymentStatusAsync(string paymentStatus, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(paymentStatus))
            {
                throw new ArgumentException("Payment status cannot be null or empty", nameof(paymentStatus));
            }

            _logger.LogInformation("Retrieving bills by payment status: {PaymentStatus}", paymentStatus);
            var allBills = await _billRepository.GetAllAsync(cancellationToken);
            
            // Map payment status string to IsPaid boolean
            var isPaid = paymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase);
            
            var bills = allBills
                .Where(b => b.IsActive && b.IsPaid == isPaid)
                .OrderByDescending(b => b.BillDate)
                .ToList();
            
            var billDtos = _mapper.Map<IEnumerable<BillDto>>(bills);
            _logger.LogInformation("Retrieved {Count} bills with payment status: {PaymentStatus}", billDtos.Count(), paymentStatus);
            return billDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bills by payment status: {PaymentStatus}", paymentStatus);
            throw;
        }
    }

    public async Task<IEnumerable<BillDto>> GetPendingBillsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving pending bills");
            var bills = await _billRepository.GetUnpaidAsync(cancellationToken);
            var billDtos = _mapper.Map<IEnumerable<BillDto>>(bills);
            _logger.LogInformation("Retrieved {Count} pending bills", billDtos.Count());
            return billDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving pending bills");
            throw;
        }
    }

    public async Task<IEnumerable<BillDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be greater than end date");
            }

            _logger.LogInformation("Retrieving bills from {StartDate} to {EndDate}", 
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            
            var allBills = await _billRepository.GetAllAsync(cancellationToken);
            
            var bills = allBills
                .Where(b => b.IsActive && 
                           b.BillDate.Date >= startDate.Date && 
                           b.BillDate.Date <= endDate.Date)
                .OrderByDescending(b => b.BillDate)
                .ToList();
            
            var billDtos = _mapper.Map<IEnumerable<BillDto>>(bills);
            _logger.LogInformation("Retrieved {Count} bills for date range", billDtos.Count());
            return billDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bills for date range");
            throw;
        }
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Calculating total revenue");
            var allBills = await _billRepository.GetAllAsync(cancellationToken);
            
            var bills = allBills.Where(b => b.IsActive);
            
            if (startDate.HasValue)
            {
                bills = bills.Where(b => b.BillDate.Date >= startDate.Value.Date);
            }
            
            if (endDate.HasValue)
            {
                bills = bills.Where(b => b.BillDate.Date <= endDate.Value.Date);
            }
            
            var totalRevenue = bills.Sum(b => b.Amount);
            
            _logger.LogInformation("Total revenue: {TotalRevenue}", totalRevenue);
            return totalRevenue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while calculating total revenue");
            throw;
        }
    }

    public async Task<decimal> GetPendingAmountAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Calculating pending amount");
            var unpaidBills = await _billRepository.GetUnpaidAsync(cancellationToken);
            
            var pendingAmount = unpaidBills
                .Where(b => b.IsActive)
                .Sum(b => b.Amount);
            
            _logger.LogInformation("Total pending amount: {PendingAmount}", pendingAmount);
            return pendingAmount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while calculating pending amount");
            throw;
        }
    }

    public async Task<bool> ProcessPaymentAsync(int billId, decimal amount, string paymentMethod, CancellationToken cancellationToken = default)
    {
        try
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Payment amount must be greater than zero", nameof(amount));
            }

            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                throw new ArgumentException("Payment method cannot be null or empty", nameof(paymentMethod));
            }

            _logger.LogInformation("Processing payment of {Amount} for bill ID: {BillId}", amount, billId);
            
            var bill = await _billRepository.GetByIdAsync(billId, cancellationToken);
            if (bill == null)
            {
                _logger.LogWarning("Bill with ID {BillId} not found", billId);
                return false;
            }

            // Update bill with payment information
            // Note: This assumes Bill entity has properties for tracking payments
            // If the Bill entity doesn't have these properties, they would need to be added
            bill.IsPaid = amount >= bill.Amount;
            bill.ModifiedDate = DateTime.UtcNow;
            bill.ModifiedBy = "System"; // Should be replaced with actual user context
            
            await _billRepository.UpdateAsync(bill, cancellationToken);
            
            _logger.LogInformation("Successfully processed payment for bill ID: {BillId}", billId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing payment for bill ID: {BillId}", billId);
            throw;
        }
    }
}
