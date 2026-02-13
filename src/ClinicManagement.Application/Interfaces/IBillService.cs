using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.Interfaces;

public interface IBillService
{
    Task<IEnumerable<BillDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BillDto?> GetByIdAsync(int billId, CancellationToken cancellationToken = default);
    Task<BillDto> CreateAsync(BillCreateDto billCreateDto, CancellationToken cancellationToken = default);
    Task<BillDto> UpdateAsync(BillUpdateDto billUpdateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int billId, CancellationToken cancellationToken = default);
    
    // Entity-specific methods
    Task<IEnumerable<BillDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BillDto>> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BillDto>> GetByPaymentStatusAsync(string paymentStatus, CancellationToken cancellationToken = default);
    Task<IEnumerable<BillDto>> GetPendingBillsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BillDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<decimal> GetPendingAmountAsync(CancellationToken cancellationToken = default);
    Task<bool> ProcessPaymentAsync(int billId, decimal amount, string paymentMethod, CancellationToken cancellationToken = default);
}
