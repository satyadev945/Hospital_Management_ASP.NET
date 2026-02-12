using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class OtherStaffRepository : IOtherStaffRepository
{
    private readonly HospitalDbContext _context;

    public OtherStaffRepository(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OtherStaff>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OtherStaff.AsNoTracking().Where(s => s.Status).ToListAsync(cancellationToken);
    }

    public async Task<OtherStaff?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.OtherStaff.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<OtherStaff> AddAsync(OtherStaff staff, CancellationToken cancellationToken = default)
    {
        _context.OtherStaff.Add(staff);
        await _context.SaveChangesAsync(cancellationToken);
        return staff;
    }

    public async Task UpdateAsync(OtherStaff staff, CancellationToken cancellationToken = default)
    {
        _context.OtherStaff.Update(staff);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var staff = await GetByIdAsync(id, cancellationToken);
        if (staff != null)
        {
            staff.Status = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.OtherStaff.AnyAsync(s => s.StaffID == id, cancellationToken);
    }

    public async Task<IEnumerable<OtherStaff>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.OtherStaff.AsNoTracking()
            .Where(s => s.Status && s.Name.Contains(searchTerm))
            .ToListAsync(cancellationToken);
    }
}
