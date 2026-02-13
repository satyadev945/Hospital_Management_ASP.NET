using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Bill entity
/// </summary>
public class BillRepository : IBillRepository
{
    private readonly ClinicManagementDbContext _context;
    private readonly ILogger<BillRepository> _logger;

    public BillRepository(ClinicManagementDbContext context, ILogger<BillRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all bills");
            return await _context.Bills
                .Include(b => b.Appointment)
                .Where(b => b.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all bills");
            throw;
        }
    }

    public async Task<Bill?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting bill by ID: {BillId}", id);
            return await _context.Bills
                .Include(b => b.Appointment)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bill by ID: {BillId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Bill>> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting bills by appointment ID: {AppointmentId}", appointmentId);
            return await _context.Bills
                .Include(b => b.Appointment)
                .Where(b => b.AppointmentId == appointmentId && b.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bills by appointment ID: {AppointmentId}", appointmentId);
            throw;
        }
    }

    public async Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new bill");
            bill.CreatedDate = DateTime.UtcNow;
            bill.IsActive = true;

            await _context.Bills.AddAsync(bill, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Bill added successfully: {BillId}", bill.Id);
            return bill;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding bill");
            throw;
        }
    }

    public async Task UpdateAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating bill: {BillId}", bill.Id);
            bill.ModifiedDate = DateTime.UtcNow;

            _context.Bills.Update(bill);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Bill updated successfully: {BillId}", bill.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bill: {BillId}", bill.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting bill: {BillId}", id);
            var bill = await _context.Bills.FindAsync(new object[] { id }, cancellationToken);

            if (bill != null)
            {
                bill.IsActive = false;
                bill.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Bill deleted successfully: {BillId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bill: {BillId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bills
                .AnyAsync(b => b.Id == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking bill existence: {BillId}", id);
            throw;
        }
    }
}
