using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.DTOs
{
    public class DoctorUpdateDto
    {
        [Required]
        public int DoctorID { get; set; }

        /// <summary>
        /// Alias for DoctorID (for backward compatibility)
        /// </summary>
        public int DoctorId
        {
            get => DoctorID;
            set => DoctorID = value;
        }

        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(11)]
        public string Phone { get; set; }

        [StringLength(40)]
        public string Address { get; set; }

        public DateTime? BirthDate { get; set; }

        public char? Gender { get; set; }

        public int? DeptNo { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ChargesPerVisit { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MonthlySalary { get; set; }

        [StringLength(100)]
        public string Qualification { get; set; }

        [StringLength(100)]
        public string Specialization { get; set; }

        [Range(0, 100)]
        public int? WorkExperience { get; set; }

        public int? Status { get; set; }
    }
}
