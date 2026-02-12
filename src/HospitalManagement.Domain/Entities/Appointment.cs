using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>
    /// Represents an appointment between a patient and a doctor
    /// </summary>
    public class Appointment
    {
        /// <summary>
        /// Unique identifier for the appointment
        /// </summary>
        public int AppointID { get; set; }

        /// <summary>
        /// Doctor assigned to this appointment (foreign key)
        /// </summary>
        public int? DoctorID { get; set; }

        /// <summary>
        /// Patient who booked this appointment (foreign key)
        /// </summary>
        public int? PatientID { get; set; }

        /// <summary>
        /// Scheduled date and time of the appointment
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Status of the appointment
        /// 1 = Approved, 2 = Pending, 3 = Completed, 4 = Rejected
        /// </summary>
        public int? AppointmentStatus { get; set; }

        /// <summary>
        /// Total bill amount for this appointment
        /// </summary>
        public decimal? BillAmount { get; set; }

        /// <summary>
        /// Payment status of the bill (Paid/Unpaid)
        /// </summary>
        public string? BillStatus { get; set; }

        /// <summary>
        /// Doctor notification status
        /// 1 = Seen, 2 = Unseen
        /// </summary>
        public int? DoctorNotification { get; set; }

        /// <summary>
        /// Patient notification status
        /// 1 = Seen, 2 = Unseen
        /// </summary>
        public int? PatientNotification { get; set; }

        /// <summary>
        /// Feedback status for this appointment
        /// 1 = Given, 2 = Pending
        /// </summary>
        public int? FeedbackStatus { get; set; }

        /// <summary>
        /// Diagnosed disease or condition
        /// </summary>
        public string? Disease { get; set; }

        /// <summary>
        /// Patient's progress notes
        /// </summary>
        public string? Progress { get; set; }

        /// <summary>
        /// Prescribed medications and treatment
        /// </summary>
        public string? Prescription { get; set; }

        /// <summary>
        /// Indicates if the appointment record is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when the appointment was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the appointment was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the appointment record
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the appointment record
        /// </summary>
        public string? ModifiedBy { get; set; }

        // Navigation Properties

        /// <summary>
        /// Doctor associated with this appointment
        /// </summary>
        public virtual Doctor? Doctor { get; set; }

        /// <summary>
        /// Patient associated with this appointment
        /// </summary>
        public virtual Patient? Patient { get; set; }

        /// <summary>
        /// Bill generated for this appointment
        /// </summary>
        public virtual Bill? Bill { get; set; }

        /// <summary>
        /// Treatment history record for this appointment
        /// </summary>
        public virtual TreatmentHistory? TreatmentHistory { get; set; }

        /// <summary>
        /// Feedback provided for this appointment
        /// </summary>
        public virtual Feedback? Feedback { get; set; }

        // Helper Methods

        /// <summary>
        /// Checks if the appointment is pending approval
        /// </summary>
        public bool IsPending => AppointmentStatus == 2;

        /// <summary>
        /// Checks if the appointment is approved
        /// </summary>
        public bool IsApproved => AppointmentStatus == 1;

        /// <summary>
        /// Checks if the appointment is completed
        /// </summary>
        public bool IsCompleted => AppointmentStatus == 3;

        /// <summary>
        /// Checks if the appointment is rejected
        /// </summary>
        public bool IsRejected => AppointmentStatus == 4;

        /// <summary>
        /// Checks if the bill has been paid
        /// </summary>
        public bool IsBillPaid => BillStatus?.Equals("Paid", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
