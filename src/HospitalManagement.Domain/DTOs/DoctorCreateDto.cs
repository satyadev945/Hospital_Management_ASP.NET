using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.DTOs
{
    public class DoctorCreateDto
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [StringLength(11)]
        public string Phone { get; set; }

        [StringLength(40)]
        public string Address { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public char Gender { get; set; }

        [Required]
        public int DeptNo { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ChargesPerVisit { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MonthlySalary { get; set; }

        [Required]
        [StringLength(100)]
        public string Qualification { get; set; }

        [StringLength(100)]
        public string Specialization { get; set; }

        [Range(0, 100)]
        public int? WorkExperience { get; set; }
    }
}
