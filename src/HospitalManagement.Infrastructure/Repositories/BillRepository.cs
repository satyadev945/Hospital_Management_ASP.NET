using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class BillRepository : IBillRepository
{
    private readonly HospitalDbContext _context;

    public BillRepository(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Bills.AsNoTracking().Include(b => b.Patient).ToListAsync(cancellationToken);
    }

    public async Task<Bill?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Bills.Include(b => b.Patient).FirstOrDefaultAsync(b => b.BillID == id, cancellationToken);
    }

    public async Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        _context.Bills.Add(bill);
        await _context.SaveChangesAsync(cancellationToken);
        return bill;
    }

    public async Task UpdateAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        _context.Bills.Update(bill);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var bill = await GetByIdAsync(id, cancellationToken);
        if (bill != null)
        {
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Bills.AnyAsync(b => b.BillID == id, cancellationToken);
    }

    public async Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Bills.AsNoTracking().Where(b => b.PatientID == patientId).ToListAsync(cancellationToken);
    }

    public async Task<Bill?> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Bills.FirstOrDefaultAsync(b => b.AppointmentID == appointmentId, cancellationToken);
    }
}
