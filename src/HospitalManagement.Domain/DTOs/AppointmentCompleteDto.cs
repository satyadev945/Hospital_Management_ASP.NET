using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.DTOs
{
    public class AppointmentCompleteDto
    {
        [Required]
        public int AppointID { get; set; }

        [Required]
        public int DoctorID { get; set; }

        [StringLength(100)]
        public string Disease { get; set; }

        [StringLength(100)]
        public string Progress { get; set; }

        [StringLength(100)]
        public string Prescription { get; set; }

        [Required]
        public bool IsBillPaid { get; set; }
    }
}
