using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>
    /// Represents a patient in the hospital management system
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Unique identifier for the patient (references LoginTable.LoginID)
        /// </summary>
        public int PatientID { get; set; }

        /// <summary>
        /// Full name of the patient
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Residential address
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gender (M/F)
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Email address from login credentials
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password hash for authentication
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Calculated age based on birth date
        /// </summary>
        public int Age => DateTime.Now.Year - BirthDate.Year -
                         (DateTime.Now.DayOfYear < BirthDate.DayOfYear ? 1 : 0);

        /// <summary>
        /// Indicates if the patient record is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when the patient record was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the patient record was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the patient record
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the patient record
        /// </summary>
        public string? ModifiedBy { get; set; }

        // Navigation Properties

        /// <summary>
        /// Collection of appointments for this patient
        /// </summary>
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        /// <summary>
        /// Collection of bills associated with this patient
        /// </summary>
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

        /// <summary>
        /// Collection of treatment history records for this patient
        /// </summary>
        public virtual ICollection<TreatmentHistory> TreatmentHistories { get; set; } = new List<TreatmentHistory>();

        /// <summary>
        /// Collection of feedback provided by this patient
        /// </summary>
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
