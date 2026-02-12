using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.DTOs
{
    public class PatientUpdateDto
    {
        [Required]
        public int PatientID { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(11)]
        public string Phone { get; set; }

        [StringLength(40)]
        public string Address { get; set; }

        public DateTime? BirthDate { get; set; }

        public char? Gender { get; set; }
    }
}
