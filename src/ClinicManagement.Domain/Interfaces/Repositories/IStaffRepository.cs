using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories
{
    public interface IStaffRepository
    {
        Task<IEnumerable<Staff>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Staff?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Staff> AddAsync(Staff staff, CancellationToken cancellationToken = default);
        Task<Staff> UpdateAsync(Staff staff, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Staff>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
