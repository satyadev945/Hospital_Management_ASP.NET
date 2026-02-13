using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Department> AddAsync(Department department, CancellationToken cancellationToken = default);
        Task<Department> UpdateAsync(Department department, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    }
}
