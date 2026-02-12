using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.DTOs
{
    public class AppointmentUpdateDto
    {
        [Required]
        public int AppointID { get; set; }

        public int? DoctorID { get; set; }

        public int? PatientID { get; set; }

        public DateTime? Date { get; set; }

        public int? AppointmentStatus { get; set; }

        public decimal? BillAmount { get; set; }

        [StringLength(10)]
        public string BillStatus { get; set; }

        public int? DoctorNotification { get; set; }

        public int? PatientNotification { get; set; }

        public int? FeedbackStatus { get; set; }

        [StringLength(100)]
        public string Disease { get; set; }

        [StringLength(100)]
        public string Progress { get; set; }

        [StringLength(100)]
        public string Prescription { get; set; }
    }
}
