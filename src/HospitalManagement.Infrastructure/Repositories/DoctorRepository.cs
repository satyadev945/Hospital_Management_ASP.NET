using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<DoctorRepository> _logger;

    public DoctorRepository(HospitalDbContext context, ILogger<DoctorRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.AsNoTracking().Include(d => d.Department).Where(d => d.Status).ToListAsync(cancellationToken);
    }

    public async Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.Include(d => d.Department).FirstOrDefaultAsync(d => d.DoctorID == id, cancellationToken);
    }

    public async Task<Doctor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.FirstOrDefaultAsync(d => d.Email == email, cancellationToken);
    }

    public async Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync(cancellationToken);
        return doctor;
    }

    public async Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var doctor = await GetByIdAsync(id, cancellationToken);
        if (doctor != null)
        {
            doctor.Status = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.AnyAsync(d => d.DoctorID == id, cancellationToken);
    }

    public async Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.AsNoTracking().Include(d => d.Department)
            .Where(d => d.Status && d.Name.Contains(searchTerm))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Doctor>> GetByDepartmentAsync(int deptNo, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.AsNoTracking().Include(d => d.Department)
            .Where(d => d.DeptNo == deptNo && d.Status)
            .ToListAsync(cancellationToken);
    }

    public async Task<Doctor?> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.FirstOrDefaultAsync(d => d.Email == email && d.Status, cancellationToken);
    }
}
