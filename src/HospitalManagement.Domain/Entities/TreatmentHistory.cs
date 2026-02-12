using System;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>
    /// Represents the medical treatment history for a patient
    /// </summary>
    public class TreatmentHistory
    {
        /// <summary>
        /// Unique identifier for the treatment history record
        /// </summary>
        public int TreatmentHistoryID { get; set; }

        /// <summary>
        /// Appointment associated with this treatment (foreign key)
        /// </summary>
        public int AppointID { get; set; }

        /// <summary>
        /// Patient who received the treatment (foreign key)
        /// </summary>
        public int PatientID { get; set; }

        /// <summary>
        /// Doctor who provided the treatment (foreign key)
        /// </summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Date when the treatment was provided
        /// </summary>
        public DateTime TreatmentDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Diagnosed disease or medical condition
        /// </summary>
        public string? Disease { get; set; }

        /// <summary>
        /// Symptoms reported by the patient
        /// </summary>
        public string? Symptoms { get; set; }

        /// <summary>
        /// Diagnosis details
        /// </summary>
        public string? Diagnosis { get; set; }

        /// <summary>
        /// Treatment plan and procedures performed
        /// </summary>
        public string? TreatmentPlan { get; set; }

        /// <summary>
        /// Prescribed medications
        /// </summary>
        public string? Prescription { get; set; }

        /// <summary>
        /// Patient's progress and response to treatment
        /// </summary>
        public string? Progress { get; set; }

        /// <summary>
        /// Follow-up instructions
        /// </summary>
        public string? FollowUpInstructions { get; set; }

        /// <summary>
        /// Next follow-up appointment date
        /// </summary>
        public DateTime? NextFollowUpDate { get; set; }

        /// <summary>
        /// Laboratory test results
        /// </summary>
        public string? LabResults { get; set; }

        /// <summary>
        /// Vital signs recorded (BP, temperature, pulse, etc.)
        /// </summary>
        public string? VitalSigns { get; set; }

        /// <summary>
        /// Additional notes or observations
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Indicates if the treatment record is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when the treatment history was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the treatment history was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the treatment history record
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the treatment history record
        /// </summary>
        public string? ModifiedBy { get; set; }

        // Navigation Properties

        /// <summary>
        /// Appointment associated with this treatment
        /// </summary>
        public virtual Appointment? Appointment { get; set; }

        /// <summary>
        /// Patient who received the treatment
        /// </summary>
        public virtual Patient? Patient { get; set; }

        /// <summary>
        /// Doctor who provided the treatment
        /// </summary>
        public virtual Doctor? Doctor { get; set; }

        // Helper Methods

        /// <summary>
        /// Checks if a follow-up appointment is due
        /// </summary>
        public bool IsFollowUpDue => NextFollowUpDate.HasValue &&
                                      NextFollowUpDate.Value <= DateTime.UtcNow;

        /// <summary>
        /// Gets the number of days since treatment
        /// </summary>
        public int DaysSinceTreatment => (DateTime.UtcNow - TreatmentDate).Days;
    }
}
