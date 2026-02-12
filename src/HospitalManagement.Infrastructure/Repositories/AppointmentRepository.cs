using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;

namespace HospitalManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Appointment entity operations
    /// </summary>
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HospitalDbContext _context;
        private readonly ILogger<AppointmentRepository> _logger;

        public AppointmentRepository(HospitalDbContext context, ILogger<AppointmentRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving all appointments");
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                    .Where(a => a.IsActive)
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all appointments");
                throw;
            }
        }

        public async Task<Appointment> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving appointment with ID: {AppointmentId}", id);
                var appointment = await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                    .FirstOrDefaultAsync(a => a.AppointID == id && a.IsActive, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found", id);
                }

                return appointment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment with ID: {AppointmentId}", id);
                throw;
            }
        }

        public async Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            try
            {
                if (appointment == null)
                {
                    throw new ArgumentNullException(nameof(appointment));
                }

                _logger.LogInformation("Adding new appointment for Patient ID: {PatientId}, Doctor ID: {DoctorId}",
                    appointment.PatientID, appointment.DoctorID);

                appointment.CreatedDate = DateTime.UtcNow;
                appointment.IsActive = true;
                appointment.AppointmentStatus ??= 2; // Default to Pending
                appointment.DoctorNotification ??= 2; // Default to Unseen
                appointment.PatientNotification ??= 1; // Default to Seen (patient created it)
                appointment.FeedbackStatus ??= 2; // Default to Pending

                await _context.Appointments.AddAsync(appointment, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Appointment added successfully with ID: {AppointmentId}", appointment.AppointID);
                return appointment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding appointment for Patient ID: {PatientId}",
                    appointment?.PatientID);
                throw;
            }
        }

        public async Task<Appointment> UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            try
            {
                if (appointment == null)
                {
                    throw new ArgumentNullException(nameof(appointment));
                }

                _logger.LogInformation("Updating appointment with ID: {AppointmentId}", appointment.AppointID);

                var existingAppointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == appointment.AppointID, cancellationToken);

                if (existingAppointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for update", appointment.AppointID);
                    throw new InvalidOperationException($"Appointment with ID {appointment.AppointID} not found");
                }

                existingAppointment.DoctorID = appointment.DoctorID;
                existingAppointment.PatientID = appointment.PatientID;
                existingAppointment.Date = appointment.Date;
                existingAppointment.AppointmentStatus = appointment.AppointmentStatus;
                existingAppointment.BillAmount = appointment.BillAmount;
                existingAppointment.BillStatus = appointment.BillStatus;
                existingAppointment.Disease = appointment.Disease;
                existingAppointment.Progress = appointment.Progress;
                existingAppointment.Prescription = appointment.Prescription;
                existingAppointment.ModifiedDate = DateTime.UtcNow;
                existingAppointment.ModifiedBy = appointment.ModifiedBy;

                _context.Appointments.Update(existingAppointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Appointment updated successfully with ID: {AppointmentId}", appointment.AppointID);
                return existingAppointment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating appointment with ID: {AppointmentId}", appointment?.AppointID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting appointment with ID: {AppointmentId}", id);

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == id, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for deletion", id);
                    return false;
                }

                // Soft delete
                appointment.IsActive = false;
                appointment.ModifiedDate = DateTime.UtcNow;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Appointment deleted successfully with ID: {AppointmentId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting appointment with ID: {AppointmentId}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Checking if appointment exists with ID: {AppointmentId}", id);
                return await _context.Appointments
                    .AsNoTracking()
                    .AnyAsync(a => a.AppointID == id && a.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking appointment existence with ID: {AppointmentId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetAllAsync(cancellationToken);
                }

                _logger.LogInformation("Searching appointments with term: {SearchTerm}", searchTerm);

                var normalizedSearchTerm = searchTerm.ToLower().Trim();

                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.IsActive &&
                        ((a.Patient != null && a.Patient.Name.ToLower().Contains(normalizedSearchTerm)) ||
                         (a.Doctor != null && a.Doctor.Name.ToLower().Contains(normalizedSearchTerm))))
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching appointments with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving appointments for Patient ID: {PatientId}", patientId);
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                    .Where(a => a.IsActive && a.PatientID == patientId)
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments for Patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving appointments for Doctor ID: {DoctorId}", doctorId);
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Where(a => a.IsActive && a.DoctorID == doctorId)
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments for Doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetPendingAppointmentsByDoctorAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving pending appointments for Doctor ID: {DoctorId}", doctorId);
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Where(a => a.IsActive && a.DoctorID == doctorId && a.AppointmentStatus == 2)
                    .OrderBy(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending appointments for Doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetTodaysAppointmentsByDoctorAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving today's appointments for Doctor ID: {DoctorId}", doctorId);

                var today = DateTime.UtcNow.Date;
                var tomorrow = today.AddDays(1);

                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Where(a => a.IsActive &&
                        a.DoctorID == doctorId &&
                        a.Date.HasValue &&
                        a.Date.Value >= today &&
                        a.Date.Value < tomorrow)
                    .OrderBy(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving today's appointments for Doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<Appointment> GetCurrentAppointmentByPatientAsync(int patientId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving current appointment for Patient ID: {PatientId}", patientId);

                var today = DateTime.UtcNow.Date;
                var tomorrow = today.AddDays(1);

                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                    .Where(a => a.IsActive &&
                        a.PatientID == patientId &&
                        a.Date.HasValue &&
                        a.Date.Value >= today &&
                        a.Date.Value < tomorrow)
                    .OrderBy(a => a.Date)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current appointment for Patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetByStatusAsync(int status, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving appointments by status: {Status}", status);
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.IsActive && a.AppointmentStatus == status)
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments by status: {Status}", status);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving appointments for date: {Date}", date.Date);

                var startDate = date.Date;
                var endDate = startDate.AddDays(1);

                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.IsActive &&
                        a.Date.HasValue &&
                        a.Date.Value >= startDate &&
                        a.Date.Value < endDate)
                    .OrderBy(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments for date: {Date}", date);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving appointments between {StartDate} and {EndDate}", startDate, endDate);

                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.IsActive &&
                        a.Date.HasValue &&
                        a.Date.Value >= startDate &&
                        a.Date.Value <= endDate)
                    .OrderBy(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments between {StartDate} and {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<bool> ApproveAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Approving appointment ID: {AppointmentId}", appointmentId);

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == appointmentId, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for approval", appointmentId);
                    return false;
                }

                appointment.AppointmentStatus = 1; // Approved
                appointment.PatientNotification = 2; // Unseen by patient
                appointment.ModifiedDate = DateTime.UtcNow;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Appointment approved successfully with ID: {AppointmentId}", appointmentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving appointment with ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<bool> RejectAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Rejecting appointment ID: {AppointmentId}", appointmentId);

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == appointmentId, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for rejection", appointmentId);
                    return false;
                }

                appointment.AppointmentStatus = 4; // Rejected
                appointment.PatientNotification = 2; // Unseen by patient
                appointment.ModifiedDate = DateTime.UtcNow;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Appointment rejected successfully with ID: {AppointmentId}", appointmentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting appointment with ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<bool> CompleteAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Completing appointment ID: {AppointmentId}", appointmentId);

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == appointmentId, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for completion", appointmentId);
                    return false;
                }

                appointment.AppointmentStatus = 3; // Completed
                appointment.ModifiedDate = DateTime.UtcNow;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Appointment completed successfully with ID: {AppointmentId}", appointmentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing appointment with ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<bool> UpdatePrescriptionAsync(int appointmentId, string disease, string progress, string prescription, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating prescription for appointment ID: {AppointmentId}", appointmentId);

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == appointmentId, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for prescription update", appointmentId);
                    return false;
                }

                appointment.Disease = disease;
                appointment.Progress = progress;
                appointment.Prescription = prescription;
                appointment.ModifiedDate = DateTime.UtcNow;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Prescription updated successfully for appointment ID: {AppointmentId}", appointmentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating prescription for appointment ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<bool> UpdateBillAsync(int appointmentId, float billAmount, string billStatus, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating bill for appointment ID: {AppointmentId}", appointmentId);

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == appointmentId, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for bill update", appointmentId);
                    return false;
                }

                appointment.BillAmount = (decimal)billAmount;
                appointment.BillStatus = billStatus;
                appointment.ModifiedDate = DateTime.UtcNow;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Bill updated successfully for appointment ID: {AppointmentId}", appointmentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bill for appointment ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<bool> MarkBillAsPaidAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Marking bill as paid for appointment ID: {AppointmentId}", appointmentId);

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == appointmentId, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for bill payment", appointmentId);
                    return false;
                }

                appointment.BillStatus = "Paid";
                appointment.ModifiedDate = DateTime.UtcNow;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Bill marked as paid successfully for appointment ID: {AppointmentId}", appointmentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking bill as paid for appointment ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetUnpaidAppointmentsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving appointments with unpaid bills");
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.IsActive &&
                        a.BillStatus != null &&
                        a.BillStatus.ToLower() == "unpaid")
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments with unpaid bills");
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetUnpaidAppointmentsByPatientAsync(int patientId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving unpaid appointments for Patient ID: {PatientId}", patientId);
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Doctor)
                    .Where(a => a.IsActive &&
                        a.PatientID == patientId &&
                        a.BillStatus != null &&
                        a.BillStatus.ToLower() == "unpaid")
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unpaid appointments for Patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<bool> UpdateDoctorNotificationAsync(int appointmentId, int status, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating doctor notification for appointment ID: {AppointmentId} to status: {Status}",
                    appointmentId, status);

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == appointmentId, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for doctor notification update", appointmentId);
                    return false;
                }

                appointment.DoctorNotification = status;
                appointment.ModifiedDate = DateTime.UtcNow;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Doctor notification updated successfully for appointment ID: {AppointmentId}", appointmentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating doctor notification for appointment ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<bool> UpdatePatientNotificationAsync(int appointmentId, int status, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating patient notification for appointment ID: {AppointmentId} to status: {Status}",
                    appointmentId, status);

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == appointmentId, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for patient notification update", appointmentId);
                    return false;
                }

                appointment.PatientNotification = status;
                appointment.ModifiedDate = DateTime.UtcNow;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Patient notification updated successfully for appointment ID: {AppointmentId}", appointmentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient notification for appointment ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetUnseenDoctorNotificationsAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving unseen notifications for Doctor ID: {DoctorId}", doctorId);
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Where(a => a.IsActive && a.DoctorID == doctorId && a.DoctorNotification == 2)
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unseen notifications for Doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetUnseenPatientNotificationsAsync(int patientId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving unseen notifications for Patient ID: {PatientId}", patientId);
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Doctor)
                    .Where(a => a.IsActive && a.PatientID == patientId && a.PatientNotification == 2)
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unseen notifications for Patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<bool> UpdateFeedbackStatusAsync(int appointmentId, int status, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating feedback status for appointment ID: {AppointmentId} to status: {Status}",
                    appointmentId, status);

                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointID == appointmentId, cancellationToken);

                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found for feedback status update", appointmentId);
                    return false;
                }

                appointment.FeedbackStatus = status;
                appointment.ModifiedDate = DateTime.UtcNow;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Feedback status updated successfully for appointment ID: {AppointmentId}", appointmentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating feedback status for appointment ID: {AppointmentId}", appointmentId);
                throw;
            }
        }

        public async Task<Appointment> GetPendingFeedbackByPatientAsync(int patientId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving pending feedback appointment for Patient ID: {PatientId}", patientId);
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                    .Where(a => a.IsActive &&
                        a.PatientID == patientId &&
                        a.AppointmentStatus == 3 && // Completed
                        a.FeedbackStatus == 2) // Pending
                    .OrderBy(a => a.Date)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending feedback appointment for Patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetTreatmentHistoryByPatientAsync(int patientId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving treatment history for Patient ID: {PatientId}", patientId);
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                    .Where(a => a.IsActive &&
                        a.PatientID == patientId &&
                        a.AppointmentStatus == 3) // Completed
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving treatment history for Patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<Appointment>> GetBillHistoryByPatientAsync(int patientId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving bill history for Patient ID: {PatientId}", patientId);
                return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Doctor)
                    .Where(a => a.IsActive &&
                        a.PatientID == patientId &&
                        a.BillAmount.HasValue &&
                        a.BillAmount.Value > 0)
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bill history for Patient ID: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<DateTime>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving available time slots for Doctor ID: {DoctorId} on {Date}", doctorId, date.Date);

                var startDate = date.Date;
                var endDate = startDate.AddDays(1);

                var bookedSlots = await _context.Appointments
                    .AsNoTracking()
                    .Where(a => a.IsActive &&
                        a.DoctorID == doctorId &&
                        a.Date.HasValue &&
                        a.Date.Value >= startDate &&
                        a.Date.Value < endDate &&
                        a.AppointmentStatus != 4) // Not rejected
                    .Select(a => a.Date.Value)
                    .ToListAsync(cancellationToken);

                // Generate time slots (9 AM to 5 PM, hourly)
                var allSlots = new List<DateTime>();
                for (int hour = 9; hour <= 17; hour++)
                {
                    var slot = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                    allSlots.Add(slot);
                }

                // Filter out booked slots
                var availableSlots = allSlots
                    .Where(slot => !bookedSlots.Any(booked =>
                        booked.Hour == slot.Hour && booked.Date == slot.Date))
                    .ToList();

                _logger.LogInformation("Found {Count} available time slots for Doctor ID: {DoctorId}", availableSlots.Count, doctorId);
                return availableSlots;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available time slots for Doctor ID: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving total appointment count");
                return await _context.Appointments
                    .AsNoTracking()
                    .CountAsync(a => a.IsActive, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total appointment count");
                throw;
            }
        }

        public async Task<float> GetTotalIncomeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving total income from paid appointments");
                var totalIncome = await _context.Appointments
                    .AsNoTracking()
                    .Where(a => a.IsActive &&
                        a.BillStatus != null &&
                        a.BillStatus.ToLower() == "paid" &&
                        a.BillAmount.HasValue)
                    .SumAsync(a => a.BillAmount.Value, cancellationToken);

                _logger.LogInformation("Total income calculated: {TotalIncome}", totalIncome);
                return (float)totalIncome;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total income");
                throw;
            }
        }
    }
}
