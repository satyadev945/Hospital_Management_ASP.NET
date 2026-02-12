using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.DTOs
{
    public class AppointmentCreateDto
    {
        [Required]
        public int DoctorID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
