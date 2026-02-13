using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default);
        Task<Doctor> UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<Doctor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<Doctor>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
    }
}
