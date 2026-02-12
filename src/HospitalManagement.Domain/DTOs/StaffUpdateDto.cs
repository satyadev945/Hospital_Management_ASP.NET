using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.DTOs
{
    public class StaffUpdateDto
    {
        [Required]
        public int StaffID { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(11)]
        public string Phone { get; set; }

        [StringLength(30)]
        public string Address { get; set; }

        [StringLength(15)]
        public string Designation { get; set; }

        public char? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(50)]
        public string HighestQualification { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Salary { get; set; }
    }
}
