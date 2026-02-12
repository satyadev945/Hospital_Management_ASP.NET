using System;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>
    /// Represents non-medical staff members in the hospital
    /// </summary>
    public class OtherStaff
    {
        /// <summary>
        /// Unique identifier for the staff member
        /// </summary>
        public int StaffID { get; set; }

        /// <summary>
        /// Full name of the staff member
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
        /// Job designation or title
        /// </summary>
        public string Designation { get; set; } = string.Empty;

        /// <summary>
        /// Gender (M/F)
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Date of birth
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Highest educational qualification
        /// </summary>
        public string? HighestQualification { get; set; }

        /// <summary>
        /// Monthly salary
        /// </summary>
        public decimal? Salary { get; set; }

        /// <summary>
        /// Date when the staff member joined
        /// </summary>
        public DateTime? JoiningDate { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Emergency contact number
        /// </summary>
        public string? EmergencyContact { get; set; }

        /// <summary>
        /// Emergency contact person name
        /// </summary>
        public string? EmergencyContactName { get; set; }

        /// <summary>
        /// Department or section where staff is assigned
        /// </summary>
        public string? Department { get; set; }

        /// <summary>
        /// Work shift (Morning/Evening/Night)
        /// </summary>
        public string? Shift { get; set; }

        /// <summary>
        /// Employment type (Full-time/Part-time/Contract)
        /// </summary>
        public string? EmploymentType { get; set; }

        /// <summary>
        /// Employee ID or badge number
        /// </summary>
        public string? EmployeeID { get; set; }

        /// <summary>
        /// National ID or passport number
        /// </summary>
        public string? NationalID { get; set; }

        /// <summary>
        /// Blood group
        /// </summary>
        public string? BloodGroup { get; set; }

        /// <summary>
        /// Employment status (Active/On Leave/Terminated)
        /// </summary>
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Date of termination or resignation (if applicable)
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// Reason for termination or resignation
        /// </summary>
        public string? TerminationReason { get; set; }

        /// <summary>
        /// Additional notes or remarks
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Calculated age based on birth date
        /// </summary>
        public int? Age => BirthDate.HasValue
            ? DateTime.Now.Year - BirthDate.Value.Year -
              (DateTime.Now.DayOfYear < BirthDate.Value.DayOfYear ? 1 : 0)
            : null;

        /// <summary>
        /// Indicates if the staff record is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when the staff record was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the staff record was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the staff record
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the staff record
        /// </summary>
        public string? ModifiedBy { get; set; }

        // Helper Methods

        /// <summary>
        /// Checks if the staff member is currently employed
        /// </summary>
        public bool IsCurrentlyEmployed => Status?.Equals("Active", StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>
        /// Calculates years of service
        /// </summary>
        public int? YearsOfService => JoiningDate.HasValue
            ? DateTime.UtcNow.Year - JoiningDate.Value.Year
            : null;

        /// <summary>
        /// Checks if the staff member is on probation (less than 6 months)
        /// </summary>
        public bool IsOnProbation => JoiningDate.HasValue &&
                                      (DateTime.UtcNow - JoiningDate.Value).TotalDays < 180;
    }
}
