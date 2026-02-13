using ClinicManagement.Application.Interfaces;

namespace ClinicManagement.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ClinicDbContext _context;

    public UnitOfWork(ClinicDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
