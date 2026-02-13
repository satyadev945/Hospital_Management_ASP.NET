using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Bill entity operations.
/// Provides data access methods with error handling and logging.
/// </summary>
public class BillRepository : IBillRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<BillRepository> _logger;

    public BillRepository(ClinicDbContext context, ILogger<BillRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Bill?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving bill with ID: {BillId}", id);

            var bill = await _context.Bills
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .ThenInclude(a => a!.Doctor)
                .FirstOrDefaultAsync(b => b.BillId == id, cancellationToken);

            if (bill == null)
            {
                _logger.LogWarning("Bill with ID {BillId} not found", id);
            }

            return bill;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bill with ID: {BillId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all bills");

            var bills = await _context.Bills
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .ThenInclude(a => a!.Doctor)
                .OrderByDescending(b => b.BillDate)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Retrieved {Count} bills", bills.Count);
            return bills;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all bills");
            throw;
        }
    }

    public async Task<Bill> AddAsync(Bill entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding new bill for Patient ID: {PatientId} with amount: {Amount}", 
                entity.PatientId, entity.TotalAmount);

            await _context.Bills.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added bill with ID: {BillId}", entity.BillId);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding bill for Patient ID: {PatientId}", 
                entity.PatientId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding bill for Patient ID: {PatientId}", entity.PatientId);
            throw;
        }
    }

    public async Task UpdateAsync(Bill entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating bill with ID: {BillId}", entity.BillId);

            _context.Bills.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated bill with ID: {BillId}", entity.BillId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating bill with ID: {BillId}", 
                entity.BillId);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating bill with ID: {BillId}", 
                entity.BillId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bill with ID: {BillId}", entity.BillId);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting bill with ID: {BillId}", id);

            var bill = await _context.Bills.FindAsync(new object[] { id }, cancellationToken);
            if (bill == null)
            {
                _logger.LogWarning("Bill with ID {BillId} not found for deletion", id);
                throw new KeyNotFoundException($"Bill with ID {id} not found");
            }

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted bill with ID: {BillId}", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while deleting bill with ID: {BillId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bill with ID: {BillId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving bills for patient ID: {PatientId}", patientId);

            var bills = await _context.Bills
                .Where(b => b.PatientId == patientId)
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .ThenInclude(a => a!.Doctor)
                .OrderByDescending(b => b.BillDate)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} bills for patient ID: {PatientId}", bills.Count, patientId);
            return bills;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bills for patient ID: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetByPaymentStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                _logger.LogWarning("Empty payment status provided for bill search");
                return Enumerable.Empty<Bill>();
            }

            _logger.LogDebug("Retrieving bills with payment status: {Status}", status);

            var bills = await _context.Bills
                .Where(b => b.PaymentStatus == status)
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .ThenInclude(a => a!.Doctor)
                .OrderByDescending(b => b.BillDate)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} bills with payment status: {Status}", bills.Count, status);
            return bills;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bills with payment status: {Status}", status);
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetPendingBillsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving pending bills");

            var bills = await _context.Bills
                .Where(b => b.PaymentStatus == "Pending" || b.PaymentStatus == "Partial")
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .ThenInclude(a => a!.Doctor)
                .OrderBy(b => b.DueDate)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} pending bills", bills.Count);
            return bills;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending bills");
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetOverdueBillsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving overdue bills");

            var today = DateTime.Today;
            var bills = await _context.Bills
                .Where(b => b.DueDate < today && 
                           (b.PaymentStatus == "Pending" || b.PaymentStatus == "Partial"))
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .ThenInclude(a => a!.Doctor)
                .OrderBy(b => b.DueDate)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} overdue bills", bills.Count);
            return bills;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving overdue bills");
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving bills between {StartDate} and {EndDate}", startDate, endDate);

            var bills = await _context.Bills
                .Where(b => b.BillDate.Date >= startDate.Date && b.BillDate.Date <= endDate.Date)
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .ThenInclude(a => a!.Doctor)
                .OrderBy(b => b.BillDate)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("Found {Count} bills between {StartDate} and {EndDate}", 
                bills.Count, startDate, endDate);
            return bills;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bills between {StartDate} and {EndDate}", 
                startDate, endDate);
            throw;
        }
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Calculating total revenue between {StartDate} and {EndDate}", 
                startDate, endDate);

            var query = _context.Bills.Where(b => b.PaymentStatus == "Paid");

            if (startDate.HasValue)
            {
                query = query.Where(b => b.PaymentDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(b => b.PaymentDate <= endDate.Value);
            }

            var totalRevenue = await query.SumAsync(b => b.PaidAmount, cancellationToken);

            _logger.LogDebug("Total revenue calculated: {TotalRevenue}", totalRevenue);
            return totalRevenue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total revenue");
            throw;
        }
    }

    public async Task<decimal> GetOutstandingAmountAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Calculating total outstanding amount");

            var outstandingAmount = await _context.Bills
                .Where(b => b.PaymentStatus == "Pending" || b.PaymentStatus == "Partial")
                .SumAsync(b => b.TotalAmount - b.PaidAmount, cancellationToken);

            _logger.LogDebug("Total outstanding amount calculated: {OutstandingAmount}", outstandingAmount);
            return outstandingAmount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total outstanding amount");
            throw;
        }
    }
}
