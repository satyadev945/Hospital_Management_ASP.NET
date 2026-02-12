using System;
using System.Collections.Generic;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>
    /// Represents a medical department in the hospital
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Unique identifier for the department
        /// </summary>
        public int DeptNo { get; set; }

        /// <summary>
        /// Name of the department
        /// </summary>
        public string DeptName { get; set; } = string.Empty;

        /// <summary>
        /// Alias for DeptName (for backward compatibility)
        /// </summary>
        public string DepartmentName
        {
            get => DeptName;
            set => DeptName = value;
        }

        /// <summary>
        /// Detailed description of the department's services and specialties
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Indicates if the department is currently active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when the department was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the department was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the department record
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the department record
        /// </summary>
        public string? ModifiedBy { get; set; }

        // Navigation Properties

        /// <summary>
        /// Collection of doctors assigned to this department
        /// </summary>
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
