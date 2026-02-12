using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.DTOs
{
    public class PatientCreateDto
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
    }
}
