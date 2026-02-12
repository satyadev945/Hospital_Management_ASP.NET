using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly HospitalDbContext _context;

    public DepartmentRepository(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Departments.AsNoTracking().Where(d => d.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Departments.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Department> AddAsync(Department department, CancellationToken cancellationToken = default)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync(cancellationToken);
        return department;
    }

    public async Task UpdateAsync(Department department, CancellationToken cancellationToken = default)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var dept = await GetByIdAsync(id, cancellationToken);
        if (dept != null)
        {
            dept.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Departments.AnyAsync(d => d.DeptNo == id, cancellationToken);
    }

    public async Task<IEnumerable<Department>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Departments.AsNoTracking()
            .Where(d => d.IsActive && d.DeptName.Contains(searchTerm))
            .ToListAsync(cancellationToken);
    }
}
