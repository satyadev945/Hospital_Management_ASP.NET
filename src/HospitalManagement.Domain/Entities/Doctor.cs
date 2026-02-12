using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>
    /// Represents a doctor in the hospital management system
    /// </summary>
    public class Doctor
    {
        /// <summary>
        /// Unique identifier for the doctor (references LoginTable.LoginID)
        /// </summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Full name of the doctor
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
        /// Department number (foreign key)
        /// </summary>
        public int DeptNo { get; set; }

        /// <summary>
        /// Consultation fee per visit
        /// </summary>
        public decimal ChargesPerVisit { get; set; }

        /// <summary>
        /// Alias for ChargesPerVisit (for backward compatibility)
        /// </summary>
        public decimal Charges => ChargesPerVisit;

        /// <summary>
        /// Monthly salary of the doctor
        /// </summary>
        public decimal? MonthlySalary { get; set; }

        /// <summary>
        /// Reputation index based on patient feedback and performance
        /// </summary>
        public decimal? ReputeIndex { get; set; }

        /// <summary>
        /// Total number of patients treated by this doctor
        /// </summary>
        public int PatientsTreated { get; set; } = 0;

        /// <summary>
        /// Educational qualifications (degrees, certifications)
        /// </summary>
        public string Qualification { get; set; } = string.Empty;

        /// <summary>
        /// Area of medical specialization
        /// </summary>
        public string? Specialization { get; set; }

        /// <summary>
        /// Years of work experience
        /// </summary>
        public int? WorkExperience { get; set; }

        /// <summary>
        /// Employment status (1 = Active/Present, 0 = Inactive/Left)
        /// </summary>
        public int Status { get; set; } = 1;

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
        /// Indicates if the doctor record is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when the doctor record was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the doctor record was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the doctor record
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the doctor record
        /// </summary>
        public string? ModifiedBy { get; set; }

        // Navigation Properties

        /// <summary>
        /// Department to which this doctor belongs
        /// </summary>
        public virtual Department? Department { get; set; }

        /// <summary>
        /// Collection of appointments scheduled with this doctor
        /// </summary>
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        /// <summary>
        /// Collection of treatment records by this doctor
        /// </summary>
        public virtual ICollection<TreatmentHistory> TreatmentHistories { get; set; } = new List<TreatmentHistory>();

        /// <summary>
        /// Collection of feedback received by this doctor
        /// </summary>
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
