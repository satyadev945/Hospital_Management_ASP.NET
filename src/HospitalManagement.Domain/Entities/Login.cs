using System;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>
    /// Represents login credentials for system users
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Unique identifier for the login record
        /// </summary>
        public int LoginID { get; set; }

        /// <summary>
        /// User email address (unique)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password for authentication
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// User type identifier
        /// 1 = Patient, 2 = Doctor, 3 = Admin, 4 = Staff
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Indicates if the login account is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when the login record was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the login record was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Last login date and time
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Failed login attempts counter
        /// </summary>
        public int FailedLoginAttempts { get; set; } = 0;

        /// <summary>
        /// Account lockout timestamp
        /// </summary>
        public DateTime? LockoutEnd { get; set; }

        /// <summary>
        /// User who created the login record
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the login record
        /// </summary>
        public string? ModifiedBy { get; set; }

        // Helper Methods

        /// <summary>
        /// Checks if the account is locked out
        /// </summary>
        public bool IsLockedOut => LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;

        /// <summary>
        /// Checks if the user is a patient
        /// </summary>
        public bool IsPatient => Type == 1;

        /// <summary>
        /// Checks if the user is a doctor
        /// </summary>
        public bool IsDoctor => Type == 2;

        /// <summary>
        /// Checks if the user is an admin
        /// </summary>
        public bool IsAdmin => Type == 3;

        /// <summary>
        /// Checks if the user is staff
        /// </summary>
        public bool IsStaff => Type == 4;
    }
}
