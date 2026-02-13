using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default);
        Task<Patient> UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<Patient?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
