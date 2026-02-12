using System;

namespace HospitalManagement.Domain.DTOs
{
    public class AppointmentDto
    {
        public int AppointID { get; set; }
        public int? DoctorID { get; set; }
        public string DoctorName { get; set; }
        public int? PatientID { get; set; }
        public string PatientName { get; set; }
        public DateTime? Date { get; set; }
        public int? AppointmentStatus { get; set; }
        public string AppointmentStatusText { get; set; }
        public decimal? BillAmount { get; set; }
        public string BillStatus { get; set; }
        public int? DoctorNotification { get; set; }
        public int? PatientNotification { get; set; }
        public int? FeedbackStatus { get; set; }
        public string Disease { get; set; }
        public string Progress { get; set; }
        public string Prescription { get; set; }
    }
}
