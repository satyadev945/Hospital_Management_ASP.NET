using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories
{
    public interface IBillRepository
    {
        Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Bill?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default);
        Task<Bill> UpdateAsync(Bill bill, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Bill?> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Bill>> GetUnpaidAsync(CancellationToken cancellationToken = default);
    }
}
