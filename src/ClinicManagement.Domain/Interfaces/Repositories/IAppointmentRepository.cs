using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
        Task<Appointment> UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Appointment>> GetPendingAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Appointment>> GetTodayAsync(CancellationToken cancellationToken = default);
    }
}
